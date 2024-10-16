using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes the abstract class for a rule that represents a model reference
    /// </summary>
    /// <typeparam name="TSemanticElement">the type of the semantic element</typeparam>
    /// <typeparam name="TReference">the type of the reference</typeparam>
    public abstract class AssignModelReferenceRule<TSemanticElement, TReference> : AssignReferenceRule<TSemanticElement, TReference>
        where TSemanticElement : class, IModelElement
        where TReference : class, IModelElement
    {
        /// <inheritdoc />
        protected override string GetReferenceString(TReference reference, ParseContext context)
        {
            return reference.ToIdentifierString();
        }
    }
}
