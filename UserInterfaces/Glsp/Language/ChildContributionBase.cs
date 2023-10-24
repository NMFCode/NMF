using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal abstract class ChildContributionBase<T>
    {
        public abstract void Contribute(T input, GElement element);
    }
}
