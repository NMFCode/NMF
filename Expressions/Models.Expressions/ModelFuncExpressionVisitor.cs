using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NMF.Models;

namespace NMF.Expressions
{
    internal class ModelFuncExpressionVisitor : ExpressionVisitorBase
    {
        private Dictionary<string, ParameterExtraction> parameters = new Dictionary<string, ParameterExtraction>();

        public ICollection<ParameterExtraction> ExtractParameters
        {
            get
            {
                return parameters.Values;
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (typeof(IModelElement).IsAssignableFrom(node.Type) && !typeof(IModelElement).IsAssignableFrom(node.Expression.Type))
            {
                var replaceMemberId = node.ToString();
                ParameterExtraction extraction;
                if (!parameters.TryGetValue(replaceMemberId, out extraction))
                {
                    extraction = new ParameterExtraction(Expression.Parameter(node.Type, "model_par_" + parameters.Count.ToString()), node);
                    parameters.Add(replaceMemberId, extraction);
                }
                return extraction.Parameter;
            }
            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (typeof(IModelElement).IsAssignableFrom(node.Type) && node.Arguments.Any(arg => !typeof(IModelElement).IsAssignableFrom(arg.Type)))
            {
                var replaceMemberId = node.ToString();
                ParameterExtraction extraction;
                if (!parameters.TryGetValue(replaceMemberId, out extraction))
                {
                    extraction = new ParameterExtraction(Expression.Parameter(node.Type, "model_par_" + parameters.Count.ToString()), node);
                    parameters.Add(replaceMemberId, extraction);
                }
                return extraction.Parameter;
            }
            return base.VisitMethodCall(node);
        }
    }
}
