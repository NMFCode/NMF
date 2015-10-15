using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using NMF.Utilities;
using NMF.Transformations.Properties;
using NMF.Transformations.Core;

namespace NMF.Transformations
{
    /// <summary>
    /// This class is used to describe transformation rules that have more than two input arguments
    /// </summary>
    /// <typeparam name="T">The type of the transformation rule output</typeparam>
    public abstract class TransformationRuleBase<T> : GeneralTransformationRule
        where T : class
    {

        /// <summary>
        /// Gets the type signature of the output type of this transformation
        /// </summary>
        public override Type OutputType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Marks the current transformation rule instantiating for the specified rule
        /// </summary>
        /// <param name="filter">The filter that should filter the inputs where this transformation rule is marked instantiating</param>
        /// <param name="rule">The transformation rule</param>
        public void MarkInstantiatingFor(GeneralTransformationRule rule, Predicate<object[]> filter)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (rule.InputType.IsAssignableArrayFrom(InputType) && rule.OutputType.IsAssignableFrom(OutputType))
            {
                if (filter != null)
                {
                    MarkInstantiatingFor(rule, (Computation c) => HasCompliantInput(c) && filter(c.CreateInputArray()));
                }
                else
                {
                    MarkInstantiatingFor(rule);
                }
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrMarkInstantiatingMustInherit);
            }
        }

        /// <summary>
        /// Gets the rule with the specified type (exact match)
        /// </summary>
        /// <param name="type">The type of the transformation rule</param>
        /// <returns>The transformation rule with this type or null, if there is none</returns>
        /// <remarks>This method assumes there is only one transformation rule per type</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public GeneralTransformationRule GetRule(Type type)
        {
            return Transformation.GetRuleForRuleType(type);
        }
    }
}
