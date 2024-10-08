﻿using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.BaseProtocol;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Contracts
{
    /// <summary>
    /// Denotes a session with a client
    /// </summary>
    public interface IGlspSession
    {
        /// <summary>
        /// The root of the graph
        /// </summary>
        GGraph Root { get; }

        /// <summary>
        /// Gets the graphical language used in this session
        /// </summary>
        GraphicalLanguage Language { get; }

        /// <summary>
        /// Gets the trace to trace skeletons created for semantic objects
        /// </summary>
        ISkeletonTrace Trace { get; }

        /// <summary>
        /// The selected elements
        /// </summary>
        GElement[] SelectedElements { get; set; }

        /// <summary>
        /// True, if the client session contains unsaved changes, otherwise false
        /// </summary>
        bool IsDirty { get; set; }

        /// <summary>
        /// Send the given action to the client
        /// </summary>
        /// <param name="action">The action to send</param>
        void SendToClient(BaseAction action);

        /// <summary>
        /// Sends the entire graph to the client
        /// </summary>
        void Synchronize();

        /// <summary>
        /// True, if there is a transaction to undo, otherwise false
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// True, if there is a transaction to redo, otherwise false
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Undo the last transaction
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo the last transaction
        /// </summary>
        void Redo();

        /// <summary>
        /// Initializes the client for the given source model URI
        /// </summary>
        /// <param name="uri"></param>
        Task InitializeAsync(Uri uri);

        /// <summary>
        /// Saves the content at the given URI
        /// </summary>
        /// <param name="uri">the URI where to save the session content or null, if the model should be saved at the default location</param>
        void Save(Uri uri);

        /// <summary>
        /// Sends a request to the client and waits for the response
        /// </summary>
        /// <param name="request">The request to the client</param>
        /// <returns>A task that completes when the client returns a response</returns>
        Task<ResponseAction> RequestAsync(RequestAction request);
    }
}
