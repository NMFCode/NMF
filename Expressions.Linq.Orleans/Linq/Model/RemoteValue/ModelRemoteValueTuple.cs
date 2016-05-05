using System;
using System.Collections.Generic;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    [Serializable]
    public class ModelRemoteValueTuple<TTuple> : IModelRemoteValue<TTuple> where TTuple : IModelElementTuple
    {
        public ModelRemoteValueTuple(TTuple tuple)
        {
            _tuple = tuple;
            _items = new List<IModelRemoteValue>();
            foreach (var item in tuple)
            {
                _items.Add(ModelRemoteValueFactory.CreateModelChangeValue(item));
            }
            _tupleIdentifier = tuple.Identifier;
        }

        private List<IModelRemoteValue> _items;
        private Guid _tupleIdentifier;

        [NonSerialized]
        private TTuple _tuple;

        object IModelRemoteValue.Retrieve(ILocalResolveContext resolveContext)
        {
            return Retrieve(resolveContext);
        }

        public object ReferenceComparable => _tuple;

        public TTuple Retrieve(ILocalResolveContext resolveContext)
        {
            if (resolveContext.ObjectLookup.ContainsKey(_tupleIdentifier))
            {
                return (TTuple) resolveContext.ObjectLookup[_tupleIdentifier];
            }

            // Unknown, so resolve each element individually and register.
            var localElements = new List<IModelElement>();
            foreach (var item in _items)
            {
                localElements.Add((IModelElement) item.Retrieve(resolveContext));
            }

            var resultTuple =  ModelElementTupleFactory.CreateTuple<TTuple>(localElements);
            resultTuple.Identifier = _tupleIdentifier;

            resolveContext.ObjectLookup.Add(_tupleIdentifier, resultTuple);

            return resultTuple;
        }

        public object GlobalIdentifier => _tupleIdentifier;
    }
}