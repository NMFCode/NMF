using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using TaskParallel = System.Threading.Tasks.Parallel;

namespace NMF.Transformations.Parallel
{
    /// <summary>
    /// Denotes the transformation context of a parallel transformation
    /// </summary>
    public class ParallelTransformationContext : ITransformationEngineContext
    {
        private readonly ConcurrentDictionary<object[], List<ITraceEntry>> computationsMade = new ConcurrentDictionary<object[], List<ITraceEntry>>(ItemEqualityComparer<object>.Instance);
        private readonly ConcurrentDictionary<GeneralTransformationRule, List<ITraceEntry>> computationsByTransformationRule = new ConcurrentDictionary<GeneralTransformationRule, List<ITraceEntry>>();
        private readonly ConcurrentQueue<Computation> dependencyCallQueue = new ConcurrentQueue<Computation>();
        private readonly List<ConcurrentQueue<Computation>> computationOrder = new List<ConcurrentQueue<Computation>>();
        private readonly List<ConcurrentQueue<DelayedOutputCreation>> delay = new List<ConcurrentQueue<DelayedOutputCreation>>();

        private readonly List<object[]> inputs = new List<object[]>();
        private readonly List<object> outputs = new List<object>();

        /// <summary>
        /// Gets the parent transformation, that the context is based upon
        /// </summary>
        public Transformation Transformation { get; private set; }
        private byte currentTransformationDelay = 0;
        private byte currentOutputDelay = 0;
        private readonly ITransformationTrace trace;
        private bool callTransformations;

        /// <summary>
        /// Creates a new transformation context for the given transformation
        /// </summary>
        /// <param name="transformation">The transformation, a context should be generated for</param>
        public ParallelTransformationContext(Transformation transformation)
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            Transformation = transformation;
            trace = new TransformationContextTrace(this);
        }

        #region CallTransformation & Outsourced functions

        /// <summary>
        /// Calls the given transformation with the specified input
        /// </summary>
        /// <param name="input">The input for the transformation rule</param>
        /// <param name="transformationRule">The rule that should be applied</param>
        /// <param name="context">The callers context</param>
        /// <returns>The computation that handles this request</returns>
        public Computation CallTransformation(GeneralTransformationRule transformationRule, object[] input, IEnumerable context)
        {
            if (transformationRule == null) throw new ArgumentNullException(nameof(transformationRule));
            if (input == null) throw new ArgumentNullException(nameof(input));

            List<ITraceEntry> computations = CalculateComputations(input);

            var originalTransformationRule = transformationRule;
            while (transformationRule.BaseRule != null)
            {
                transformationRule = transformationRule.BaseRule;
            }

            Computation comp;
            ParallelComputationContext compCon = null;
            bool handleComputation;
            if (transformationRule.IsUnique)
            {
                lock (computations)
                {
                    comp = GetLeastSpecificComputation(ref transformationRule, computations, originalTransformationRule);

                    if (comp == null)
                    {
                        CallTransformationCore(transformationRule, input, computations, out comp, out compCon, out handleComputation);
                    }
                    else
                    {
                        handleComputation = false;
                    }
                }
            }
            else
            {
                lock (computations)
                {
                    CallTransformationCore(transformationRule, input, computations, out comp, out compCon, out handleComputation);
                }
            }
            if (handleComputation)
            {
                AddTraceEntry(comp);

                CallDependencies(comp, true);

                HandleComputation(transformationRule, input, context, computations, originalTransformationRule, comp, compCon);
            }
            return comp;
        }

        private void CallTransformationCore(GeneralTransformationRule transformationRule, object[] input, List<ITraceEntry> computations, out Computation comp, out ParallelComputationContext compCon, out bool handleComputation)
        {
            handleComputation = true;
            compCon = CreateComputationContext(transformationRule);
            comp = transformationRule.CreateComputation(input, compCon);
            computations.Add(comp);
            compCon.DelayOutput(new OutputDelay());
        }

        private static Computation GetLeastSpecificComputation(ref GeneralTransformationRule transformationRule, List<ITraceEntry> computations, GeneralTransformationRule originalTransformationRule)
        {
            var rule = transformationRule;
            Computation comp = computations.OfType<Computation>().FirstOrDefault(cpt => cpt.TransformationRule == rule);
            if (comp != null && transformationRule != originalTransformationRule)
            {
                rule = transformationRule = originalTransformationRule;
                while (computations.OfType<Computation>().FirstOrDefault(cpt => cpt.TransformationRule == rule.BaseRule) == null)
                {
                    rule = transformationRule = transformationRule.BaseRule;
                }
                comp = computations.OfType<Computation>().FirstOrDefault(cpt => cpt.TransformationRule == rule);
            }

            return comp;
        }

