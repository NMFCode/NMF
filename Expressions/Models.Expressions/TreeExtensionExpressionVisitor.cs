using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal class TreeExtensionExpressionVisitor : ModelExpressionVisitorBase<object>
    {
        public struct ParameterInfo
        {
            public ParameterInfo(List<string> properties) : this()
            {
                Anchors = new List<Type>();
                Properties = properties;
            }

            public List<Type> Anchors { get; private set; }

            public List<string> Properties { get; private set; }
        }

        protected override bool ResetForCrossReference(Expression targetExpression, PropertyInfo property, bool isCrossReference, out Expression returnValue)
        {
            returnValue = null;
            return false;
        }

        protected override bool ResetForLambdaExpression(object state, MethodCallExpression methodCall, LambdaExpression lambda, out Expression returnValue)
        {
            returnValue = null;
            return false;
        }

        protected override object SaveState()
        {
            return null;
        }


        public Dictionary<ParameterExpression, ParameterInfo> CollectParameterInfos()
        {
            var dict = new Dictionary<ParameterExpression, ParameterInfo>();
            foreach (var access in PropertyAccesses.OfType<PropertyAccess>())
            {
                if (!IsModelProperty(access.Property))
                {
                    continue;
                }
                ParameterInfo parameterInfo;
                if (!dict.TryGetValue(access.Parameter, out parameterInfo))
                {
                    parameterInfo = new ParameterInfo(new List<string>());
                    dict.Add(access.Parameter, parameterInfo);
                }
                var current = access;
                while (current != null)
                {
                    if (!parameterInfo.Properties.Contains(current.PropertyName))
                    {
                        parameterInfo.Properties.Add(current.PropertyName);
                    }
                    if (current.IsCrossReference)
                    {
                        var anchor = current.Anchor;
                        if (anchor != null)
                        {
                            if (current.IsAnchorEffective(anchor))
                            {
                                if (!parameterInfo.Anchors.Contains(anchor))
                                {
                                    parameterInfo.Anchors.Add(anchor);
                                }
                            }
                        }
                        else
                        {
                            parameterInfo.Anchors.Add(null);
                        }
                    }
                    current = current.Base as PropertyAccess;
                }
            }
            foreach (var info in dict.Values)
            {
                if (info.Anchors.Contains(null))
                {
                    info.Anchors.Clear();
                    info.Anchors.Add(typeof(IModelRepository));
                }
            }
            return dict;
        }
    }
}
