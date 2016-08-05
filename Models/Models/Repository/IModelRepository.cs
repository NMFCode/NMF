using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public interface IModelRepository
    {
        IModelElement Resolve(Uri uri);

        ModelCollection Models { get; }

        event EventHandler<BubbledChangeEventArgs> BubbledChange;
    }
}
