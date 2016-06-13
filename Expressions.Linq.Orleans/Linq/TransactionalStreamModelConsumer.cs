﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Stateful;
using Orleans.Streams.Stateful.Endpoints;
using Orleans.Streams.Stateful.Messages;

namespace NMF.Expressions.Linq.Orleans
{
    /// <summary>
    /// Consumes stream items and receives them with regard to a model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public class TransactionalStreamModelConsumer<T, TModel> : TransactionalStreamRemoteObjectConsumer<T> where TModel : IResolvableModel
    {
        public TModel Model { get; private set; }
 
        public TransactionalStreamModelConsumer(IStreamProvider streamProvider, Func<Task> tearDownFunc = null, IList<T> items = null) : base(streamProvider, null, tearDownFunc, items)
        {
        }

        public async Task SetModelContainer(IModelContainerGrain<TModel> modelContainer, string modelPath = "")
        {
            if(modelPath == "")
                modelPath = await modelContainer.GetModelPath();
            Model = ModelLoader.Instance.LoadModel<TModel>(modelPath);
            var receiveContext = new LocalModelReceiveContext(Model);
            receiveContext.BuildCache(Model.Descendants().OfType<T>());
            ReceiveContext = receiveContext;
            await modelContainer.ModelIsLoaded();
            await MessageDispatcher.Subscribe(await modelContainer.GetModelUpdateStream());
        }

        public async Task SetLocalModel(IModelSiloGrain<TModel> localModel)
        {
            Model = (await localModel.GetModel()).Value;
            var receiveContext = new LocalModelReceiveContext(Model);
            receiveContext.BuildCache(Model.Descendants().OfType<T>());
            ReceiveContext = receiveContext;
        }

        protected override void SetupMessageDispatcher(StreamMessageDispatchReceiver dispatcher)
        {
            base.SetupMessageDispatcher(dispatcher);
            dispatcher.Register<ModelExecuteActionMessage<TModel>>(message =>
            {
                message.Execute(Model);
                return TaskDone.Done;
            });
        }

        public override Task TearDown()
        {
            Model = default(TModel);
            return base.TearDown();
        }
    }
}