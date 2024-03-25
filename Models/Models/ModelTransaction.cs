using System;
using System.Linq;
using NMF.Expressions;
using NMF.Models.Changes;
using NMF.Models.Repository;

namespace NMF.Models
{
    /// <summary>
    /// Denotes a model transaction
    /// </summary>
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    public class ModelTransaction : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        private bool _committed;
        private readonly ExecutionEngine _engine = ExecutionEngine.Current;
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder();

        /// <summary>
        /// Creates a new model transaction
        /// </summary>
        /// <param name="rootElement">The root element</param>
        /// <exception cref="ArgumentNullException">Thrown if rootElement is null</exception>
        public ModelTransaction(IModelElement rootElement)
        {
            if (rootElement == null) throw new ArgumentNullException(nameof(rootElement));
            _recorder.Start(rootElement);
            _engine.BeginTransaction();
        }

        /// <summary>
        /// Commits the transaction
        /// </summary>
        public void Commit()
        {
            _engine.CommitTransaction();
            _committed = true;
        }

        /// <summary>
        /// Roll back the transaction
        /// </summary>
        public void Rollback()
        {
            _recorder.Stop();
            var modelChanges = _recorder.GetModelChanges();

            var inverted = modelChanges.CreateInvertedChangeSet();

            inverted.Apply();
            
            _engine.RollbackTransaction();
        }

        /// <summary>
        /// True, if the transaction was committed, otherwise False
        /// </summary>
        public bool IsComitted => _committed;

        /// <inheritdoc />
        public virtual void Dispose()
        {
            if (!_committed)
                Rollback();
        }

    }
}
