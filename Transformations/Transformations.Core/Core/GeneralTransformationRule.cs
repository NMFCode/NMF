using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using NMF.Transformations.Core.Properties;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class represents the base class for a transformation rule
    /// </summary>
    public abstract class GeneralTransformationRule
    {
        #region Constructor
        /// <summary>
        /// Creates a new transformation rule
        /// </summary>
        protected GeneralTransformationRule()
        {
            Dependencies = new List<ITransformationRuleDependency>();
            IsUnique = true;
        }
        #endregion

        #region Abstract Members
        
        /// <summary>
        /// Registers all the dependencies (both calling and non-calling) and additional configuration
        /// </summary>
        /// <remarks>This method is called during initialization of the entire transformation and is independent of any transformation contexts. However, this method may rely on the <see cref="Transformation"/>-property.</remarks>
        public virtual void RegisterDependencies() {}
        
        /// <summary>
        /// Gets the type signature of the input arguments of this transformation rule
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public abstract Type[] InputType { get; }

        /// <summary>
        /// Gets the type signature of the output type of this transformation
        /// </summary>
        public abstract Type OutputType { get; }

        /// <summary>
        /// Creates a new Computation instance for this transformation rule or the given input 
        /// </summary>
        /// <param name="input">The input arguments for this computation</param>
        /// <param name="context">The context for this computation</param>
        /// <returns>A computation object</returns>
        public abstract Computation CreateComputation(object[] input, IComputationContext context);


        /// <summary>
        /// Gets a value indicating whether the output for all dependencies must have been created before this rule creates the output
        /// </summary>
        public virtual bool NeedDependenciesForOutputCreation
        {
            get { return true; }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the transformation, that this rule is assigned to
        /// </summary>
        public Transformation Transformation { get; internal set; }

        /// <summary>
        /// Gets a collection of dependencies for this transformation rule
        /// </summary>
        public IList<ITransformationRuleDependency> Dependencies { get; private set; }

        /// <summary>
        /// Gets or sets the output delay level
        /// </summary>
        /// <remarks>The default delay level is 0. The delay level has an influence on the availability of the trace data during output creation.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public byte OutputDelayLevel { get; protected set; }

        /// <summary>
        /// Gets or sets the transformation delay level
        /// </summary>
        /// <remarks>The default transformation delay level is 0. The delay has an influence when computations are made</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public byte TransformationDelayLevel { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this transformation rule is unique per input and context
        /// </summary>
        public bool IsUnique { get; set; }

        #endregion

        #region Internal properties

        internal List<GeneralTransformationRule> Children { get; private set; }
        
        /// <summary>
        /// Gets the base rule, i.e. the rule the current rule can instantiate
        /// </summary>
        public GeneralTransformationRule BaseRule { get; private set; }
        internal Predicate<Computation> BaseFilter { get; private set; }
        
        #endregion

        #region RegistrationMethods

        internal ITransformationRuleDependency Depend(Predicate<Computation> filter, Func<Computation, object[]> selector, GeneralTransformationRule transformation, Action<object, object> persistor, bool executeBefore, bool needOutput)
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            if (selector == null) throw new ArgumentNullException("selector");

            var dependency = new SingleDependency()
            {
                Filter = filter,
                Selector = selector,
                Persistor = persistor,
                BaseTransformation = this,
                DependencyTransformation = transformation,
                ExecuteBefore = executeBefore,
                NeedOutput = needOutput
            };

            Dependencies.Add(dependency);

            return dependency;
        }

        internal ITransformationRuleDependency DependMany(Predicate<Computation> filter, Func<Computation, IEnumerable<object[]>> selector, GeneralTransformationRule transformation, Action<object, IEnumerable> persistor, bool executeBefore, bool needOutput)
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            if (selector == null) throw new ArgumentNullException("selector");

            var dependency = new MultipleDependency()
            {
                Filter = filter,
                Selector = selector,
                Persistor = persistor,
                BaseTransformation = this,
                DependencyTransformation = transformation,
                ExecuteBefore = executeBefore,
                NeedOutput = needOutput
            };

            Dependencies.Add(dependency);

            return dependency;
        }

        internal ITransformationRuleDependency CallForInternal(GeneralTransformationRule rule, Predicate<Computation> filter, Func<Computation, object[]> selector, Action<object, object> persistor, bool needOutput)
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return rule.Depend(filter, selector, this, persistor, false, needOutput);
        }

        internal ITransformationRuleDependency CallForEachInternal(GeneralTransformationRule rule, Predicate<Computation> filter, Func<Computation, IEnumerable<object[]>> selector, Action<object, IEnumerable> persistor, bool needOutput)
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return rule.DependMany(filter, selector, this, persistor, false, needOutput);
        }

        internal bool IsLeafTransformation
        {
            get { return Children == null; }
        }

        private void AddBase(GeneralTransformationRule rule, Predicate<Computation> filter)
        {
            if (BaseRule == null)
            {
                BaseRule = rule;
                BaseFilter = filter;
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrMarkInstantiatingForOneRuleAtMost);
            }
        }

        private void AddChild(GeneralTransformationRule rule)
        {
            if (Children == null) Children = new List<GeneralTransformationRule>();
            Children.Add(rule);
        }

        /// <summary>
        /// Determines whether the current transformation rule can instantiate the output of the given computation
        /// </summary>
        /// <param name="computation">The computation that may be instantiated by the current rule</param>
        /// <returns>True, if the computation instantiates the given computation, otherwise false</returns>
        public bool IsInstantiating(Computation computation)
        {
            if (computation != null)
            {
                var rule = computation.TransformationRule;
                return HasCompliantInput(computation)
                    && (rule.OutputType == OutputType || rule.OutputType.IsAssignableFrom(OutputType))
                    && (BaseFilter == null || BaseFilter(computation));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the given computation has a compliant input to be instantiated by the current transformation rule
        /// </summary>
        /// <param name="computation">The computation that is a candidate for instantiation</param>
        /// <returns>True, if the input arguments match the input types of the current transformation rule, otherwise false</returns>
        public bool HasCompliantInput(Computation computation)
        {
            if (computation == null) return false;
            var inputTypes = InputType;
            if (computation.InputArguments != inputTypes.Length) return false;
            for (int i = 0; i < inputTypes.Length; i++)
            {
                var instance = computation.GetInput(i);
                if (instance != null && !inputTypes[i].IsInstanceOfType(instance)) return false;
            }
            return true;
        }

        #endregion

        #region Convenience methods

        /// <summary>
        /// Requires the given transformation rule
        /// </summary>
        /// <param name="rule">The transformation rule that should be required</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        public void Require(GeneralTransformationRule rule)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (rule.InputType.IsAssignableArrayFrom(InputType))
            {
                Depend(null, c => c.CreateInputArray(), rule, null, true, false);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequiresTransNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Requires the given transformation rule
        /// </summary>
        /// <param name="rule">The transformation rule that should be required</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        public void Call(GeneralTransformationRule rule)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (rule.InputType.IsAssignableArrayFrom(InputType))
            {
                Depend(null, c => c.CreateInputArray(), rule, null, false, false);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequiresTransNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Marks the current transformation rule instantiating for the specified rule
        /// </summary>
        /// <param name="rule">The base transformation rule</param>
        public void MarkInstantiatingFor(GeneralTransformationRule rule)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (rule.InputType.IsAssignableArrayFrom(InputType) && (rule.OutputType == OutputType || rule.OutputType.IsAssignableFrom(OutputType) || OutputType.IsInterface))
            {
                MarkInstantiatingFor(rule, null);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrMarkInstantiatingForMustInherit);
            }
        }

        /// <summary>
        /// Marks the current transformation rule instantiating for the specified rule
        /// </summary>
        /// <param name="rule">The base transformation rule</param>
        /// <param name="filter">A method that filters the possible computations</param>
        /// <remarks>Note that in this version, the filter method is also responsible for checking the types!</remarks>
        public void MarkInstantiatingFor(GeneralTransformationRule rule, Predicate<Computation> filter)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            rule.AddChild(this);
            AddBase(rule, filter);
        }

        #endregion
    }
}