        private List<ITraceEntry> CalculateComputations(object[] input)
        {
            List<ITraceEntry> computations;
            if (!computationsMade.TryGetValue(input, out computations))
            {
                computations = new List<ITraceEntry>();
                if (!computationsMade.TryAdd(input, computations))
                {
                    computations = computationsMade[input];
                }
            }

            return computations;
        }

        /// <summary>
        /// Handles the computation internally, i.e. calls dependencies, creates output, manages delays, etc
        /// </summary>
        /// <param name="transformationRule">The transformation rule</param>
        /// <param name="input">The input elements for this computation</param>
        /// <param name="context">The transformation context</param>
        /// <param name="computations">The computations for the input</param>
        /// <param name="originalTransformationRule">The transformation rule of the original call</param>
        /// <param name="comp">The computation</param>
        /// <param name="compCon">The computation context</param>
        private void HandleComputation(GeneralTransformationRule transformationRule, object[] input, IEnumerable context, List<ITraceEntry> computations, GeneralTransformationRule originalTransformationRule, Computation comp, ComputationContext compCon)
        {
            // The transformation output is only generated when we are handling the base transformation rule,
            // because this is always required
            if (compCon.IsDelayed)
            {
                Stack<Computation> dependantComputes = new Stack<Computation>();
                var ruleStack = Transformation.ComputeInstantiatingTransformationRulePath(comp);
                if (transformationRule != originalTransformationRule)
                {
                    ReorderStack(originalTransformationRule, comp, ruleStack);
                }
                var delayLevel = comp.Context.MinOutputDelayLevel;

                List<Computation> computes = CalculateComputes(input, computations, comp, dependantComputes, ruleStack, ref delayLevel);

                // delay the call of dependencies
                // this prevents the issue arising from computations calling their parents that come later in the stack
                foreach (var comp2 in dependantComputes)
                {
                    CallDependencies(comp2, true);
                }

                if (delayLevel <= currentOutputDelay)
                {
                    HandleComputationCore(context, computes);
                }
                else
                {
                    //Save computations into Delay
                    Delay(delayLevel, computes, context);
                }

                if (!callTransformations)
                {
                    for (int i = computes.Count - 1; i >= 0; i--)
                    {
                        AddToComputationOrder(computes[i], currentTransformationDelay);
                    }
                }

                for (int i = computes.Count - 1; i >= 0; i--)
                {
                    dependencyCallQueue.Enqueue(computes[i]);
                }
            }
        }

        private void HandleComputationCore(IEnumerable context, List<Computation> computes)
        {
            var createRule = computes[0];

            // Generate the output
            var output = createRule.CreateOutput(context);

            for (int i = computes.Count - 1; i >= 0; i--)
            {
                computes[i].InitializeOutput(output);
            }
            if (callTransformations)
            {
                for (int i = computes.Count - 1; i >= 0; i--)
                {
                    computes[i].Transform();
                }
            }
        }

        private List<Computation> CalculateComputes(object[] input, List<ITraceEntry> computations, Computation comp, Stack<Computation> dependantComputes, Stack<GeneralTransformationRule> ruleStack, ref byte delayLevel)
        {
            var computes = new List<Computation>();
            Computation lastComp = null;

            while (ruleStack.Count > 0)
            {
                var rule = ruleStack.Pop();
                var comp2 = FindOrCreateDependentComputation(input, computations, comp, dependantComputes, rule);

                // in case comp2 is not yet handled, a delay does not yet exist and thus
                // DelayLevel < minDelayLevel
                delayLevel = Math.Max(delayLevel, Math.Max(comp2.OutputDelayLevel, comp2.Context.MinOutputDelayLevel));
                if (lastComp != null)
                {
                    lastComp.SetBaseComputation(comp2);
                }
                lastComp = comp2;
                computes.Add(comp2);
            }

            return computes;
        }

        private ParallelComputationContext CreateComputationContext(GeneralTransformationRule rule)
        {
            var compCon = new ParallelComputationContext(this);
            compCon.DelayOutputAtLeast(rule.OutputDelayLevel);
            compCon.DelayTransformationAtLeast(rule.TransformationDelayLevel);
            return compCon;
        }

