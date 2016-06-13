using System;
using System.Collections.Generic;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Model
{
    [Serializable]
    public class ModelRemoteValueTuple<TTuple> : ObjectRemoteValueBase<TTuple> where TTuple : IModelElementTuple
    {
        public ModelRemoteValueTuple(TTuple tuple, ILocalSendContext sendContext)
        {
            _tuple = tuple;
            _items = new List<IObjectRemoteValue>();
            foreach (var item in tuple)
            {
                _items.Add(ModelRemoteValueFactory.CreateModelRemoteValue(item, sendContext));
            }
            GlobalIdentifier = tuple.Identifier;
        }

        private readonly List<IObjectRemoteValue> _items;

        [NonSerialized]
        private readonly TTuple _tuple;

        protected override TTuple CreateLocalObject(ILocalReceiveContext receiveContext, LocalContextAction localContextAction)
        {
            var localElements = new List<IModelElement>();
            foreach (var item in _items)
            {
                localElements.Add((IModelElement)item.Retrieve(receiveContext, localContextAction));
            }

            var resultTuple = CreateTuple<TTuple>(localElements);
            resultTuple.Identifier = GlobalIdentifier;

            return resultTuple;
        }

        private static T CreateTuple<T>(IList<IModelElement> items) where T : IModelElementTuple
        {
            var genericTupleType = typeof(T).GenericTypeArguments;
            if (genericTupleType.Length != items.Count)
                throw new ArgumentException($"Cannot create tuple of size {genericTupleType.Length} with {items.Count} items.");

            // Match types and create parameters
            object[] invocationArgs = new object[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                var castedArg = (dynamic)items[i];
                invocationArgs[i] = castedArg;
            }

            return (T)Activator.CreateInstance(typeof(T), invocationArgs);
        }

        public override object ReferenceComparable => _tuple;

        public override Guid GlobalIdentifier { get; }
    }
}