using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NMF.Utilities;
using System.Runtime.CompilerServices;
using NMF.Collections.Generic;

namespace NMF.Expressions
{
    internal class PromotionExpressionVisitor : ModelExpressionVisitorBase<Dictionary<string, ParameterExtraction>>
    {
        private Dictionary<string, ParameterExtraction> parameterextractions = new Dictionary<string, ParameterExtraction>();
        
        public ICollection<ParameterExtraction> ExtractParameters
        {
            get
            {
                return parameterextractions.Values;
            }
        }

        protected override bool ResetForCrossReference(Expression targetExpression, PropertyInfo property, bool isCrossReference, out Expression returnValue)
        {
            var extract = ExtractParameter(targetExpression);
            propertyAccesses.Clear();
            propertyAccesses.Add(new PropertyAccess(new ParameterReference(extract), property, isCrossReference));
            returnValue = Expression.MakeMemberAccess(extract, property);
            return true;
        }

        private ParameterExpression ExtractParameter(Expression node)
        {
            ParameterExtraction par;
            var id = node.ToString();
            if (!parameterextractions.TryGetValue(id, out par))
            {
                var parameter = Expression.Parameter(node.Type, "promotion_arg_" + parameterextractions.Count.ToString());
                par = new ParameterExtraction(parameter, node);
                parameterextractions.Add(id, par);
            }
            return par.Parameter;
        }

        private Expression ExtractLambda(MethodCallExpression node, Dictionary<string, ParameterExtraction> extractionsSaved)
        {
            parameterextractions = extractionsSaved;
            var extraction = ExtractParameter(node);
            propertyAccesses.Add(new ParameterReference(extraction));
            return extraction;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ParameterExtraction extraction;
            if (parameterextractions.TryGetValue(node.ToString(), out extraction))
            {
                return extraction.Parameter;
            }
            return base.VisitMember(node);
        }

        private bool CheckConflictingExtractions(Dictionary<string, ParameterExtraction> extractionsSaved, LambdaExpression lambda)
        {
            if (parameterextractions.Count > extractionsSaved.Count)
            {
                foreach (var extraction in parameterextractions)
                {
                    if (!extractionsSaved.ContainsKey(extraction.Key))
                    {
                        var parametersUsed = new ParameterCollector();
                        parametersUsed.Visit(extraction.Value.Value);
                        foreach (var parameter in lambda.Parameters)
                        {
                            if (parametersUsed.Parameters.Contains(parameter))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public Dictionary<ParameterExpression, ParameterInfo> CollectParameterInfos()
        {
            var dict = new Dictionary<ParameterExpression, ParameterInfo>();
            foreach (var access in propertyAccesses.OfType<PropertyAccess>())
            {
                if (!IsModelProperty(access.Property))
                {
                    continue;
                }
                ParameterInfo parameterInfo;
                if (!dict.TryGetValue(access.Parameter, out parameterInfo))
                {
                    parameterInfo = new ParameterInfo(new List<string>(), false);
                    dict.Add(access.Parameter, parameterInfo);
                }
                var current = access;
                var count = 0;
                while (current != null)
                {
                    if (!parameterInfo.Properties.Contains(current.PropertyName))
                    {
                        parameterInfo.Properties.Add(current.PropertyName);
                    }
                    count++;
                    current = current.Base as PropertyAccess;
                }
                if (!parameterInfo.NeedsContainment && count > 1)
                {
                    parameterInfo.NeedsContainment = true;
                    dict[access.Parameter] = parameterInfo;
                }
            }
            return dict;
        }

        protected override bool ResetForLambdaExpression(Dictionary<string, ParameterExtraction> state, MethodCallExpression methodCall, LambdaExpression lambda, out Expression returnValue)
        {
            if (!CheckConflictingExtractions(state, lambda))
            {
                returnValue = ExtractLambda(methodCall, state);
                return true;
            }
            else
            {
                returnValue = null;
                return false;
            }
        }

        protected override Dictionary<string, ParameterExtraction> SaveState()
        {
            return new Dictionary<string, ParameterExtraction>(parameterextractions);
        }
    }
}
