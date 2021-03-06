//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMFExamples.ComponentBasedSystems;
using NMFExamples.ComponentBasedSystems.Assembly;
using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using global::System.Collections;
using global::System.Collections.Generic;
using global::System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NMFExamples.ComponentBasedSystems.Deployment
{
    
    
    public class Container_MM06LinksCollection : ObservableOppositeOrderedSet<IContainer_MM06, ILink>
    {
        
        public Container_MM06LinksCollection(IContainer_MM06 parent) : 
                base(parent)
        {
        }
        
        private void OnItemDeleted(object sender, global::System.EventArgs e)
        {
            this.Remove(((ILink)(sender)));
        }
        
        protected override void SetOpposite(ILink item, IContainer_MM06 parent)
        {
            if ((parent != null))
            {
                item.Deleted += this.OnItemDeleted;
                item.ConnectedContainers.Add(parent);
            }
            else
            {
                item.Deleted -= this.OnItemDeleted;
                item.ConnectedContainers.Remove(this.Parent);
            }
        }
    }
}