        private Computation FindOrCreateDependentComputation(object[] input, List<ITraceEntry> computations, Computation comp, Stack<Computation> dependantComputes, GeneralTransformationRule rule)
        {
            lock (computations)
            {
                var comp2 = computations.Where(cmp => cmp.TransformationRule == rule).OfType<Computation>().FirstOrDefault();
                if (comp2 == null)
                {
                    comp2 = rule.CreateComputation(input, comp.Context);
                    computations.Add(comp2);
                    AddTraceEntry(comp2);
                    dependantComputes.Push(comp2);
                }
                return comp2;
            }
        }

        private static void ReorderStack(GeneralTransformationRule originalTransformationRule, Computation comp, Stack<GeneralTransformationRule> ruleStack)
        {
            var testTransformationRule = originalTransformationRule;
            var missingStack = new Stack<GeneralTransformationRule>();
            while (!ruleStack.Contains(testTransformationRule))
            {
                missingStack.Push(testTransformationRule);
                testTransformationRule = testTransformationRule.BaseRule;
            }
            while (ruleStack.Peek() != testTransformationRule)
            {
                ruleStack.Pop();
                if (ruleStack.Count == 0) throw new InvalidOperationException("The rule stack from the transformation rule did not contain the base rule of the computation");
            }
            while (missingStack.Count > 0)
            {
                testTransformationRule = missingStack.Pop();
                ruleStack.Push(testTransformationRule);
            }
            while (!testTransformationRule.IsLeafTransformation)
            {
                var found = false;
                foreach (var next in testTransformationRule.Children)
                {
                    if (next.IsInstantiating(comp))
                    {
                        testTransformationRule = next;
                        ruleStack.Push(next);
                        found = true;
                        break;
                    }
                }
                if (!found) break;
            }
        }

        /// <summary>
        /// Creates a trace entry for the given computation object
        /// </summary>
        /// <remarks>Override for custom trace entries. A null-check for the argument is not required.</remarks>
        /// <param name="computation">The computation that needs to be added to the trace</param>
        protected virtual void AddTraceEntry(Computation computation)
        {
            List<ITraceEntry> comps;
            var rule = computation.TransformationRule;
            if (!computationsByTransformationRule.TryGetValue(rule, out comps))
            {
                comps = new List<ITraceEntry>();
                if (!computationsByTransformationRule.TryAdd(rule, comps))
                {
                    comps = computationsByTransformationRule[rule];
                }
            }
            lock (comps)
            {
                comps.Add(computation);
            }
        }

        private readonly object _computationLevelLockObject = new object();
        private void AddToComputationOrder(Computation c, byte level)
        {
            level = Math.Max(level, c.Context.MinTransformDelayLevel);
            ConcurrentQueue<Computation> list;
            if (computationOrder.Count > level)
            {
                list = computationOrder[level];
                if (list == null)
                {
                    lock (_computationLevelLockObject)
                    {
                        list = CreateLevel(level);
                    }
                }
            }
            else
            {
                lock (_computationLevelLockObject)
                {
                    if (computationOrder.Count <= level)
                    {
                        while (computationOrder.Count < level)
                        {
                            computationOrder.Add(null);
                        }
                        list = new ConcurrentQueue<Computation>();
                        computationOrder.Add(list);
                    }
                    else
                    {
                        list = CreateLevel(level);
                    }
                }
            }
            list.Enqueue(c);
        }

        private ConcurrentQueue<Computation> CreateLevel(byte level)
        {
            var list = computationOrder[level];
            if (list == null)
            {
                list = new ConcurrentQueue<Computation>();
                computationOrder[level] = list;
            }
            return list;
        }

        private readonly object _delayLockObject = new object();

        private void Delay(byte delayLevel, List<Computation> computes, IEnumerable context)
        {
            delayLevel = Math.Max(delayLevel, currentOutputDelay);
            ConcurrentQueue<DelayedOutputCreation> list;
            if (delay.Count > delayLevel)
            {
                list = delay[delayLevel];
                if (list == null)
                {
                    lock (_delayLockObject)
                    {
                        list = CreateDelayLevel(delayLevel);
                    }
                }
            }
            else
            {
                lock (_delayLockObject)
                {
                    if (delay.Count <= delayLevel)
                    {
                        while (delay.Count < delayLevel)
                        {
                            delay.Add(null);
                        }
                        list = new ConcurrentQueue<DelayedOutputCreation>();
                        delay.Add(list);
                    }
                    else
                    {
                        list = CreateDelayLevel(delayLevel);
                    }
                }
            }
            var output = new DelayedOutputCreation(computes, context);
            list.Enqueue(output);
        }

