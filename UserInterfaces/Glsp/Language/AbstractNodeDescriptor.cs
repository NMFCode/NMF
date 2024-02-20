using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Defines a base class for a description of abstract classes
    /// </summary>
    /// <typeparam name="T">The semantic type of elements described by this descriptor</typeparam>
    public class AbstractNodeDescriptor<T> : NodeDescriptor<T>
    {
        /// <inheritdoc/>
        protected internal override void DefineLayout()
        {
            // intentionally left blank for abstract nodes
        }

        /// <inheritdoc/>
        public override bool CanCreateElement => false;

        /// <inheritdoc/>
        public override T CreateElement(string profile, object parent)
        {
            throw new NotSupportedException("Attempted to create an element for the abstract descriptor " + GetType().Name);
        }
    }
}
