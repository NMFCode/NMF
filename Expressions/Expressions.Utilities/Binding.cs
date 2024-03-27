using System;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Helper class to create bindings
    /// </summary>
    /// <typeparam name="T">The type of bindings</typeparam>
    public static class Binding<T>
    {
        /// <summary>
        /// Creates a new binding for the type T
        /// </summary>
        /// <typeparam name="TMember">The type of the member that should be bound</typeparam>
        /// <param name="func">The function the member should be bound to</param>
        /// <param name="member">The member that should be bound</param>
        /// <returns>A binding</returns>
        public static IBinding<T> Create<TMember>(Expression<Func<T, TMember>> func, Expression<Func<T, TMember>> member)
        {
            var setter = SetExpressionRewriter.CreateSetter(member) ?? throw new InvalidOperationException($"The expression {member.ToString()} could not be inverted.");
            return Create(func, setter.Compile());
        }

        /// <summary>
        /// Creates a new binding for the type T
        /// </summary>
        /// <typeparam name="TMember">The type of the member that should be bound</typeparam>
        /// <param name="func">The function the member should be bound to</param>
        /// <param name="setter">The setter function for the member</param>
        /// <returns>A binding</returns>
        public static IBinding<T> Create<TMember>(Expression<Func<T, TMember>> func, Action<T, TMember> setter)
        {
            return new Binding<T, TMember>(func, setter);
        }
    }

    /// <summary>
    /// Denotes a binding of type T
    /// </summary>
    /// <typeparam name="T">The type of the object that is bound</typeparam>
    public interface IBinding<T>
    {
        /// <summary>
        /// Executes the binding for the given element
        /// </summary>
        /// <param name="item">The element that should be bound</param>
        /// <returns>A disposable instance. When disposed, the binding for the provided element ends.</returns>
        IDisposable Bind( T item );
    }

    /// <summary>
    /// Denotes a binding of type T
    /// </summary>
    /// <typeparam name="T">The type of the object that is bound</typeparam>
    /// <typeparam name="TMember">The value type of the binding</typeparam>
    public class Binding<T, TMember> : IBinding<T>
    {
        private readonly ObservingFunc<T, TMember> func;
        private readonly Action<T, TMember> setter;

        /// <summary>
        /// Creates a new binding
        /// </summary>
        /// <param name="func">The function that should be bound</param>
        /// <param name="setter">The setter to which the function should be bound</param>
        public Binding(ObservingFunc<T, TMember> func, Action<T, TMember> setter)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
            this.setter = setter ?? throw new ArgumentNullException(nameof(setter));
        }

        /// <inheritdoc />
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
