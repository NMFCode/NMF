using System;
using System.Collections;
using System.Collections.Generic;

namespace NMF.Expressions.Linq
{
    internal class ChunkIndexedExpression<T, TResult> : IEnumerableExpression<TResult>
    {
        private readonly IEnumerableExpression<T> _source;
        private INotifyEnumerable<TResult> _notifiable;
        private readonly int _chunkSize;
        private readonly Func<IEnumerableExpression<(T, int)>, int, TResult> _resultConverter;
        private readonly IChunkBalancingStrategyProvider<(T, int), TResult> _balancingStrategyProvider;

        public ChunkIndexedExpression( IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultConverter, IChunkBalancingStrategyProvider<(T, int), TResult> balancingStrategyProvider )
        {
            _source = source;
            _chunkSize = chunkSize;
            _resultConverter = resultConverter;
            _balancingStrategyProvider = balancingStrategyProvider;
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            if(_notifiable == null)
            {
                _notifiable = _source.AsNotifiable().ChunkIndexed( _chunkSize, _resultConverter, _balancingStrategyProvider );
            }
            return _notifiable;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            return ChunkExtensions.ChunkIndexedCore( _source, _chunkSize, _resultConverter ).GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class ChunkExpression<T, TResult> : IEnumerableExpression<TResult>
    {
        private readonly IEnumerableExpression<T> _source;
        private INotifyEnumerable<TResult> _notifiable;
        private readonly int _chunkSize;
        private readonly Func<IEnumerableExpression<T>, int, TResult> _resultConverter;
        private readonly IChunkBalancingStrategyProvider<T, TResult> _balancingStrategyProvider;

        public ChunkExpression( IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultConverter, IChunkBalancingStrategyProvider<T, TResult> balancingStrategyProvider )
        {
            _source = source;
            _chunkSize = chunkSize;
            _resultConverter = resultConverter;
            _balancingStrategyProvider = balancingStrategyProvider;
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            if(_notifiable == null)
            {
                _notifiable = _source.AsNotifiable().Chunk( _chunkSize, _resultConverter, _balancingStrategyProvider );
            }
            return _notifiable;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            return ChunkExtensions.ChunkCore( _source, _chunkSize, _resultConverter ).GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
