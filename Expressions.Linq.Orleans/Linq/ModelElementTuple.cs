using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NMF.Models;
using NMF.Utilities;

namespace NMF.Expressions.Linq.Orleans
{
    [Serializable]
    public class ModelElementTuple<T1, T2> : ModelElementTupleBase where T1 : IModelElement where T2 : IModelElement
    {
        public T1 Item1 => (T1) ElementStore[0];
        public T2 Item2 => (T2) ElementStore[1];

        /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`2" /> class.</summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        public ModelElementTuple(T1 item1, T2 item2) : base(item1, item2)
        {
        }
    }

    [Serializable]
    public class ModelElementTuple<T1, T2, T3> : ModelElementTupleBase where T1 : IModelElement where T2 : IModelElement where T3 : IModelElement
    {
        public T1 Item1 => (T1)ElementStore[0];
        public T2 Item2 => (T2)ElementStore[1];
        public T3 Item3 => (T3) ElementStore[2];

        /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`2" /> class.</summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        /// <param name="item3">The value of the tuple's third component.</param>
        public ModelElementTuple(T1 item1, T2 item2, T3 item3) : base(item1, item2, item3)
        {
        }
    }

    [Serializable]
    public class ModelElementTuple<T1, T2, T3, T4> : ModelElementTupleBase where T1 : IModelElement where T2 : IModelElement where T3 : IModelElement where T4 : IModelElement
    {
        public T1 Item1 => (T1)ElementStore[0];
        public T2 Item2 => (T2)ElementStore[1];
        public T3 Item3 => (T3)ElementStore[2];
        public T4 Item4 => (T4)ElementStore[3];

        /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`2" /> class.</summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        /// <param name="item3">The value of the tuple's third component.</param>
        /// <param name="item4">The value of the tuple's fourth component.</param>
        public ModelElementTuple(T1 item1, T2 item2, T3 item3, T4 item4) : base(item1, item2, item3, item4)
        {
        }
    }

    [Serializable]
    public class ModelElementTuple<T1, T2, T3, T4, T5> : ModelElementTupleBase where T1 : IModelElement where T2 : IModelElement where T3 : IModelElement where T4 : IModelElement where T5 : IModelElement
    {
        public T1 Item1 => (T1)ElementStore[0];
        public T2 Item2 => (T2)ElementStore[1];
        public T3 Item3 => (T3)ElementStore[2];
        public T4 Item4 => (T4)ElementStore[3];
        public T5 Item5 => (T5)ElementStore[4];

        /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`2" /> class.</summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        /// <param name="item3">The value of the tuple's third component.</param>
        /// <param name="item4">The value of the tuple's fourth component.</param>
        /// <param name="item5">The value of the tuple's fifth component.</param>
        public ModelElementTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : base(item1, item2, item3, item4, item5)
        {
        }
    }

    [Serializable]
    public abstract class ModelElementTupleBase : IModelElementTuple
    {
        protected ModelElementTupleBase(params IModelElement[] modelElements)
        {
            Identifier = Guid.NewGuid();
            ElementStore = modelElements;
        }

        protected IModelElement[] ElementStore;

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<IModelElement> GetEnumerator()
        {
            return ((IEnumerable<IModelElement>) ElementStore).GetEnumerator();
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
        public static T CreateTuple<T>(IList<IModelElement> items) where T : IModelElementTuple
        {
            var genericTupleType = typeof(T).GenericTypeArguments;
            if (genericTupleType.Length != items.Count)
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