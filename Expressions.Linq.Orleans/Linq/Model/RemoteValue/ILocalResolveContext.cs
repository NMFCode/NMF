using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface ILocalResolveContext
    {
        IResolvableModel LookupModel { get; }

        Dictionary<object, object> ObjectLookup { get; }
    }
}