        private ConcurrentQueue<DelayedOutputCreation> CreateDelayLevel(byte level)
        {
            var list = delay[level];
            if (list == null)
            {
                list = new ConcurrentQueue<DelayedOutputCreation>();
                delay[level] = list;
            }
            return list;
        }

        private static void CallDependencies(Computation c, bool executeBefore)
        {
            TaskParallel.ForEach(c.TransformationRule.Dependencies, requirement =>
            {
                if (requirement.ExecuteBefore == executeBefore)
                {
                    requirement.HandleDependency(c);
                }
            });
        }

        /// <summary>
        /// Executes all computations registered,but not already handled
        /// </summary>
        public void ExecutePendingComputations()
        {
            callTransformations = true;
            if (computationOrder != null)
            {
                while (currentTransformationDelay < computationOrder.Count)
                {
                    //first increment currentTransformationDelay to avoid conflicts
                    //because items are put in the collection while there is an Enumerator
                    //on the collection
                    var collection = computationOrder[currentTransformationDelay];
                    if (collection != null)
                    {
                        ExecuteLevel(collection);
                    }
                    currentTransformationDelay++;
                    CreateDelayedOutputs();
                }
            }
        }

        /// <inheritdoc />
        protected void ExecuteLevel(ConcurrentQueue<Computation> computationsOfLevel)
        {
            using (var countEvent = new CountdownEvent(1))
            {
                while (true)
                {
                    Computation item;
                    if (computationsOfLevel.TryDequeue(out item))
                    {
                        if (item.Context is ParallelComputationContext cc)
                        {
                            countEvent.AddCount();
                            cc.RunTransform(() =>
                            {
                                item.Transform();
                                countEvent.Signal();
                            });
                        }
                        else
                        {
                            countEvent.Signal();
                            countEvent.Wait();
                            item.Transform();
                            countEvent.Reset();
                        }
                        OnComputationCompleted(new ComputationEventArgs(item));
                    }
                    else
                    {
                        break;
                    }
                }
                countEvent.Signal();
                countEvent.Wait();
            }
        }

        /// <summary>
        /// Creates the outputs of all delayed computations
        /// </summary>
        private void CreateDelayedOutputs()
        {
            if (delay != null)
            {
                while (currentOutputDelay < delay.Count)
                {
                    var delayLevel = delay[currentOutputDelay];
                    if (delayLevel != null)
                    {
                        CreateDelayedOutputs(delayLevel);
                    }
                    currentOutputDelay++;
                }
                delay.Clear();
                currentOutputDelay = 0;
            }
        }

        private void CreateDelayedOutputs(ConcurrentQueue<DelayedOutputCreation> delayLevel)
        {
            while (delayLevel.TryDequeue(out DelayedOutputCreation item))
            {
                item.CreateDelayedOutput(callTransformations);
            }
        }

        #endregion

        #region Trace Helpers

        /// <summary>
        /// Gets all rules that apply the given signature
        /// </summary>
        /// <param name="input">The input argument type list</param>
        /// <param name="output">The output type</param>
        /// <returns>A collection with all the rules that have the given signature</returns>
        public IEnumerable<GeneralTransformationRule> GetRules(Type[] input, Type output)
        {
            return Transformation.GetRulesForTypeSignature(input, output);
        }

        /// <summary>
        /// Gets any rules that apply the given signature
        /// </summary>
        /// <param name="input">The input argument type list</param>
        /// <param name="output">The output type</param>
        /// <returns>A random rule that has the given signature</returns>
        public GeneralTransformationRule GetRule(Type[] input, Type output)
        {
            return Transformation.GetRuleForTypeSignature(input, output);
        }

        #endregion

        #region Bag & Data

        private readonly dynamic bag = new ExpandoObject();
        private readonly IDictionary<object, object> data = new Dictionary<object, object>();

        /// <summary>
        /// Gets a Bag, where dynamic data can be added
        /// </summary>
        /// <remarks>The value of this property is an ExpandoObject, so that the bag can be easily extended with new properties</remarks>
        public dynamic Bag
        {
            get { return bag; }
        }


        /// <summary>
        /// Gets a data dictionary, where data set during the transformation can be added
        /// </summary>
        public IDictionary<object, object> Data
        {
            get { return data; }
        }

        #endregion

        #region Trace

