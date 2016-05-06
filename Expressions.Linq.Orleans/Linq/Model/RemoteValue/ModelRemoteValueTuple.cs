using System;
using System.Collections.Generic;
using NMF.Models;
using Orleans.Collections;

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

        private List<IObjectRemoteValue> _items;

        [NonSerialized]
        private TTuple _tuple;

        protected override TTuple CreateLocalObject(ILocalReceiveContext resolveContext, ReceiveAction receiveAction)
        {
            var localElements = new List<IModelElement>();
            foreach (var item in _items)
            {
                localElements.Add((IModelElement)item.Retrieve(resolveContext, receiveAction));
            }

            var resultTuple = ModelElementTupleFactory.CreateTuple<TTuple>(localElements);
            resultTuple.Identifier = GlobalIdentifier;

            return resultTuple;
        }

        public override object ReferenceComparable => _tuple;

        public override Guid GlobalIdentifier { get; }
    }
}