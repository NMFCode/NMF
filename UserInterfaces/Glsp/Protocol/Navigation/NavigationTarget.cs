﻿using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Navigation
{
    /// <summary>
    /// A NavigationTarget identifies the object we want to navigate to via its uri and may further provide a label to display 
    /// for the client. Additionally, generic arguments may be used to to encode any domain- or navigation type-specific information.
    /// </summary>
    public class NavigationTarget
    {

        /// <summary>
        ///  URI to identify the object we want to navigate to.
        /// </summary>
        public string Uri { get; init; }

        /// <summary>
        ///  Optional label to display to the user.
        /// </summary>
        public string Label { get; init; }

        /// <summary>
        ///  Domain-specific arguments that may be interpreted directly or resolved further.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }
    }
}
