using System;
using System.Linq;
using NMF.Expressions;
using NMF.Models.Changes;
using NMF.Models.Repository;

namespace NMF.Models
{
    public class ModelTransaction : IDisposable
    {
        private bool _committed;
        private readonly ExecutionEngine _engine = ExecutionEngine.Current;
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder(true);
        private readonly IModelRepository _repository;

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
            _recorder.Stop();
            var modelChanges = _recorder.GetModelChanges();

            var inverted = modelChanges.CreateInvertedChangeSet();

            inverted.Apply();
            
            _engine.RollbackTransaction();
        }
        public void Dispose()
        {
            if (!_committed)
                Rollback();
        }

    }
}
