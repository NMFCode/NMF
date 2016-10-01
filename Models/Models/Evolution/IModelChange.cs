using NMF.Models.Repository;
using NMF.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public interface IModelChange
    {
        Uri AbsoluteUri { get; }

        void Apply(IModelRepository repository);

        void Undo(IModelRepository repository);
    }
}
