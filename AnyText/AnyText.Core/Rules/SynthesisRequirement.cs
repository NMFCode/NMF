using NMF.AnyText.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes the abstract base class for a synthesis requirement
    /// </summary>
    public abstract class SynthesisRequirement
    {
        /// <summary>
        /// Determines whether the requirement is satisfied
        /// </summary>
        /// <param name="semanticObject">the semantic object that should be matched</param>
        /// <returns>true, if the requirement is satisfied, otherwise false</returns>
        public abstract bool Matches(object semanticObject);

        /// <summary>
        /// Gets the feature that is required for synthesis
        /// </summary>
        public virtual string Feature => null;

        /// <summary>
        /// True, if the synthesis requirement can consume many values, otherwise false
        /// </summary>
        public virtual bool CanConsumeMany => false;

        /// <summary>
        /// Places reservations for the given semantic object
        /// </summary>
        /// <param name="semanticObject">the semantic object</param>
        internal virtual void PlaceReservations(ParseObject semanticObject) { }

        /// <summary>
        /// Frees reservations for the semantic object
        /// </summary>
        /// <param name="semanticObject">the semantic object</param>
        internal virtual void FreeReservations(ParseObject semanticObject) { }
    }
}
