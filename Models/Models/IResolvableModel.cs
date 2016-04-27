using System;
using NMF.Models;

namespace NMF.Models
{
    public interface IResolvableModel
    {
        IModelElement Resolve(string uriString);

        IModelElement Resolve(Uri resolveUri);
    }
}