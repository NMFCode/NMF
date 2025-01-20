using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NMF.Models.Repository;
using A2BHelperWithoutContextNamespace;
using NMF.Synchronizations;
using NMF.Transformations;
using NMF.Models;
using System.Linq;

[assembly: ModelMetadata("platform:/resource/ListMetamodelRefactoring/Metamodel/TypeA.ecore", "Synchronizations.Tests.A2B.TypeA.nmf")]
[assembly: ModelMetadata("platform:/resource/ListMetamodelRefactoring/Metamodel/TypeB.ecore", "Synchronizations.Tests.A2B.TypeB.nmf")]

namespace Synchronizations.Tests.A2B
{
    [TestClass]
    public class A2BTest
    {
        [TestMethod]
        public void A2BTestSuceeds()
        {
            RunA2BTest(new A2BHelperWithoutContext());
        }

        [TestMethod]
        public void A2BTestWithMappingSuceeds()
        {
            RunA2BTest( new A2BMapping() );
        }

        private static void RunA2BTest(ReflectiveSynchronization transformation )
        {
            //absolute path is needed for the execution from a junit test
            var pathInputModel1 = "A2B\\SampleInput.xmi";
            var pathOutputModel = "A2B\\SampleOutput.xmi";

            //load input models
            var repository = new ModelRepository();
            var inputModel1 = repository.Resolve( pathInputModel1 );
            var expectedOutput = repository.Resolve( pathOutputModel );

            if(inputModel1 == null)
            {
                throw new FileNotFoundException( "One of the Input Models was not found" );
            }

            var inputModelContainer = new InputModelContainer( inputModel1 );
            var outputModelContainer = new OutputModelContainer();

            var direction = SynchronizationDirection.LeftToRight;
            var changePropagartion = ChangePropagationMode.OneWay;

            A2BHelperWithoutContext.InputModelContainer = inputModelContainer;

            var startRule = transformation.GetSynchronizationRuleForSignature( typeof( InputModelContainer ), typeof( OutputModelContainer ) ) as SynchronizationRule<InputModelContainer, OutputModelContainer>;
            var context = transformation.Synchronize( startRule, ref inputModelContainer, ref outputModelContainer, direction, changePropagartion );

            Assert.AreEqual( expectedOutput.Descendants().Count(), outputModelContainer.OUT.Descendants().Count() );
        }
    }
}
