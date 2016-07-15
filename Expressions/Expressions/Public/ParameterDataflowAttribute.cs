using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ParameterDataflowAttribute : Attribute
    {
        public const int TargetObjectIndex = -1;

        public int SourceIndex { get; private set; }

        public int FunctionIndex { get; private set; }

        public int FunctionParameterIndex { get; private set; }

        public ParameterDataflowAttribute(int functionIndex, int functionParameterIndex, int source)
        {
            FunctionIndex = functionIndex;
            FunctionParameterIndex = functionParameterIndex;
            SourceIndex = source;
        }
    }
}
