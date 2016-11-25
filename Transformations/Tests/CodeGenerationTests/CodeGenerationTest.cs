using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models.Meta;
using NMF.Serialization;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.CodeGenerationTests
{
    public static class CodeGenerationTest
    {
        private static object codeGenLock = new object();

        public static int GenerateAndCompile(INamespace ns, out string error, out string log)
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

                    var code = MetaFacade.CreateCode(ns, "Test");
                    MetaFacade.GenerateCode(code, new Microsoft.CSharp.CSharpCodeProvider(), codeFile, false);

                    File.WriteAllText(projectFile, GenerateProjectFile(codeFile));

                    var startInfo = new ProcessStartInfo("MSBuild.exe", projectFile);
                    startInfo.CreateNoWindow = true;
                    startInfo.ErrorDialog = false;
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardError = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.WorkingDirectory = path;

                    var buildJob = Process.Start(startInfo);
                    string buildLog = null;

                    buildJob.ErrorDataReceived += (o, e) =>
                    {
                        buildLog += e.Data;
                        Debug.WriteLine(e.Data);
                    };
                    log = null;
                    var line = buildJob.StandardOutput.ReadLine();
                    while (line != null)
                    {
                        log += line;
                        line = buildJob.StandardOutput.ReadLine();
                    }
                    buildJob.WaitForExit();
                    error = buildLog;
                    return buildJob.ExitCode;
                }
                finally
                {
                    Directory.Delete(path, true);
                }
            }
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
