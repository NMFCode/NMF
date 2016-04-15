using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans
{
    public interface IIncrementalStreamProcessorAggregateFactory
    {
        IGrainFactory GrainFactory { get; }

        Task<IStreamProcessorAggregate<ContainerElement<TIn>, ContainerElement<TOut>>> CreateSelect<TIn, TOut>(Expression<Func<ContainerElement<TIn>, TOut>> selectionFunc, IList<StreamIdentity> streamIdentities);

        Task<IStreamProcessorAggregate<ContainerElement<TIn>, ContainerElement<TIn>>> CreateWhere<TIn>(Expression<Func<ContainerElement<TIn>, bool>> filterFunc, IList<StreamIdentity> streamIdentities);
    }
}