        /// <summary>
        /// Represents the trace class for TransformationContext
        /// </summary>
        protected class TransformationContextTrace : AbstractTrace, ITransformationTrace
        {
            private readonly ConcurrentDictionary<object[], List<ITraceEntry>> computationsMade;
            private readonly ConcurrentDictionary<GeneralTransformationRule, List<ITraceEntry>> computationsByTransformationRule;
            private readonly ParallelTransformationContext context;

            /// <summary>
            /// Creates a new trace class for the given TraceContext
            /// </summary>
            /// <param name="context">The trace class for which the trace should be generated</param>
            public TransformationContextTrace(ParallelTransformationContext context)
            {
                if (context == null) throw new ArgumentNullException("context");
                this.computationsMade = context.computationsMade;
                this.computationsByTransformationRule = context.computationsByTransformationRule;
                this.context = context;
            }

            /// <summary>
            /// Gets a collection of the underlying computations
            /// </summary>
            public override IEnumerable<ITraceEntry> Computations
            {
                get { return context.Computations
                    .Except(revoked)
                    .Concat(published); }
            }

            #region Trace accelerated by "computationsMade"

            /// <summary>
            /// Traces the computations based upon the specified input
            /// </summary>
            /// <returns>The computations with the given inputs</returns>
            /// <param name="input">The input arguments</param>
            public override IEnumerable<ITraceEntry> Trace(object[] input)
            {
                List<ITraceEntry> comps;
                if (computationsMade.TryGetValue(input, out comps))
                {
                    return comps;
                }
                return Enumerable.Empty<ITraceEntry>();
            }

            /// <summary>
            /// Traces the computations of the specified inputs with the specified transformation rules
            /// </summary>
            /// <param name="rule">The transformation rules that transformed the specified inputs</param>
            /// <param name="inputs">A collection of input arguments</param>
            /// <returns>A collection of computations</returns>
            public override IEnumerable<ITraceEntry> TraceManyIn(GeneralTransformationRule rule, IEnumerable<object[]> inputs)
            {
                if (rule == null) throw new ArgumentNullException("rule");
                if (inputs == null) return Enumerable.Empty<ITraceEntry>();
                List<ITraceEntry> result = new List<ITraceEntry>();
                foreach (var input in inputs)
                {
                    List<ITraceEntry> comps;
                    if (computationsMade.TryGetValue(input, out comps))
                    {
                        result.AddRange(comps.Where(c => c.TransformationRule == rule));
                    }
                }
                return result;
            }

            /// <summary>
            /// Traces the computation based upon the specified input with the specified transformation rule
            /// </summary>
            /// <param name="rule">The transformation rule the object was transformed with</param>
            /// <returns>The computation or null, if there was none</returns>
            /// <param name="input">The input arguments</param>
            public override IEnumerable<ITraceEntry> TraceIn(GeneralTransformationRule rule, params object[] input)
            {
                if (rule == null) throw new ArgumentNullException("rule");
                List<ITraceEntry> comps;
                if (computationsMade.TryGetValue(input, out comps))
                {
                    return comps.Where(c => c.TransformationRule == rule);
                }
                return Enumerable.Empty<ITraceEntry>();
            }

            /// <summary>
            /// Traces the computations of the specified inputs that match the given type signature
            /// </summary>
            /// <param name="inputs">A collection of input arguments</param>
            /// <param name="inputTypes">The input types</param>
            /// <param name="outputType">The output types</param>
            /// <returns>A collection of computations</returns>
            public override IEnumerable<ITraceEntry> TraceMany(Type[] inputTypes, Type outputType, IEnumerable<object[]> inputs)
            {
                if (inputs == null) return Enumerable.Empty<ITraceEntry>();
                List<ITraceEntry> result = new List<ITraceEntry>();
                foreach (var input in inputs)
                {
                    List<ITraceEntry> comps;
                    if (computationsMade.TryGetValue(input, out comps))
                    {
                        result.AddRange(comps);
                    }
                }
                return result;
            }

            #endregion

            #region Trace accelerated by "computationsByTransformationRule"

            /// <summary>
            /// Traces all computations with any inputs that math the given filters with the specified transformation rule
            /// </summary>
            /// <param name="rule">The transformation rule</param>
            /// <returns>A collection with all computations made under these circumstances</returns>
            public override IEnumerable<ITraceEntry> TraceAllIn(GeneralTransformationRule rule)
            {
                if (rule == null) throw new ArgumentNullException("rule");
                List<ITraceEntry> comps;
                if (computationsByTransformationRule.TryGetValue(rule, out comps))
                {
                    return comps.AsReadOnly();
                }
                else
                {
                    return Enumerable.Empty<Computation>();
                }
            }

