using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Transformations.Tests.UnitTests
{
    public static class ComputationExtensions
    {
        internal static void DelayOutput(this Computation computation, OutputDelay outputDelay)
        {
            var cc = computation.Context as ComputationContext;
            cc.DelayOutput(outputDelay);
        }
    }
}
