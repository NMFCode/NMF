using NMF.Transformations.Core.Properties;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{

    /// <summary>
    /// This is the base class for transformations
    /// </summary>
    public abstract class Transformation
    {

        private bool isInitialized;
        private bool isRulesCreated;
        private bool isRulesRegistered;

        private List<ITransformationPattern> patterns = new List<ITransformationPattern>();

        /// <summary>
        /// Creates the transformation rules for this transformation
        /// </summary>
        /// <returns>A collection of transformation rules</returns>
        protected abstract IEnumerable<GeneralTransformationRule> CreateRules();

        /// <summary>
        /// Gets a collection of pattern objects used within the transformation
        /// </summary>
        public ICollection<ITransformationPattern> Patterns
        {
            get
            {
                return patterns;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the transformation has been initialized yet
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
            protected set
            {
                if (value)
                {
                    if (!isRulesCreated || !isRulesRegistered) throw new InvalidOperationException(Resources.ErrTransformationSetIsInitializedNoRulesRegistered);
                }
                isInitialized = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the transformation has registered its rules requirements
        /// </summary>
        public bool IsRulesRegistered
        {
            get
            {
                return isRulesRegistered;
            }
            protected set
            {
                if (value && !isRulesCreated) throw new InvalidOperationException(Resources.ErrTransformationSetIsRulesInitializedNoRulesCreated);
                isRulesRegistered = value;
            }
        }
        
        /// <summary>
        /// Creates the rules of this transformation
        /// </summary>
        protected void CreateRulesInternal()
        {
            if (!isRulesCreated)
            {
                var rules = CreateRules();

                if (rules == null) throw new InvalidOperationException(Resources.ErrTransformationCreateRulesNull);

                Rules = rules as IList<GeneralTransformationRule>;
                if (Rules == null)
                {
                    Rules = rules.ToList();
                }

                isRulesCreated = true;
            }
        }

        /// <summary>
        /// Creates a new transformation context
        /// </summary>
        /// <returns></returns>
        public virtual ITransformationContext CreateContext()
        {
            return new TransformationContext(this);
        }

        /// <summary>
        /// Registers the rules of this transformation
        /// </summary>
        public virtual void RegisterRules()
        {
            if (!isRulesRegistered)
            {
                CreateRulesInternal();

                foreach (var item in Rules)
                {
                    item.Transformation = this;
                    item.RegisterDependencies();
                }
                isRulesRegistered = true;
            }
        }

        /// <summary>
        /// Initializes the transformation
        /// </summary>
        public virtual void Initialize()
        {
            if (!isInitialized)
            {
                RegisterRules();

                //Tuning...
                
                IsInitialized = true;
            }
        }

        /// <summary>
        /// Gets the maximum output delay level
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual byte MaxOutputDelay
        {
            get
            {
                if (Rules.IsNullOrEmpty()) Initialize();
                return Rules.Max(r => r.OutputDelayLevel);
            }
        }

        /// <summary>
        /// Gets the maximum transformation delay level
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual byte MaxTransformationDelay
        {
            get
            {
                if (Rules.IsNullOrEmpty()) Initialize();
                return Rules.Max(r => r.TransformationDelayLevel);
            }
        }

        /// <summary>
        /// Computes the path of transformation rules for a given input
        /// </summary>
        /// <param name="computation">The computation for which to compute the instantiating rule path</param>
        /// <returns>A stack of transformation rules that are involved with the output creation. The top element of the stack should be able to instantiate the output (i.e. must not be abstract)</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual Stack<GeneralTransformationRule> ComputeInstantiatingTransformationRulePath(Computation computation)
        {
            if (computation == null) throw new ArgumentNullException("computation");

            var inputTypes = computation.TransformationRule.InputType;
            var output = computation.TransformationRule.OutputType;
            Stack<GeneralTransformationRule> stack = new Stack<GeneralTransformationRule>();

            var current = computation.TransformationRule;
            stack.Push(current);
            while (current != null && !current.IsLeafTransformation)
            {
                current = current.Children.Where(rule => rule.IsInstantiating(computation)).FirstOrDefault();
                if (current != null)
                {
                    stack.Push(current);
                }
            }

            return stack;
        }

        /// <summary>
        /// Gets the rule with the specified type (exact match)
        /// </summary>
        /// <param name="transformationRule">The type of the transformation rule</param>
        /// <returns>The transformation rule with this type or null, if there is none</returns>
        /// <remarks>This method assumes there is only one transformation rule per type</remarks>
        public virtual GeneralTransformationRule GetRuleForRuleType(Type transformationRule)
        {
            return Rules.FirstOrDefault(r => r.GetType() == transformationRule);
        }

        /// <summary>
        /// Gets all rules with the specified type (exact match)
        /// </summary>
        /// <param name="transformationRule">The type of the transformation rules</param>
        /// <returns>A collection of all rules with this type</returns>
        /// <remarks>This method assumes there is only one transformation rule per type</remarks>
        public virtual IEnumerable<GeneralTransformationRule> GetRulesForRuleType(Type transformationRule)
        {
            return Rules.Where(r => r.GetType() == transformationRule);
        }

        /// <summary>
        /// Gets all rules that apply the given signature
        /// </summary>
        /// <param name="inputTypes">The input argument type list</param>
        /// <param name="outputType">The output type</param>
        /// <returns>A collection with all the rules that have the given signature</returns>
        public virtual IEnumerable<GeneralTransformationRule> GetRulesForTypeSignature(Type[] inputTypes, Type outputType)
        {
            if (inputTypes == null) throw new ArgumentNullException("inputTypes");
            if (outputType == null)
            {
                outputType = typeof(void);
            }
            return Rules.Where(rule => inputTypes.IsAssignableArrayFrom(rule.InputType) && (rule.OutputType == outputType || outputType.IsAssignableFrom(rule.OutputType)));
        }

        /// <summary>
        /// Gets all rules that apply the given signature exactly
        /// </summary>
        /// <param name="input">The input argument type list</param>
        /// <param name="output">The output type</param>
        /// <returns>A collection with all the rules that have the given signature</returns>
        public virtual IEnumerable<GeneralTransformationRule> GetRulesExact(Type[] input, Type output)
        {
            if (input == null) throw new ArgumentNullException("input");
            return Rules.Where(rule => input.ArrayEquals(rule.InputType) && rule.OutputType == output);
        }

        /// <summary>
        /// Gets any rules that apply the given signature
        /// </summary>
        /// <param name="inputTypes">The input argument type list</param>
        /// <param name="outputType">The output type</param>
        /// <returns>A random rule that has the given signature</returns>
        public virtual GeneralTransformationRule GetRuleForTypeSignature(Type[] inputTypes, Type outputType)
        {
            if (inputTypes == null) throw new ArgumentNullException("inputTypes");
            if (outputType == null)
            {
                outputType = typeof(void);
            }
            return Rules.FirstOrDefault(rule => inputTypes.IsAssignableArrayFrom(rule.InputType) && (rule.OutputType == outputType || outputType.IsAssignableFrom(rule.OutputType)));
        }

        /// <summary>
        /// Gets the applicable rules for the given input type signature
        /// </summary>
        /// <param name="input">The signature of the input types</param>
        /// <returns>A collection of rules that are applicable for this signature</returns>
        public virtual IEnumerable<GeneralTransformationRule> GetRulesForInputTypes(params Type[] input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return Rules.Where(rule => input.IsAssignableArrayFrom(rule.InputType));
        }

        /// <summary>
        /// Gets a collection of all rules of this transformation
        /// </summary>
        public IList<GeneralTransformationRule> Rules { get; private set; }

        /// <summary>
        /// Gets all rules that apply the given signature
        /// </summary>
        /// <param name="input">The input argument type list</param>
        /// <param name="output">The output type</param>
        /// <param name="exact">A value indicating whether the signatures must match exactly</param>
        /// <returns>A collection with all the rules that have the given signature</returns>
        public IEnumerable<GeneralTransformationRule> GetRulesForTypeSignature(Type[] input, Type output, bool exact)
        {
            if (!exact)
            {
                return GetRulesForTypeSignature(input, output);
            }
            else
            {
                return GetRulesExact(input, output);
            }
        }
    }
}
