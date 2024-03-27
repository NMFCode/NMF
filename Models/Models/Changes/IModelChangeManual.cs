using System.Collections.Generic;

namespace NMF.Models.Changes
{
    public partial interface IModelChange
    {
        void Apply();

        IEnumerable<IModelChange> Invert();
    }
}
