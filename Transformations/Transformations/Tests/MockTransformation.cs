using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Tests
{
    /// <summary>
    /// Mocks a transformation
    /// </summary>
    /// <remarks>The mocking effect comes from the fact that the mock transformation does not initialize all of its transformation rules. Instead, the transformation rules must be initialized manually by calling the RegisterDependencies method</remarks>
    public class MockTransformation : Transformation
    {
        private GeneralTransformationRule[] rules;

        /// <summary>
        /// Creates a new mock transformation with the given transformation rules
        /// </summary>
        /// <param name="rules">A collection of transformation rules</param>
        public MockTransformation(IEnumerable<GeneralTransformationRule> rules) : this(rules.ToArray()) { }

        /// <summary>
        /// Creates a new mock transformation with the given transformation rules
        /// </summary>
        /// <param name="rules">A collection of transformation rules</param>
        public MockTransformation(params GeneralTransformationRule[] rules)
        {
            if (rules == null) throw new ArgumentNullException("rules");

            this.rules = rules;

            for (int i = 0; i < rules.Length; i++)
        {
                rules[i].Transformation = this;
            }
            }

        /// <summary>
        /// This method is overridden for internal purpose and is not supposed to be called directly
        /// </summary>
        /// <returns>The collection of transformation rules</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override IEnumerable<GeneralTransformationRule> CreateRules()
        {
            return rules;
        }

        /// <summary>
        /// This method is overridden for internal purpose and is not supposed to be called directly
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed override void RegisterRules()
        {
            base.CreateRulesInternal();
            IsRulesRegistered = true;
        }
    }
}
