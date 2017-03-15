using System;
using NMF.Expressions;
using NMF.Models.Evolution;
using NMF.Models.Repository;

namespace NMF.Models
{
    public class ModelTransaction : IDisposable
    {
        private bool _committed;
        private readonly ExecutionEngine _engine = ExecutionEngine.Current;
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder(true);
        private IModelRepository _repository;

        public ModelTransaction(IModelElement rootElement)
        {
            if (rootElement == null) throw new ArgumentNullException(nameof(rootElement));
            _repository = rootElement.Model.Repository;
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
        }

    }
}
