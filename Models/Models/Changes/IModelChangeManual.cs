using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Changes
{
    public partial interface IModelChange
    {
        void Apply();

        IEnumerable<IModelChange> Invert();
    }
}
