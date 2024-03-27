using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    internal class RepositoryChangeApplyParametersVisitor : ExpressionVisitorBase
    {
        private readonly IDictionary<string, object> parameterMappings;

        public List<IChangeInfo> Recorders { get; private set; }

        public RepositoryChangeApplyParametersVisitor(IDictionary<string, object> parameterMappings)
        {
            this.parameterMappings = parameterMappings;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            object argument;
            if (parameterMappings.TryGetValue(node.Name, out argument))
            {
                if (node.Type.IsInstanceOfType(argument))
                {
                    return Expression.Constant(argument);
                }
                else if (argument is Expression expression)
                {
                    return expression;
                }
                else
                {
                    var notifyValueType = typeof(INotifyValue<>).MakeGenericType(node.Type);
                    if (notifyValueType.IsInstanceOfType(argument))
                    {
                        if (Recorders == null)
                        {
                            Recorders = new List<IChangeInfo>();
                        }
                        var valueProperty = notifyValueType.GetProperty("Value");
                        var recorderType = typeof(ChangeRecorder<>).MakeGenericType(node.Type);
                        var recorder = recorderType.GetConstructors()[0].Invoke(new object[] { argument });

                        Recorders.Add((IChangeInfo)recorder);

                        return Expression.MakeMemberAccess(Expression.Constant(argument, notifyValueType), valueProperty);
                    }
                    throw new InvalidOperationException(string.Format("The provided value {0} for parameter {1} is not valid.", argument, node.Type));
                }
            }
            else
            {
                return node;
            }
        }

        private sealed class ChangeRecorder<T> : IChangeInfo
        {
            public INotifyValue<T> Value { get; private set; }

            public ChangeRecorder(INotifyValue<T> value)
            {
                Value = value;
                Value.ValueChanged += Value_ValueChanged;
            }

            public event EventHandler ChangeCaptured;

            private void Value_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                ChangeCaptured?.Invoke(this, e);
            }
        }
    }

    internal interface IChangeInfo
    {
        event EventHandler ChangeCaptured;
    }
}