            #endregion

            private readonly HashSet<ITraceEntry> revoked = new HashSet<ITraceEntry>();
            private readonly HashSet<ITraceEntry> published = new HashSet<ITraceEntry>();

            /// <summary>
            /// Revokes the given computation and deletes it from the trace
            /// </summary>
            /// <param name="traceEntry">The computation that is to be revoked</param>
            public override void RevokeEntry(ITraceEntry traceEntry)
            {
                if (traceEntry == null) throw new ArgumentNullException("traceEntry");

                if (published.Contains(traceEntry))
                {
                    published.Remove(traceEntry);
                }
                else
                {
                    revoked.Add(traceEntry);
                }

                List<ITraceEntry> comps;
                if (computationsMade.TryGetValue(traceEntry.CreateInputArray(), out comps))
                {
                    comps.Remove(traceEntry);
                }
                if (computationsByTransformationRule.TryGetValue(traceEntry.TransformationRule, out comps))
                {
                    comps.Remove(traceEntry);
                }
            }

            /// <summary>
            /// Publishes the given computation to the trace
            /// </summary>
            /// <param name="traceEntry">The computation that should be added to the trace</param>
            public override void PublishEntry(ITraceEntry traceEntry)
            {
                if (traceEntry == null) throw new ArgumentNullException("traceEntry");

                if (revoked.Contains(traceEntry))
                {
                    revoked.Remove(traceEntry);
                }
                else
                {
                    var success = published.Add(traceEntry);
                    if (!success) return;
                }

                var box = traceEntry.CreateInputArray();
                List<ITraceEntry> comps;
                if (!computationsMade.TryGetValue(box, out comps))
                {
                    comps = new List<ITraceEntry>();
                    if (!computationsMade.TryAdd(box, comps))
                    {
                        comps = computationsMade[box];
                    }
                }
                if (!comps.Contains(traceEntry))
                {
                    comps.Add(traceEntry);
                    var rule = traceEntry.TransformationRule;
                    if (!computationsByTransformationRule.TryGetValue(rule, out comps))
                    {
                        comps = new List<ITraceEntry>();
                        if (!computationsByTransformationRule.TryAdd(rule, comps))
                        {
                            comps = computationsByTransformationRule[rule];
                        }
                    }
                    comps.Add(traceEntry);
                }
                else
                {
                    published.Remove(traceEntry);
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether calls to this transformation context implementation are thread-safe
        /// </summary>
        bool ITransformationContext.IsThreadSafe
        {
            get { return true; }
        }

        /// <summary>
        /// Calls the transformation context to finish any things yet undone
        /// </summary>
        public void ExecutePending()
        {
            CallPendingDependencies();
            CreateDelayedOutputs();
            ExecutePendingComputations();
        }

        private void CallPendingDependencies()
        {
            Computation item;
            while (dependencyCallQueue.TryDequeue(out item))
            {
                CallDependencies(item, false);
            }
        }

        /// <summary>
        /// Gets all computations (for custom trace purposes)
        /// </summary>
        public IEnumerable<Computation> Computations
        {
            get { return computationOrder.SelectMany(o => o); }
        }

        /// <summary>
        /// Gets the object responsible for trace operations for this transformation context
        /// </summary>
        public ITransformationTrace Trace
        {
            get { return trace; }
        }

        /// <summary>
        /// Gets the input of the transformation context
        /// </summary>
        /// <remarks>If the transformation has multiple inputs, this returns the first input</remarks>
        public object[] Input
        {
            get { return inputs.FirstOrDefault(); }
            set
            {
                inputs.Clear();
                inputs.Add(value);
            }
        }

        /// <summary>
        /// Gets a collection of inputs
        /// </summary>
        public IList<object[]> Inputs
        {
            get { return inputs; }
        }

        /// <summary>
        /// Gets the output of the transformation context
        /// </summary>
        /// <remarks>If the transformation has multiple outputs, this property returns the first output</remarks>
        public object Output
        {
            get { return outputs.FirstOrDefault(); }
        }

        /// <summary>
        /// Gets a collection of outputs
        /// </summary>
        public IList<object> Outputs
        {
            get { return outputs; }
        }

        /// <summary>
        /// Fires the ComputationCompleted event with the given event data
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnComputationCompleted(ComputationEventArgs e)
        {
            ComputationCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Gets fired when a computation completes
        /// </summary>
        public event EventHandler<ComputationEventArgs> ComputationCompleted;
    }
}