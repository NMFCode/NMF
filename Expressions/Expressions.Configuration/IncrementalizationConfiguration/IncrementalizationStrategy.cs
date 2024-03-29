//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NMF.Expressions.IncrementalizationConfiguration
{
    
    /// <summary>
    /// Denotes the different possibilities for a function incrementalization
    /// </summary>
    [ModelRepresentationClassAttribute("http://nmf.codeplex.com/incrementalizationConfig#//IncrementalizationStrategy/")]
    public enum IncrementalizationStrategy
    {
        /// <summary>
        /// Denotes that functions should be incrementalized on instruction-level
        /// </summary>
        InstructionLevel = 0,
        /// <summary>
        /// Denotes that functions should be incrementalized by promoting cross-references to arguments
        /// </summary>
        ArgumentPromotion = 1,
        /// <summary>
        /// Denotes that functions should be incrementalized by listening to repository changes
        /// </summary>
        ListenRepositoryChanges = 2,
        /// <summary>
        /// Denotes that functions should be incrementalized by augmentations
        /// </summary>
        UseAugmentation = 3,
    }
}

