using System;
using NMF.Expressions;
using NMF.Utilities;

namespace NMF.Models
{
    public interface IResolvableModel : IModelElement
    {
        IModelElement Resolve(string uriString);

        IModelElement Resolve(Uri resolveUri);
    }
}