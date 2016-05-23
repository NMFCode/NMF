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
        private HashSet<ParameterExpression> removeParameters = new HashSet<ParameterExpression>();

        public ICollection<ParameterExtraction> ExtractParameters
        {
            get
            {
                return parameters.Values;
            }
        }

        public ICollection<ParameterExpression> RemoveParameters
        {
            get
            {
                return removeParameters;
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var memberStack = new Stack<MemberExpression>();
            var currentMem = node;
            ParameterExpression parameter = null;
            while (parameter == null)
            {
                memberStack.Push(currentMem);
                switch (currentMem.Expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        currentMem = currentMem.Expression as MemberExpression;
                        break;
                    case ExpressionType.Parameter:
                        parameter = currentMem.Expression as ParameterExpression;
                        break;
                    default:
                        return base.VisitMember(node);
                }
            }
            while (memberStack.Count > 0 && !(typeof(IModelElement).IsAssignableFrom(memberStack.Peek().Type)))
            {
                memberStack.Pop();
            }
            if (memberStack.Count > 0)
            {
                removeParameters.Add(parameter);
                var replaceMember = memberStack.Pop();
                var replaceMemberId = replaceMember.ToString();
                ParameterExtraction extraction;
                if (!parameters.TryGetValue(replaceMemberId, out extraction))
                {
                    extraction = new ParameterExtraction(Expression.Parameter(replaceMember.Type, "model_par_" + parameters.Count.ToString()), replaceMember);
                    parameters.Add(replaceMemberId, extraction);
                }
                Expression returnExp = extraction.Parameter;
                while (memberStack.Count > 0)
                {
                    returnExp = memberStack.Pop().Update(returnExp);
                }
                return returnExp;
            }
            else
            {
                return node;
            }
        }
    }
}
