namespace NMF.Expressions.Linq
{
    internal class BalancingStrategyProvider<T, TChunk> : IChunkBalancingStrategyProvider<T, TChunk>
    {
        private readonly IChunkBalancingStrategyProvider _provider;

        public BalancingStrategyProvider(IChunkBalancingStrategyProvider provider)
        {
            _provider = provider ?? new NoBalancingStrategy();
        }

        public IChunkBalancingStrategy<T, TChunk> CreateStrategy( IObservableChunk<T, TChunk> observableChunk )
        {
            return _provider.CreateStrategy( observableChunk );
        }
    }
}
