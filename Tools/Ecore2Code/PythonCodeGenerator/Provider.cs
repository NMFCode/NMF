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
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

namespace PythonCodeGenerator.CodeDom {
    public class PythonProvider : CodeDomProvider
    {
        List<string> references = new List<string>();

        [Obsolete]
        public override ICodeCompiler CreateCompiler()
        {
            //return new PythonGenerator();
            throw new Exception("Not Supported");
        }

        [Obsolete]
        public override ICodeGenerator CreateGenerator()
        {
            return new PythonGenerator();
        }

        public void AddReference(string assemblyName)
        {
            references.Add(assemblyName);
        }

        public override string FileExtension
        {
            get
            {
                return "py";
            }
        }

        public override void GenerateCodeFromCompileUnit(CodeCompileUnit compileUnit, TextWriter writer, CodeGeneratorOptions options)
        {
            base.GenerateCodeFromCompileUnit(compileUnit, writer, options);
        }

        public void MergeCodeFromCompileUnit(CodeCompileUnit compileUnit)
        {
            new PythonGenerator().InternalGenerateCompileUnit(compileUnit);
        }

    }

}
