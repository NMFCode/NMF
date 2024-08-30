using System;

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

        /// <summary>
        /// The index of the function to which the source element flows
        /// </summary>
        public int FunctionIndex { get; private set; }

        /// <summary>
        /// The index of the parameter for which the source is used
        /// </summary>
        public int FunctionParameterIndex { get; private set; }

        /// <summary>
        /// Specifies that elements of a source parameter are used as argument for a function given in another parameter
        /// </summary>
        /// <param name="functionIndex">The index of the function to which the source element flows</param>
        /// <param name="functionParameterIndex">The index of the parameter for which the source is used</param>
        /// <param name="source">The index of the parameter that is the source of the dependency</param>
        public ParameterDataflowAttribute(int functionIndex, int functionParameterIndex, int source)
        {
            FunctionIndex = functionIndex;
            FunctionParameterIndex = functionParameterIndex;
            SourceIndex = source;
        }
    }
}
