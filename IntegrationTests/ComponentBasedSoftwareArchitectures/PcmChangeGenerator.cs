using NMF.Models;
using NMF.Models.Changes;
using NMF.Models.Repository;
using NMFExamples.Pcm.Allocation;
using NMFExamples.Pcm.Core.Composition;
using NMFExamples.Pcm.Repository;
using NMFExamples.Pcm.Resourceenvironment;
using NMFExamples.Pcm.System;
using System;

namespace NMFExamples
{
    public static class PcmChangeGenerator
    {
        public const int NumChanges = 7;

        public static void PerformChanges(IRepository pcmRepository, ISystem0 system, IResourceEnvironment resourceEnvironment, Allocation allocation, Model model, string target)
        {
            var last = false;
            var i = 0;
            ModelChangeRecorder recorder = null;
            StartNextRecorder(model, target, last, ref i, ref recorder);

            var container = new ResourceContainer
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "A random new container",
            };
            resourceEnvironment.ResourceContainer_ResourceEnvironment.Add(container);

            StartNextRecorder(model, target, last, ref i, ref recorder);
            // next, we allocate an assembly context to this container
            var allocationCtx = allocation.AllocationContexts_Allocation[0];
            var oldContainer = allocationCtx.ResourceContainer_AllocationContext;
            allocationCtx.ResourceContainer_AllocationContext = container;
            StartNextRecorder(model, target, last, ref i, ref recorder);
            // to repair the situation, we create a link between the old container and the new one
            var link = new LinkingResource
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "SnailConnection"
            };
            link.ConnectedResourceContainers_LinkingResource.Add(oldContainer);
            link.ConnectedResourceContainers_LinkingResource.Add(container);
            resourceEnvironment.LinkingResources__ResourceEnvironment.Add(link);
            StartNextRecorder(model, target, last, ref i, ref recorder);
            // create a new dummy interface
            var dummyInterface = new OperationInterface
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "Foobar"
            };
            pcmRepository.Interfaces__Repository.Add(dummyInterface);

            StartNextRecorder(model, target, last, ref i, ref recorder);

            // create a dummy component
            var dummyComponent = new BasicComponent
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "Dummy"
            };
            var realAssembly = allocationCtx.AssemblyContext_AllocationContext;
            var realComponent = realAssembly.EncapsulatedComponent__AssemblyContext;
            var realProvided = realComponent.ProvidedRoles_InterfaceProvidingEntity[0] as IOperationProvidedRole;
            var realInterface = realProvided.ProvidedInterface__OperationProvidedRole;
            var requireReal = new OperationRequiredRole
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "Required",
                RequiredInterface__OperationRequiredRole = realInterface
            };
            dummyComponent.RequiredRoles_InterfaceRequiringEntity.Add(requireReal);

            var requireFake = new OperationRequiredRole
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "Fake",
                RequiredInterface__OperationRequiredRole = dummyInterface
            };
            dummyComponent.RequiredRoles_InterfaceRequiringEntity.Add(requireFake);
            pcmRepository.Components__Repository.Add(dummyComponent);

            StartNextRecorder(model, target, last, ref i, ref recorder);

            // create an assembly for the dummy
            var dummyAssembly = new AssemblyContext
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "DummyAssembly",
                EncapsulatedComponent__AssemblyContext = dummyComponent
            };
            system.AssemblyContexts__ComposedStructure.Add(dummyAssembly);

            // create a connector from the dummy to a real assembly
            var connector = new AssemblyConnector
            {
                Id = Guid.NewGuid().ToString(),
                EntityName = "A connector",
                ProvidedRole_AssemblyConnector = realProvided,
                ProvidingAssemblyContext_AssemblyConnector = realAssembly,
                RequiredRole_AssemblyConnector = requireFake,
                RequiringAssemblyContext_AssemblyConnector = dummyAssembly
            };
            system.Connectors__ComposedStructure.Add(connector);

            StartNextRecorder(model, target, last, ref i, ref recorder);
            // fix the connector
            connector.RequiredRole_AssemblyConnector = requireReal;
            last = true;
            StartNextRecorder(model, target, last, ref i, ref recorder);
        }

        private static void StartNextRecorder(Model model, string target, bool last, ref int i, ref ModelChangeRecorder recorder)
        {
            if (target != null)
            {
                if (recorder != null)
                {
                    recorder.Stop();
                    var changes = recorder.GetModelChanges();
                    (model.Repository as ModelRepository).Save(changes, string.Format(target, i++));
                }
                if (!last)
                {
                    recorder = new ModelChangeRecorder();
                    recorder.Start(model);
                }
            }
        }
    }
}
