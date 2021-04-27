using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableParameter<T> : NotifyExpression<T>
    {
        public override string ToString()
        {
            return "<" + Name + ">";
        }

        public ObservableParameter(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        protected override T GetValue()
        {
            throw new InvalidOperationException(string.Format("No value for the argument {0} was provided.", Name));
        }
        
        public override bool IsParameterFree
        {
            get { return false; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get { return Enumerable.Empty<INotifiable>(); }
        }

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            object value;
            if (parameters != null && parameters.TryGetValue(Name, out value))
            {
                if(value is INotifyExpression<T> notifyExpression)
                {
                    return notifyExpression;
                }
                else
                {
                    if(value is INotifyValue<T> notifyValue)
                    {
                        if(value is INotifyReversableValue<T> notifyReversableValue)
                        {
                            return new ObservableProxyReversableExpression<T>( notifyReversableValue );
                        }
                        else
                        {
                            return new ObservableProxyExpression<T>( notifyValue );
                        }
                    }
                    else
                    {
                        return new ObservableConstant<T>( (T)value );
                    }
                }
            }
            else
            {
                return this;
            }
        }
    }
}
