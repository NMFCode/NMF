using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    public class ModelElementRule<TReference> : ElementRule<TReference>
        where TReference : IModelElement
    {
        /// <inheritdoc />
        protected override string GetReferenceString(TReference reference, ParseContext context)
        {
            return reference.ToIdentifierString();
        }
    }
}
