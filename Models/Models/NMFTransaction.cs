using System;
using System.Linq;
using System.Transactions;
using NMF.Expressions;
using NMF.Models.Evolution;

namespace NMF.Models
{
    class NMFTransaction : IDisposable
    {
        private bool _committed;
        private readonly ExecutionEngine _engine = ExecutionEngine.Current;
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder();
        //private TransactionScope _scope;

        public NMFTransaction(IModelElement rootElement)
        {
            if (rootElement == null) throw new ArgumentNullException(nameof(rootElement));
            _recorder.Start(rootElement);
            //_scope = new TransactionScope();
            _engine.BeginTransaction();
        }
        public void Commit()
        {
            //if (_scope == null) throw new InvalidOperationException();
            _engine.CommitTransaction();
            _committed = true;
            //_scope.Complete();
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
            //if (_scope != null)
            //{
            //    _scope.Dispose();
            //    _scope = null;
            //}
        }

    }
}
