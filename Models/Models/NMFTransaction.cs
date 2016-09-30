using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions;

namespace NMF.Models
{
    class NMFTransaction : IDisposable
    {
        private bool committed = false;
        private readonly ExecutionEngine engine = ExecutionEngine.Current;

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
            throw new NotImplementedException();
            
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
