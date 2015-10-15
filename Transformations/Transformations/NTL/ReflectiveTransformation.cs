using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using NMF.Transformations.Properties;
using NMF.Transformations.Core;

namespace NMF.Transformations
{
    /// <summary>
    /// This is a base class of a transformation that just creates an instance of each nested class, if it is not abstract and is a transformation rule
    /// </summary>
    public abstract class ReflectiveTransformation : Transformation
    {
        private IDictionary<Type, GeneralTransformationRule> rules;

        /// <summary>
        /// Creates all transformation rules of this transformation
        /// </summary>
        /// <remarks>This method is called during Initialization. The output IEnumerable-collection is saved into a list.</remarks>
        /// <returns>A collection of transformation rules</returns>
        protected sealed override IEnumerable<GeneralTransformationRule> CreateRules()
        {
            this.rules = Reflector.ReflectDictionary(GetTransformationTypeStack(), CreateDefaultRules, CreateCustomRules);
            return rules.Values;
        }

        private Stack<Type> GetTransformationTypeStack()
        {
            var typeStack = new Stack<Type>();
            var currentType = this.GetType();
            while (currentType != typeof(ReflectiveTransformation))
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
                return null;
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
        /// Gets all rules with the specified type (exact match)
        /// </summary>
        /// <param name="transformationRule">The type of the transformation rules</param>
        /// <returns>A collection of all rules with this type</returns>
        /// <remarks>This method assumes there is only one transformation rule per type</remarks>
        public override IEnumerable<GeneralTransformationRule> GetRulesForRuleType(Type transformationRule)
        {
            var rule = GetRuleForRuleType(transformationRule);
            if (rule != null) yield return rule;
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
        /// Creates the transformation rules that are no public nested classes (cannot be overridenn by reflected rules)
        /// </summary>
        /// <returns>A collection of transformation rules</returns>
        protected virtual IEnumerable<GeneralTransformationRule> CreateCustomRules()
        {
            return null;
        }

        /// <summary>
        /// Creates the default transformation rules (can be overridden by reflected rules)
        /// </summary>
        /// <returns>A collection of transformation rules</returns>
        protected virtual IEnumerable<GeneralTransformationRule> CreateDefaultRules()
        {
            return null;
        }
    }

    /// <summary>
    /// A helper class that reflects thr transformation rules that reside as nested classes within a given type
    /// </summary>
    public static class Reflector
    {
        public static IDictionary<Type, T> ReflectDictionary<T>(Stack<Type> typeStack, Func<IEnumerable<T>> createDefaults, Func<IEnumerable<T>> createCustoms)
            where T : class
        {
            if (typeStack == null) throw new ArgumentNullException("typeStack");
            var rules = new Dictionary<Type, T>();
            AddItems<T>(createDefaults, rules);
            foreach (var type in typeStack)
            {
                Reflector.ReflectInType<T>(type, (ruleType, rule) =>
                {
                    var overridden = ruleType.GetCustomAttributes(typeof(OverrideRuleAttribute), false);
                    rules.Add(ruleType, rule);
                    if (overridden != null && overridden.Length > 0)
                    {
                        var overrd = overridden[0] as OverrideRuleAttribute;
                        if (!rules.ContainsKey(ruleType.BaseType))
                        {
                            Debug.WriteLine("The rule {0} could not be marked as override. No suitable transformation rule found to override.", ruleType.Name);
                        }
                        else
                        {
                            rules[ruleType.BaseType] = rule;
                        }
                    }
                });
            }
            AddItems<T>(createCustoms, rules);
            return rules;
        }

        private static void AddItems<T>(Func<IEnumerable<T>> createItems, Dictionary<Type, T> rules) where T : class
        {
            if (createItems != null)
            {
                var customs = createItems();
                if (customs != null)
                {
                    foreach (var customRule in customs)
                    {
                        if (customRule == null) continue;
                        var ruleType = customRule.GetType();
                        if (rules.ContainsKey(ruleType)) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ErrReflectiveTransformationCustomRulesRuleTypeAlreadyInUse, ruleType.Name));
                        rules.Add(ruleType, customRule);
                    }
                }
            }
        }


        /// <summary>
        /// Reflects a type and instantiates all transformation rules contained in this type as nested classes
        /// </summary>
        /// <param name="transformationType">The type that should be reflected</param>
        /// <param name="persistor">A method that should be executed to save the reflected transformation rule</param>
        /// <remarks>This method reflects the nested classes contained in the given type. It does not look in base types!</remarks>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the transformationType parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the persistor parameter is passed a null instance</exception>
        
        public static void ReflectInType<T>(Type transformationType, Action<Type, T> persistor)
            where T : class
        {
            if (transformationType == null) throw new ArgumentNullException("transformationType");
            if (persistor == null) throw new ArgumentNullException("persistor");

            Type target = typeof(T);
            foreach (var item in transformationType.GetNestedTypes())
            {
                if (item.IsSubclassOf(target) && !item.IsAbstract && !item.IsGenericTypeDefinition)
                    persistor(item, (Activator.CreateInstance(item) as T));
            }
        }
    }
}
