using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    public class ModelNotifySystem : InstructionLevelNotifySystem
    {
        public static new readonly ModelNotifySystem Instance = new ModelNotifySystem();

        internal override ObservableExpressionBinder CreateBinder()
        {
            return new ModelExpressionVisitor();
        }
    }
}
