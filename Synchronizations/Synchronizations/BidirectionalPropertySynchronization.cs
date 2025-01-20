using NMF.Expressions;
using System;

namespace NMF.Synchronizations
{
    internal class BidirectionalPropertySynchronization<T> : IDisposable
    {
        public INotifyReversableValue<T> Source1 { get; private set; }
        public INotifyReversableValue<T> Source2 { get; private set; }

        public BidirectionalPropertySynchronization(INotifyReversableValue<T> source1, INotifyReversableValue<T> source2)
        {
            Source1 = source1;
            Source2 = source2;

            Source1.Successors.SetDummy();
            Source2.Successors.SetDummy();
            Source1.ValueChanged += Source1_ValueChanged;
            Source2.ValueChanged += Source2_ValueChanged;
        }

        private void Source2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Source1.Value = Source2.Value;
        }

        private void Source1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Source2.Value = Source1.Value;
        }

        public void Dispose()
        {
            Source1.ValueChanged -= Source1_ValueChanged;
            Source2.ValueChanged -= Source2_ValueChanged;
            Source1.Successors.UnsetAll();
            Source2.Successors.UnsetAll();
        }
    }
}
