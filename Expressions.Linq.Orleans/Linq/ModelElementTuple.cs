using System;
using System.Collections;
using System.Collections.Generic;
using NMF.Models;
using NMF.Utilities;

namespace NMF.Expressions.Linq.Orleans
{
    [Serializable]
    public class ModelElementTuple<T1, T2> : Tuple<T1, T2>, IModelElementTuple where T1 : IModelElement where T2 : IModelElement 
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`2" /> class.</summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        public ModelElementTuple(T1 item1, T2 item2) : base(item1, item2)
        {
            Identifier = Guid.NewGuid();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<IModelElement> GetEnumerator()
        {
            return new List<IModelElement> {Item1, Item2}.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Guid Identifier { get; set; }
    }

    public static class ModelElementTupleFactory
    {
        public static T CreateTuple<T>(IList<IModelElement> items)
        {
            var genericTupleType = typeof(T).GenericTypeArguments;
            if(genericTupleType.Length != items.Count)
                throw new ArgumentException($"Cannot create tuple of size {genericTupleType.Length} with {items.Count} items.");
            
            // Match types and create parameters
            object[] invocationArgs = new object[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                var castedArg = (dynamic) items[i];
                invocationArgs[i] = castedArg;
            }

            return (T) Activator.CreateInstance(typeof(T), invocationArgs);
        }
    }
    
    // Marker interface
    public interface IModelElementTuple : IEnumerable<IModelElement>
    {
        Guid Identifier { get; set; }

    }
}