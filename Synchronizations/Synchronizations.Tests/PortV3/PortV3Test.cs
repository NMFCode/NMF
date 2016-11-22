using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NMF.Models.Repository;
using PortV3Namespace;
using NMF.Synchronizations;
using NMF.Transformations;
using NMF.Models;

[assembly: ModelMetadata("http://www.eclipse.org/atl/atlTransformations/TypeA", "Synchronizations.Tests.PortV3.TypeA.nmf")]
[assembly: ModelMetadata("http://www.eclipse.org/atl/atlTransformations/TypeB", "Synchronizations.Tests.PortV3.TypeB.nmf")]

namespace Synchronizations.Tests.PortV3
{
    [TestClass]
    public class PortV3Test
    {
        [TestMethod]
        public void PortV3Suceeds()
        {
            //absolute path is needed for the execution from a junit test
            var absolutePathInputModel1 = "PortV3\\SampleInput.xmi";

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
            var transformation = new PortV3Namespace.PortV3();

            PortV3Namespace.PortV3.InputModelContainer = inputModelContainer;

            var context = transformation.Synchronize(transformation.SynchronizationRule<PortV3Namespace.PortV3.Model2ModelMainRule>(), ref inputModelContainer, ref outputModelContainer, direction, changePropagartion);

        }
    }
}
