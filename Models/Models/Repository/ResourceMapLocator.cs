using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Models.Repository
{
    public class ResourceMapLocator : MapLocator
    {
        public Assembly Assembly { get; private set; }

        public ResourceMapLocator(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            Assembly = assembly;
        }

        public override Stream Open(Uri repositoryId)
        {
            return Assembly.GetManifestResourceStream(Mappings[repositoryId]);
        }
    }
}
