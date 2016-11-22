using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NMF.Models.Repository;
using A2BHelperWithoutContextNamespace;
using NMF.Synchronizations;
using NMF.Transformations;
using NMF.Models;

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

            //absolute path is needed for the execution from a junit test
            var absolutePathInputModel1 = "A2B\\SampleInput.xmi";

            //load input models
            var repository = new ModelRepository();
            var inputModel1 = repository.Resolve(absolutePathInputModel1);

            if (inputModel1 == null)
            {
                throw new FileNotFoundException("One of the Input Models was not found");
            }

            var inputModelContainer = new InputModelContainer(inputModel1);
            var outputModelContainer = new OutputModelContainer();

            var direction = SynchronizationDirection.LeftToRight;
            var changePropagartion = ChangePropagationMode.OneWay;
            A2BHelperWithoutContext transformation = new A2BHelperWithoutContext();

            A2BHelperWithoutContext.InputModelContainer = inputModelContainer;

            var context = transformation.Synchronize<InputModelContainer, OutputModelContainer>(transformation.SynchronizationRule<A2BHelperWithoutContext.Model2ModelMainRule>(), ref inputModelContainer, ref outputModelContainer, direction, changePropagartion);

        }
    }
}
