using NMF.Models;
using NMF.Synchronizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class RunningSynchronization : IRunningSynchronization
    {
        private readonly ISynchronizationContext _context;
        private readonly Uri _leftUri;
        private readonly Uri _rightUri;
        private readonly IModelElement _leftRoot;
        private readonly IModelElement _rightRoot;

        public RunningSynchronization(ISynchronizationContext context, Uri leftUri, Uri rightUri, IModelElement leftRoot, IModelElement rightRoot, IModelSynchronization synchronization)
        {
            _context = context;
            _leftUri = leftUri;
            _rightUri = rightUri;
            _leftRoot = leftRoot;
            _rightRoot = rightRoot;
            Synchronization = synchronization;
        }

        public IModelSynchronization Synchronization { get; }

        public IEnumerable<Uri> SynchronizedUris
        {
            get
            {
                yield return _leftUri;
                yield return _rightUri;
            }
        }

        public IEnumerable<IModelElement> SynchronizedModels
        {
            get
            {
                yield return _leftRoot;
                yield return _rightRoot;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
