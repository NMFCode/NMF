using NMF.Glsp.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal class GCompartmentSkeleton<T> : GElementSkeleton<T>
    {
        public GCompartmentSkeleton(ElementDescriptor<T> elementDescriptor) : base(elementDescriptor)
        {
        }

        private string CalculateSuffix()
        {
            var sep = Type.LastIndexOf(':');
            if (sep == -1)
            {
                return Type;
            }
            return Type.Substring(sep + 1);
        }

        public override string ElementTypeId => base.ElementTypeId + ":" + CalculateSuffix();
    }
}
