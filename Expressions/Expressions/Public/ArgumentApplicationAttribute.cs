using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ArgumentApplicationAttribute : Attribute
    {
        public int ArgumentParameterIndex { get; set; }

        public bool ArgumentIsFunction { get; set; }

        public int FunctionParameterIndex { get; set; }

        public int FunctionIndexParameter { get; set; }

        public ArgumentApplicationAttribute(int argumentIndex, bool isArgumentFunction, int functionIndex, int functionParameterIndex)
        {
            ArgumentParameterIndex = argumentIndex;
            ArgumentIsFunction = isArgumentFunction;
            FunctionParameterIndex = functionIndex;
            FunctionIndexParameter = functionParameterIndex;
        }
    }
}
