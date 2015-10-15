using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public class UnresolvedModelElementEventArgs : EventArgs
    {
        public UnresolvedModelElementEventArgs(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");

            Uri = uri;
        }

        public Uri Uri { get; private set; }

        public IModelElement ModelElement { get; set; }
    }
}
