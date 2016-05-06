using System;
using System.Collections.Generic;
using NMF.Models;
using Orleans.Collections;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class LocalModelReceiveContext : LocalReceiveContext
    {
        public LocalModelReceiveContext(IResolvableModel lookupModel) : base()
        {
            LookupModel = lookupModel;
        }

        public IResolvableModel LookupModel { get; }
    }
}