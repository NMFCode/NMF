/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License. A  copy of the license can be found in the License.html file at the
 * root of this distribution. If  you cannot locate the  Microsoft Public
 * License, please send an email to  dlr@microsoft.com. By using this source
 * code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;

using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;


namespace PythonCodeGenerator.CodeDom
{
    /// <summary>
    /// Compiler half of the Python CodeDom Generator.
    /// </summary>
    partial class PythonGenerator
    {
        #region CodeCompiler overrides
        protected override string FileExtension
        {
            get { return "py"; }
        }

        protected override string CompilerName
        {
            get { return "IronPython"; }
        }

        protected override void ProcessCompilerOutputLine(CompilerResults results, string line)
        {
            // gets called from base classes FromFileBatch - which is never invoked because we override it
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        protected override string CmdArgsFromParameters(CompilerParameters options)
        {
            // gets called from base classes FromFileBatch - which is never invoked because we override it
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        protected override CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
        {
            return FromFileWorker(options, fileNames);
        }

        protected override CompilerResults FromFile(CompilerParameters options, string fileName)
        {
            return FromFileWorker(options, fileName);
        }

        protected override CompilerResults FromDom(CompilerParameters options, CodeCompileUnit e)
        {
            return FromDomWorker(options, e);
        }

        protected override CompilerResults FromDomBatch(CompilerParameters options, CodeCompileUnit[] ea)
        {
            return FromDomWorker(options, ea);
        }

        protected override CompilerResults FromSource(CompilerParameters options, string source)
        {
            return FromSourceWorker(options, source);
        }

        protected override CompilerResults FromSourceBatch(CompilerParameters options, string[] sources)
        {
            return FromSourceWorker(options, sources);
        }
        #endregion

        #region Private implementation details
        private static CompilerResults FromFileWorker(CompilerParameters options, params string[] files)
        {
            CompilerResults res = new CompilerResults(options.TempFiles);

            PEFileKinds targetKind;
            if (options.OutputAssembly != null)
            {
                if (options.OutputAssembly.ToLower().EndsWith(".exe"))
                {
                    targetKind = PEFileKinds.WindowApplication;
                }
                else
                {
                    targetKind = options.GenerateExecutable ? PEFileKinds.WindowApplication : PEFileKinds.Dll;
                }
            }
            else
            {
                targetKind = PEFileKinds.WindowApplication;
            }

            // The new domain needs to be set up with the same ApplicationBase and PrivateBinPath
            // as the current domain to make sure that the python assembly is loadable from there
            AppDomainSetup currentDomainSetup = AppDomain.CurrentDomain.SetupInformation;
            AppDomainSetup newDomainSetup = new AppDomainSetup();
            newDomainSetup.ApplicationBase = currentDomainSetup.ApplicationBase;
            newDomainSetup.PrivateBinPath = currentDomainSetup.PrivateBinPath;

            AppDomain compileDomain = null;
            try
            {
                compileDomain = AppDomain.CreateDomain("compilation domain", null, newDomainSetup);

                // This is a horrible hack: When running in a multi app domain scenario, where
                // IronPython has not been strong named, and exists on both sides of the app domain
                // boundary, we need some common type that both sides can agree on.  That common
                // type needs to live in the GAC on both sides.  So we abuse IReflect and use it
                // as our communication channel across the app domain boundary.
                IReflect rc = (IReflect)compileDomain.CreateInstanceFromAndUnwrap(
                    Assembly.GetExecutingAssembly().Location,
                    "IronPython.CodeDom.RemoteCompiler",
                    false,
                    BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance,
                    null,
                    new object[] { files, options.OutputAssembly, options.IncludeDebugInformation, options.ReferencedAssemblies, targetKind, options.MainClass, options.EmbeddedResources },
                    null,
                    null,
                    null);

                //rc.Initialize(files, options.OutputAssembly, options.IncludeDebugInformation, options.ReferencedAssemblies, targetKind);

                InvokeCompiler(rc, "Compile");

                res.NativeCompilerReturnValue = (int)InvokeCompiler(rc, "get_ErrorCount");
                List<CompilerError> errors = (List<CompilerError>)(InvokeCompiler(rc, "get_Errors"));
                for (int i = 0; i < errors.Count; i++)
                {
                    res.Errors.Add(errors[i]);
                }
                try
                {
                    if (options.GenerateInMemory)
                        res.CompiledAssembly = (Assembly)InvokeCompiler(rc, "get_Assembly");
                    else
                        res.PathToAssembly = options.OutputAssembly;
                }
                catch
                {
                }
            }
            finally
            {
                if (compileDomain != null) AppDomain.Unload(compileDomain);
            }

            return res;
        }

        private static object InvokeCompiler(IReflect compiler, string api)
        {
            return compiler.InvokeMember(api, BindingFlags.Public, null, null, null, null, null, null);
        }

        private static CompilerResults FromSourceWorker(CompilerParameters options, params string[] sources)
        {
            string[] tempFiles = new string[sources.Length];
            for (int i = 0; i < tempFiles.Length; i++)
            {
                tempFiles[i] = options.TempFiles.AddExtension("py", true);
                using (StreamWriter sw = new StreamWriter(tempFiles[i]))
                {
                    sw.Write(sources[i]);
                }
            }

            return FromFileWorker(options, tempFiles);
        }

        private static CompilerResults FromDomWorker(CompilerParameters options, params CodeCompileUnit[] ea)
        {
            string[] tempFiles = new string[ea.Length];
            for (int i = 0; i < tempFiles.Length; i++)
            {
                tempFiles[i] = options.TempFiles.AddExtension("py", true);

                using (StreamWriter sw = new StreamWriter(tempFiles[i]))
                {
                    new PythonProvider().GenerateCodeFromCompileUnit(ea[i], sw, new CodeGeneratorOptions());
                }
            }

            return FromFileWorker(options, tempFiles);
        }

        #endregion
    }
}