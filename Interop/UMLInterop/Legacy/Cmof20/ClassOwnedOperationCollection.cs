//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NMF.Interop.Legacy.Cmof
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using NMF.Expressions;
    using NMF.Expressions.Linq;
    using NMF.Models;
    using NMF.Models.Meta;
    using NMF.Models.Collections;
    using NMF.Models.Expressions;
    using NMF.Collections.Generic;
    using NMF.Collections.ObjectModel;
    using NMF.Serialization;
    using NMF.Utilities;
    using System.Collections.Specialized;
    using NMF.Models.Repository;
    using System.Globalization;
    
    
    /// <summary>
    /// Denotes a class to implement the ownedOperation reference
    /// </summary>
    public class ClassOwnedOperationCollection : ObservableOppositeOrderedSet<NMF.Interop.Legacy.Cmof.IClass, NMF.Interop.Legacy.Cmof.IOperation>
    {
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent Class</param>
        public ClassOwnedOperationCollection(NMF.Interop.Legacy.Cmof.IClass parent) : 
                base(parent)
        {
        }
        
        private void OnItemParentChanged(object sender, ValueChangedEventArgs e)
        {
            if ((e.NewValue != this.Parent))
            {
                this.Remove(((NMF.Interop.Legacy.Cmof.IOperation)(sender)));
            }
        }
        
        /// <summary>
        /// Sets the opposite of the given item
        /// </summary>
        /// <param name="item">the item</param>
        /// <param name="newParent">the new parent or null, if the item is removed from the collection</param>
        protected override void SetOpposite(NMF.Interop.Legacy.Cmof.IOperation item, NMF.Interop.Legacy.Cmof.IClass newParent)
        {
            if ((newParent != null))
            {
                item.ParentChanged += this.OnItemParentChanged;
                item.Class = newParent;
            }
            else
            {
                item.ParentChanged -= this.OnItemParentChanged;
                if ((item.Class == this.Parent))
                {
                    item.Class = newParent;
                }
            }
        }
    }
}