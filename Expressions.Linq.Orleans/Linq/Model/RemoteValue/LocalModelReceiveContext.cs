using System;
using System.Collections.Generic;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class LocalModelReceiveContext : LocalReceiveContext
    {
        private IResolvableModel _lookupModel;
        private ObjectReferenceLookup<Uri, IModelElement> _lookupCache;

        public LocalModelReceiveContext(IResolvableModel lookupModel) : base()
        {
            _lookupModel = lookupModel;
            _lookupCache = new ObjectReferenceLookup<Uri, IModelElement>();
        }


        public IModelElement RetrieveWithCache(Uri lookupUri)
        {
            //IModelElement element = null;
            //if(lookupUri != null)
            //    element = _lookupCache.GetValue(lookupUri);
            //if (element == null || !element.RelativeUri.Equals(lookupUri))
            //{
                var element = _lookupModel.Resolve(lookupUri);
            //    if(lookupUri != null)
            //        _lookupCache.OverrideValue(lookupUri, element);
            //}

            return element;
        }

        public void BuildCache<T>(IEnumerable<T> elements)
        {
            //foreach (var element in elements)
            //{
            //    var modelElement = element as IModelElement;
            //    if(modelElement != null) 
            //        _lookupCache.Add(modelElement.RelativeUri, modelElement);
            //}
        }
    }
}