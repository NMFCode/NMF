using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NMF.Utilities;
using NMF.Transformations.Properties;
using NMF.Transformations.Core;

namespace NMF.Transformations
{
    /// <summary>
    /// This class is used to describe transformation rules that have more than two input arguments and no output
    /// </summary>
    public abstract class InPlaceTransformationRuleBase : GeneralTransformationRule
    {

        /// <summary>
        /// Gets the type signature of the output type of this transformation
        /// </summary>
        public override Type OutputType
        {
            get { return typeof(void); }
        }


        /// <summary>
        /// Marks the current transformation rule instantiating for the specified rule
        /// </summary>
        /// <param name="filter">The filter that should filter the inputs where this transformation rule is marked instantiating</param>
        /// <param name="rule">The transformation rule</param>
        public void MarkInstantiatingFor(GeneralTransformationRule rule, Predicate<object[]> filter)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (rule.InputType.IsAssignableArrayFrom(InputType) && (rule.OutputType == typeof(void)))
            {
                Require(rule);
                if (filter != null)
                {
                    MarkInstantiatingFor(rule, o => InputType.IsInstanceArrayOfType(o) && filter(o));
                }
                else
                {
                    MarkInstantiatingFor(rule, InputType.IsInstanceArrayOfType);
                }
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrMarkInstantiatingMustInherit);
            }
        }

    }
}
