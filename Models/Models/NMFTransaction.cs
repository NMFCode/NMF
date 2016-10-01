using System;
using System.Linq;
using NMF.Expressions;
using NMF.Models.Evolution;

namespace NMF.Models
{
    class NMFTransaction : IDisposable
    {
        private bool _committed;
        private readonly ExecutionEngine _engine = ExecutionEngine.Current;
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder();

        public NMFTransaction(IModelElement rootElement)
        {
            if (rootElement == null) throw new ArgumentNullException(nameof(rootElement));
            _recorder.Start(rootElement);
            _engine.BeginTransaction();
        }
        public void Commit()
        {
            _engine.CommitTransaction();
            _committed = true;
        }
        public void Rollback()
        {
            var modelChanges = _recorder.GetModelChanges().TraverseFlat().Reverse();
            _recorder.Stop();

            foreach (var change in modelChanges)
                change.Undo();
            _engine.RollbackTransaction();
        }
        public void Dispose()
        {
            if (!_committed)
                Rollback();
        }

    }
}
