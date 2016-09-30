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
        private Collection<IModelChange> changes = new Collection<IModelChange>();

        public NMFTransaction()
        {
            engine.BeginTransaction();
        }
        public void Commit()
        {
            engine.CommitTransaction();
            committed = true;
            throw new NotImplementedException();
        }
        public void Rollback()
        {
            foreach (var change in changes)
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
