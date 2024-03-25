using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Specifies a dataflow between parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ParameterDataflowAttribute : Attribute
    {
        /// <summary>
        /// A constant to specify the index for the target of a method
        /// </summary>
        public const int TargetObjectIndex = -1;

        /// <summary>
        /// The index of the parameter that is the source of the dependency
        /// </summary>
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
