using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a locator that maps to assembly embedded resources
    /// </summary>
    public class ResourceMapLocator : MapLocator
    {
        /// <summary>
        /// Gets the assembly for which the resource mapper maps to
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="assembly">the target assembly</param>
        /// <exception cref="ArgumentNullException">thrown if the assembly is null</exception>
        public ResourceMapLocator(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Assembly = assembly;
        }

        /// <inheritdoc />
        public override Stream Open(Uri repositoryId)
        {
            return Assembly.GetManifestResourceStream(Mappings[repositoryId]);
        }
    }
}
