using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    public abstract class DescriptorBase
    {
        internal GraphicalLanguage Language;

        protected T D<T>() where T : DescriptorBase
        {
            return Language?.Descriptor<T>();
        }

        protected internal abstract void DefineLayout();
    }
}
