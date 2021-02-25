using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Denotes the change propagation mode
    /// </summary>
    public enum ChangePropagationMode
    {
        /// <summary>
        /// No change propagation is performed
        /// </summary>
        None,
        /// <summary>
        /// Change propagation is performed from the source to the target model
        /// </summary>
        OneWay,
        /// <summary>
        /// Changes are propagated in both directions
        /// </summary>
        TwoWay
    }
}
