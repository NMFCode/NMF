using System;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Model
{
    /// <summary>
    /// Contains and owns a model instance.
    /// </summary>
    /// <typeparam name="T">Type of the model.</typeparam>
    public interface IModelContainerGrain<T> : IGrainWithGuidKey, ITransactionalStreamProvider<T>, IElementEnumeratorNode<T>, IContainsModel<T> where T : IResolvableModel
    {
        /// <summary>
        /// Loads a model from the given path.
        /// </summary>
        /// <param name="modelPath">Path to the model.</param>
        /// <returns></returns>
        Task LoadModelFromPath(string modelPath);

        /// <summary>
        /// Gets the path to the loaded model.
        /// </summary>
        /// <returns>Path to the loaded model.</returns>
        Task<string> GetModelPath();

        /// <summary>
        /// Executes an action on the model. If a new model element is created forwarding of changes is done via forwarding the action
        /// and not via messages.
        /// </summary>
        /// <param name="action">Action to execute on the model.</param>
        /// <param name="newModelElementCreated">Needs to be set to true if a new model element is created during the action.</param>
        /// <returns></returns>
        Task ExecuteSync(Action<T> action, bool newModelElementCreated = false);

        /// <summary>
        /// Executes an action on the model. If a new model element is created forwarding of changes is done via forwarding the action
        /// and not via messages.
        /// </summary>
        /// <param name="action">Action to execute on the model and state.</param>
        /// <param name="state">State that is passed through the action.</param>
        /// <param name="newModelElementCreated">Needs to be set to true if a new model element is created during the action.</param>
        /// <returns></returns>
        Task ExecuteSync(Action<T, object> action, object state, bool newModelElementCreated = false);

        /// <summary>
        /// Gets StreamIdentity of the stream that distributes model updates.
        /// </summary>
        /// <returns>Stream identity.</returns>
        Task<StreamIdentity> GetModelUpdateStream();

        /// <summary>
        /// This method is just here to ensure assembly loading of RailwayContainer by Orleans. 
        /// To be removed once NMF assembly registration has been changed.
        /// </summary>
        /// <returns></returns>
        Task<RailwayContainer> NeverCallMe();
    }
}