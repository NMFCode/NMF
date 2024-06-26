﻿namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    ///  Marker interface for operations.
    /// </summary>
    public abstract class Operation : ExecutableAction
    {

        /// <summary>
        ///  Discriminator property to make operations distinguishable from plain Actions.
        /// </summary>
        public bool IsOperation => true;


        /// <inheritdoc/>
        public override bool RequireTransaction() => true;
    }
}
