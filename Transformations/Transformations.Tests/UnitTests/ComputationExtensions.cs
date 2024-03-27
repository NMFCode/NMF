using NMF.Transformations.Core;

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
