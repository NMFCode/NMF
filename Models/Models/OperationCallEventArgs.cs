using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models
{
    public class OperationCallEventArgs : EventArgs
    {
        private readonly IOperation operation;
        private readonly object[] arguments;
        private readonly IModelElement target;
        private object result;

        public OperationCallEventArgs(IModelElement target, IOperation operation, params object[] arguments)
        {
            this.target = target;
            this.operation = operation;
            this.arguments = arguments;
        }

        public IOperation Operation
        {
            get
            {
                return operation;
            }
        }

        public object[] Arguments
        {
            get
            {
                return arguments;
            }
        }

        public IModelElement Target
        {
            get
            {
                return target;
            }
        }

        public object Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
            }
        }
    }
}
