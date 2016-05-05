using System;
using System.Collections.Generic;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class LocalResolveContext : ILocalResolveContext
    {
        public LocalResolveContext(IResolvableModel lookupModel)
        {
            LookupModel = lookupModel;
            ObjectLookup = new Dictionary<object, object>();
        }

        public IResolvableModel LookupModel { get; }
        public Dictionary<object, object> ObjectLookup { get; }
    }
}