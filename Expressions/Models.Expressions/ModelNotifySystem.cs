using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an incrementalization system implementing parameter promotion
    /// </summary>
    public class ModelNotifySystem : InstructionLevelNotifySystem
    {
        /// <summary>
        /// The singleton instance
        /// </summary>
        public static new readonly ModelNotifySystem Instance = new ModelNotifySystem();

        internal override ObservableExpressionBinder CreateBinder()
        {
            return new ModelExpressionVisitor();
        }
    }
}
