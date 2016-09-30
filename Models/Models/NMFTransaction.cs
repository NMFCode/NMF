using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions;
using NMF.Models.Evolution;

namespace NMF.Models
{
    class NMFTransaction : IDisposable
    {
        private bool committed = false;
        private readonly ExecutionEngine engine = ExecutionEngine.Current;
        private readonly ModelChangeRecorder recorder = new ModelChangeRecorder();

        public NMFTransaction(IModelElement rootElement)
        {
            recorder.Start(rootElement);
            engine.BeginTransaction();
        }
        public void Commit()
        {
            engine.CommitTransaction();
            committed = true;
        }
        public void Rollback()
        {
            var modelChanges = recorder.GetModelChanges().TraverseFlat();
            recorder.Stop();

            foreach (var change in modelChanges)
                change.Undo();
            engine.RollbackTransaction();
        }
        public void Dispose()
        {
            if (!committed)
            {
                Rollback();
            }
        }

    }
}
