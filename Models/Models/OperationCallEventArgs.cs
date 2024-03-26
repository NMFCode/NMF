using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Denotes event args that an operation is or was called
    /// </summary>
    public class OperationCallEventArgs : EventArgs
    {
        private readonly IOperation operation;
        private readonly object[] arguments;
        private readonly IModelElement target;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="target">The target model element</param>
        /// <param name="operation">The operation that was called</param>
        /// <param name="arguments">The arguments</param>
        public OperationCallEventArgs(IModelElement target, IOperation operation, params object[] arguments)
        {
            this.target = target;
            this.operation = operation;
            this.arguments = arguments;
        }

        /// <summary>
        /// The operation
        /// </summary>
        public IOperation Operation
        {
            get
            {
                return operation;
            }
        }

        /// <summary>
        /// The arguments
        /// </summary>
        public object[] Arguments
        {
            get
            {
                return arguments;
            }
        }

        /// <summary>
        /// The target
        /// </summary>
        public IModelElement Target
        {
            get
            {
                return target;
            }
        }

        /// <summary>
        /// Gets or sets the result of the call
        /// </summary>
        public object Result { get; set; }
    }
}
