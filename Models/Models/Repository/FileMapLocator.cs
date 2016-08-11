using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public class FileMapLocator : MapLocator
    {
        public FileMapLocator() : base() { }

        public FileMapLocator(IDictionary<Uri, string> mappings) : base(mappings) { }

        public override Stream Open(Uri repositoryId)
        {
            return new FileStream(Mappings[repositoryId], FileMode.Open, FileAccess.Read);
        }
    }
}
