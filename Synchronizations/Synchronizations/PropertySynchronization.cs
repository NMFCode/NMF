using NMF.Expressions;
using System;

namespace NMF.Synchronizations
{
    internal class PropertySynchronization<T> : IDisposable
    {
        public INotifyValue<T> Source { get; private set; }
        public Action<T> Target { get; private set; }

        public PropertySynchronization(INotifyValue<T> source, Action<T> target)
        {
            Source = source;
            Target = target;

            Source.Successors.SetDummy();
            Source.ValueChanged += Source_ValueChanged;
        }

        private void Source_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Target(Source.Value);
        }

        public void Dispose()
        {
            Source.ValueChanged -= Source_ValueChanged;
            Source.Successors.UnsetAll();
        }
    }
}
