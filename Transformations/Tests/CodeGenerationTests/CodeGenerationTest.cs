using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models.Meta;
using NMF.Serialization;
using NMF.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.CodeGenerationTests
{
    public static class CodeGenerationTest
    {
        private static readonly object codeGenLock = new object();

        public static int GenerateAndCompile(INamespace ns, Action<string, int> afterCompileAction, out string error, out string log)
        {
            lock (codeGenLock)
            {
                var path = Path.GetTempFileName();
                File.Delete(path);
                Directory.CreateDirectory(path);

                try
                {
                    var projectFile = Path.Combine(path, "project.csproj");
                    var codeFile = Path.Combine(path, "code.cs");

                    return GenerateAndCompileCore(ns, afterCompileAction, out error, out log, path, projectFile, codeFile);
                }
                finally
                {
                    try
                    {
                        Directory.Delete(path, true);
                    }
                    // In code coverage analysis, another process is usually accessing our temporary folder
                    catch (Exception) { }
                }
            }
        }

        public static int GenerateAndCompile(INamespace ns, Action<string, int> afterCompileAction, string workingDirectory, string projectFile, string codeFile, out string log, out string error)
        {
            lock (codeGenLock)
            {
                return GenerateAndCompileCore(ns, afterCompileAction, out error, out log, workingDirectory, projectFile, codeFile);
            }
        }

        private static int GenerateAndCompileCore(INamespace ns, Action<string, int> afterCompileAction, out string error, out string log, string workingDirectory, string projectFile, string codeFile)
        {
            var code = MetaFacade.CreateCode(ns, "TemporaryGeneratedCode");
            MetaFacade.GenerateCode(code, new Microsoft.CSharp.CSharpCodeProvider(), codeFile, false);

            File.WriteAllText(projectFile, GenerateProjectFile(codeFile));

            var startInfo = new ProcessStartInfo("dotnet.exe", "build " + projectFile);
            startInfo.CreateNoWindow = true;
            startInfo.ErrorDialog = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WorkingDirectory = workingDirectory;

            var buildJob = Process.Start(startInfo);
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            buildJob.ErrorDataReceived += (o, e) =>
            {
                errorBuilder.AppendLine(e.Data);
                Debug.WriteLine(e.Data);
            };
            var line = buildJob.StandardOutput.ReadLine();
            while (line != null)
            {
                outputBuilder.AppendLine(line);
                line = buildJob.StandardOutput.ReadLine();
            }
            buildJob.WaitForExit();
            error = errorBuilder.ToString();
            log = outputBuilder.ToString();
            afterCompileAction?.Invoke(workingDirectory, buildJob.ExitCode);
            return buildJob.ExitCode;
        }

        private static string GenerateProjectFile(params string[] codeFiles)
        {
            var compileFilesSnippet = string.Concat(codeFiles.Select(file => Environment.NewLine + string.Format("   <Compile Include=\"{0}\" />", Path.GetFileName(file))));
            var collectionsPath = (typeof(ObservableSet<>)).Assembly.Location;
            var expressionsPath = (typeof(INotifyExpression)).Assembly.Location;
            var expressionsLinqPath = (typeof(ExpressionExtensions)).Assembly.Location;
            var modelsPath = typeof(NMF.Models.Model).Assembly.Location;
            var serializationsPath = typeof(XmlSerializer).Assembly.Location;
            var utilitiesPath = typeof(MemoizedFunc<,>).Assembly.Location;

            using (var projectTemplateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NMF.CodeGenerationTests.ProjectTemplate.csproj"))
            {
                using (var sr = new StreamReader(projectTemplateStream))
                {
                    var projectTemplate = sr.ReadToEnd();

                    return projectTemplate
                        .Replace("%NMFCollectionsPath%", collectionsPath)
                        .Replace("%NMFExpressionsPath%", expressionsPath)
                        .Replace("%NMFExpressionsLinqPath%", expressionsLinqPath)
                        .Replace("%NMFModelsPath%", modelsPath)
                        .Replace("%NMFSerializationPath%", serializationsPath)
                        .Replace("%NMFUtilitiesPath%", utilitiesPath)
                        .Replace("%CompileFiles%", compileFilesSnippet);
                }
            }
        }
    }
}
