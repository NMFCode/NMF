using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    public static class Binding<T>
    {

        public static Binding<T, TMember> Create<TMember>(Expression<Func<T, TMember>> func, Expression<Func<T, TMember>> member)
        {
            var setter = SetExpressionRewriter.CreateSetter(member) ?? throw new InvalidOperationException($"The expression {member.ToString()} could not be inverted.");
            return Create(func, setter.Compile());
        }

        public static Binding<T, TMember> Create<TMember>(Expression<Func<T, TMember>> func, Action<T, TMember> setter)
        {
            return new Binding<T, TMember>(func, setter);
        }
    }


    public class Binding<T, TMember>
    {
        private ObservingFunc<T, TMember> func;
        private Action<T, TMember> setter;


        public Binding(ObservingFunc<T, TMember> func, Action<T, TMember> setter)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
            this.setter = setter ?? throw new ArgumentNullException(nameof(setter));
        }

        public IDisposable Bind(T item)
        {
            var notifiable = func.Observe(item);
            notifiable.Successors.SetDummy();
            setter(item, notifiable.Value);
            notifiable.ValueChanged += (o, e) => setter(item, (TMember)e.NewValue);
            return notifiable;
        }
    }
}
