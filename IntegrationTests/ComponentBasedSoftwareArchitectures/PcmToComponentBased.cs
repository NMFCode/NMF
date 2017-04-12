using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NMFExamples.ComponentBasedSystems;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Transformations;

namespace NMFExamples
{
    public static class PcmToComponentBasedSystems
    {
        public static int AnalyseConnectionViolations(Root_MM06 Root)
        {
            return (from assemblyConnector in Root.System.Connectors.OfType<ComponentBasedSystems.Assembly.AssemblyConnector>()
                    where !assemblyConnector.ProvidingComponent.Implements.ProvidedInterfaces.Contains(assemblyConnector.Interface)
                          || !assemblyConnector.UsingComponent.Implements.RequiredInterfaces.Contains(assemblyConnector.Interface)
                    select assemblyConnector).Distinct().Count();
        }

        public static int AnalyzeAllocationViolations(Root_MM06 Root)
        {
            return (from connector in Root.System.Connectors
                    from assemblyAllocation in Root.Allocation.Contexts
                    where assemblyAllocation.Assembly == connector.UsingComponent
                    from requiredAllocation in Root.Allocation.Contexts
                    where requiredAllocation.Assembly == connector.ProvidingComponent
                    where requiredAllocation.Container != assemblyAllocation.Container &&
                          !Root.Environment.Links.Any(link => link.ConnectedContainers.Contains(requiredAllocation.Container) && link.ConnectedContainers.Contains(assemblyAllocation.Container))
                    select connector).Distinct().Count();
        }

        public static INotifyValue<int> AnalyzeAllocationViolationsInc(Root_MM06 Root)
        {
            return Observable.Expression(() => (from connector in Root.System.Connectors
                                                from assemblyAllocation in Root.Allocation.Contexts
                                                where assemblyAllocation.Assembly == connector.UsingComponent
                                                from requiredAllocation in Root.Allocation.Contexts
                                                where requiredAllocation.Assembly == connector.ProvidingComponent
                                                where requiredAllocation.Container != assemblyAllocation.Container &&
                                                      !Root.Environment.Links.Any(link => link.ConnectedContainers.Contains(requiredAllocation.Container) && link.ConnectedContainers.Contains(assemblyAllocation.Container))
                                                select connector).Distinct().Count());
        }

        public static INotifyValue<int> AnalyzeConnectionViolationsInc(Root_MM06 Root)
        {
            return Observable.Expression(() => (from assemblyConnector in Root.System.Connectors.OfType<ComponentBasedSystems.Assembly.AssemblyConnector>()
                                                where !assemblyConnector.ProvidingComponent.Implements.ProvidedInterfaces.Contains(assemblyConnector.Interface)
                                                      || !assemblyConnector.UsingComponent.Implements.RequiredInterfaces.Contains(assemblyConnector.Interface)
                                                select assemblyConnector).Distinct().Count());
        }

        public static Model Transform(Model pcmModel, ChangePropagationMode changePropagation)
        {
            var result = new Model();

            var inputContainer = new PcmToMM06.InputModelContainer(pcmModel, pcmModel, pcmModel, pcmModel);
            var outputContainer = new PcmToMM06.OutputModelContainer(result);

            var transformation = new PcmToMM06.PCMto06(inputContainer, outputContainer);

            transformation.Synchronize(transformation.SynchronizationRule<PcmToMM06.PCMto06.Model2ModelMainRule>(), ref inputContainer, ref outputContainer, NMF.Synchronizations.SynchronizationDirection.LeftToRight, changePropagation);

            return result;
        }
    }
}
