using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Linq
{
    internal class IncrementalPatternEngine
    {
        private static object _DataKey = new object();

        private bool isRunning;

        public static IncrementalPatternEngine GetForContext(ITransformationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (context.Data == null) throw new ArgumentException("Data container not set", "context");
            object engine;
            if (context.Data.TryGetValue(_DataKey, out engine))
            {
                return engine as IncrementalPatternEngine;
            }
            else
            {
                IncrementalPatternEngine _engine = new IncrementalPatternEngine(context);
                context.Data.Add(_DataKey, _engine);
                return _engine;
            }
        }

        public IList<IncrementalPatternContext> Patterns { get; private set; }

        private IncrementalPatternEngine(ITransformationContext context)
        {
            Patterns = new List<IncrementalPatternContext>();

            context.ComputationCompleted += context_ComputationCompleted;
        }

        void context_ComputationCompleted(object sender, ComputationEventArgs e)
        {
            for (int i = 0; i < Patterns.Count; i++)
            {
                if (Patterns[i].PushComputation())
                {
                    return;
                }
            }
        }

        public void Run()
        {
            if (!isRunning)
            {
                for (int i = 0; i < Patterns.Count; i++)
                {
                    if (Patterns[i].PushComputation())
                    {
                        isRunning = true;
                        return;
                    }
                }
            }
        }
    }
}
