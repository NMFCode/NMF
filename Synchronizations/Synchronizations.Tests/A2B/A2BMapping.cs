using A2BHelperWithoutContextNamespace;
using NMF.Synchronizations;
using NMF.Expressions.Linq;
using NMF.Models;

namespace Synchronizations.Tests.A2B
{
    internal class A2BMapping : ReflectiveSynchronization
    {
        public class Model2ModelMainRule : SynchronizationRule<InputModelContainer, OutputModelContainer>
        {
            public override void DeclareSynchronization()
            {
                SynchronizeManyLeftToRightOnly(
                    ( @in, context ) => context.MapLefts( SyncRule<RuleA>(), @in.IN.Descendants().OfType<A2BHelperWithoutContextNamespace.TypeA.IA>() ),
                    ( @out, _ ) => new OutputModelCollection<A2BHelperWithoutContextNamespace.TypeB.IA>( @out.OUT.RootElements.OfType<IModelElement, A2BHelperWithoutContextNamespace.TypeB.IA>() ) );

                SynchronizeManyLeftToRightOnly(
                    ( @in, context ) => context.MapLefts( SyncRule<RuleB>(), @in.IN.Descendants().OfType<A2BHelperWithoutContextNamespace.TypeA.IB>() ),
                    ( @out, _ ) => new OutputModelCollection<A2BHelperWithoutContextNamespace.TypeB.IB>( @out.OUT.RootElements.OfType<IModelElement, A2BHelperWithoutContextNamespace.TypeB.IB>() ) );
            }
        }

        public class RuleA : SynchronizationRule<A2BHelperWithoutContextNamespace.TypeA.IA, A2BHelperWithoutContextNamespace.TypeB.IA>
        {
            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(
                    s => s.NameA + HelperExtensionMethods.NameExtension(),
                    t => t.Name );

                SynchronizeManyLeftToRightOnly( SyncRule<RuleB>(),
                    s => s.Elms,
                    t => t.Elms );

                SynchronizeLeftToRightOnly(
                    s => HelperExtensionMethods.TestNameOfAElement( "ABC", s ),
                    t => t.IsNameABC );
            }
        }


        public class RuleB : SynchronizationRule<A2BHelperWithoutContextNamespace.TypeA.IB, A2BHelperWithoutContextNamespace.TypeB.IB>
        {
            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(
                    s => s.NameB + HelperExtensionMethods.NameExtension(),
                    t => t.Name );

                SynchronizeLeftToRightOnly(
                    (s, context) => context.MapLeft( SyncRule<RuleA>(), HelperExtensionMethods.FirstAElement()),
                    (t, _) => t.FirstAElement.FirstOrDefault() );

                SynchronizeLeftToRightOnly( SyncRule<RuleA>(),
                    s => HelperExtensionMethods.AElementWithName( "ABC" ),
                    t => t.AElementWithName.FirstOrDefault(),
                    null );

                SynchronizeManyLeftToRightOnly( SyncRule<RuleA>(),
                    s => s.Elms,
                    t => t.Elms );
            }
        }
    }
}
