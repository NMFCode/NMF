using NMF.AnyText.Rules;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText.Model
{
    internal abstract class FeatureSynthesisRequirement : SynthesisRequirement
    {
        private readonly IEnumerable<SynthesisRequirement> _inner;

        protected FeatureSynthesisRequirement(IEnumerable<SynthesisRequirement> inner)
        {
            _inner = inner;
        }

        public override bool Matches(object semanticObject)
        {
            if (semanticObject is ParseObject parseObject)
            {
                var assigned = Peek(parseObject);
                if (assigned == null)
                {
                    return false;
                }
                return _inner.All(x => x.Matches(assigned));
            }
            return _inner.All(x => x.Matches(semanticObject));
        }

        protected abstract object Peek(ParseObject parseObject);

        internal override void FreeReservations(ParseObject semanticObject)
        {
            semanticObject.FreeReservation(Feature);
        }
    }
}
