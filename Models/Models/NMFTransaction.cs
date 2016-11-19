using System;
using System.Linq;
using System.Transactions;
using NMF.Expressions;
using NMF.Models.Evolution;
using NMF.Models.Repository;

namespace NMF.Models
{
    public class NMFTransaction : IDisposable
    {
        private bool _committed;
        private readonly ExecutionEngine _engine = ExecutionEngine.Current;
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder(true);
        private IModelRepository _repository;
        //private TransactionScope _scope;

        public NMFTransaction(IModelElement rootElement)
        {
            if (rootElement == null) throw new ArgumentNullException(nameof(rootElement));
            _repository = rootElement.Model.Repository;
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
            var modelChanges = _recorder.GetModelChanges().Changes;
            _recorder.Stop();
            modelChanges.Reverse();

            foreach (var change in modelChanges)
                change.Invert(_repository);
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
