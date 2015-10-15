using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public class ReflectiveSynchronization : Synchronization
    {
        private IDictionary<Type, GeneralTransformationRule> rules;
        private IDictionary<Type, SynchronizationRuleBase> syncRules;

        /// <summary>
        /// Creates all transformation rules of this transformation
        /// </summary>
        /// <remarks>This method is called during Initialization. The output IEnumerable-collection is saved into a list.</remarks>
        /// <returns>A collection of transformation rules</returns>
        protected sealed override IEnumerable<GeneralTransformationRule> CreateRules()
        {
            var stack = GetTransformationTypeStack();
            this.rules = Reflector.ReflectDictionary(stack, CreateDefaultTransformationRules, CreateCustomTransformationRules);
            this.syncRules = Reflector.ReflectDictionary(stack, CreateDefaultSynchronizationRules, CreateCustomSynchronizationRules);
            return rules.Values.Concat(syncRules.Values.Select(s => s.LTR)).Concat(syncRules.Values.Select(s => s.RTL));
        }

        private Stack<Type> GetTransformationTypeStack()
        {
            var typeStack = new Stack<Type>();
            var currentType = this.GetType();
            while (currentType != typeof(ReflectiveSynchronization))
            {
                typeStack.Push(currentType);
                currentType = currentType.BaseType;
            }
            return typeStack;
        }

        /// <summary>
        /// Gets the rule with the specified type (exact match)
        /// </summary>
        /// <param name="transformationRule">The type of the transformation rule</param>
        /// <returns>The transformation rule with this type or null, if there is none</returns>
        /// <remarks>This method assumes there is only one transformation rule per type</remarks>
        public override GeneralTransformationRule GetRuleForRuleType(Type transformationRule)
        {
            GeneralTransformationRule rule;
            if (rules.TryGetValue(transformationRule, out rule))
            {
                return rule;
            }
            else
            {
                return base.GetRuleForRuleType(transformationRule);
            }
        }


        /// <summary>
        /// Gets the transformation rule instance of the given rule type within the given transformation
        /// </summary>
        /// <typeparam name="TRule">The type of the desired transformation rule</typeparam>
        /// <returns>The transformation rule</returns>
        public TRule Rule<TRule>() where TRule : GeneralTransformationRule
        {
            Initialize();
            return GetRuleForRuleType(typeof(TRule)) as TRule;
        }

        /// <summary>
        /// Gets the synchronization rule instance of the given rule type within the given transformation
        /// </summary>
        /// <typeparam name="TRule">The type of the desired transformation rule</typeparam>
        /// <returns>The transformation rule</returns>
        public TRule SynchronizationRule<TRule>() where TRule : SynchronizationRuleBase
        {
            Initialize();
            SynchronizationRuleBase rule;
            if (syncRules.TryGetValue(typeof(TRule), out rule))
            {
                return rule as TRule;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all rules with the specified type (exact match)
        /// </summary>
        /// <param name="transformationRule">The type of the transformation rules</param>
        /// <returns>A collection of all rules with this type</returns>
        /// <remarks>This method assumes there is only one transformation rule per type</remarks>
        public override IEnumerable<GeneralTransformationRule> GetRulesForRuleType(Type transformationRule)
        {
            var rule = GetRuleForRuleType(transformationRule);
            if (rule != null) return Enumerable.Repeat(rule, 1);
            return base.GetRulesForRuleType(transformationRule);
        }

        /// <summary>
        /// Registers the rules of this transformation
        /// </summary>
        public override void RegisterRules()
        {
            if (!IsRulesRegistered)
            {
                base.RegisterRules();

                var patterns = Reflector.ReflectDictionary<ITransformationPattern>(GetTransformationTypeStack(), null, null);
                foreach (var item in patterns.Values)
                {
                    Patterns.Add(item);
                }
            }
        }

        /// <summary>
        /// Creates the custom transformation rules that are no public nested classes (cannot be overridden by reflected rules)
        /// </summary>
        /// <remarks>This method is called during Initialization. The output IEnumerable-collection is saved into a list.</remarks>
        /// <returns>A collection of transformation rules</returns>
        protected virtual IEnumerable<GeneralTransformationRule> CreateCustomTransformationRules()
        {
            return null;
        }

        /// <summary>
        /// Creates the default transformation rules that are no public nested classes (can be overridden by reflected rules)
        /// </summary>
        /// <remarks>This method is called during Initialization. The output IEnumerable-collection is saved into a list.</remarks>
        /// <returns>A collection of transformation rules</returns>
        protected virtual IEnumerable<GeneralTransformationRule> CreateDefaultTransformationRules()
        {
            return null;
        }

        /// <summary>
        /// Creates the custom synchronization rules that are no public nested classes (cannot be overridden by reflected rules)
        /// </summary>
        /// <remarks>This method is called during Initialization. The output IEnumerable-collection is saved into a list.</remarks>
        /// <returns>A collection of transformation rules</returns>
        protected virtual IEnumerable<SynchronizationRuleBase> CreateCustomSynchronizationRules()
        {
            return null;
        }

        /// <summary>
        /// Creates the default synchronization rules that are no public nested classes (can be overridden by reflected rules)
        /// </summary>
        /// <remarks>This method is called during Initialization. The output IEnumerable-collection is saved into a list.</remarks>
        /// <returns>A collection of transformation rules</returns>
        protected virtual IEnumerable<SynchronizationRuleBase> CreateDefaultSynchronizationRules()
        {
            return null;
        }


        /// <summary>
        /// Gets the synchronization rules contained in this synchronization
        /// </summary>
        public override IEnumerable<SynchronizationRuleBase> SynchronizationRules
        {
            get { return syncRules.Values; }
        }
    }
}
