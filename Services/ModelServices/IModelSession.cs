﻿using NMF.Expressions;
using NMF.Models.Changes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes a working session for a model of a given type
    /// </summary>
    public interface IModelSession
    {
        /// <summary>
        /// The root element
        /// </summary>
        IModelElement Root { get; set; }

        /// <summary>
        /// Gets the model represented by this session
        /// </summary>
        Model Model { get; }

        /// <summary>
        /// True, if a redo operation is currently available, otherwise False
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// True, if an undo operation is currently available, otherwise False
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// True, if there are unsaved changes for this session, otherwise False
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Raised when the propery IsDirty changes
        /// </summary>
        event EventHandler IsDirtyChanged;

        /// <summary>
        /// Raised when an operation was performed
        /// </summary>
        event EventHandler<ModelChangeSet> PerformedOperation;

        /// <summary>
        /// Performs the given operation on the model stored in this session
        /// </summary>
        /// <param name="operation">The operation that should be performed</param>
        /// <returns>true, if the operation had an effect that can be undone, otherwise false</returns>
        bool PerformOperation(Action operation);

        /// <summary>
        /// Performs the given operation on the model stored in this session
        /// </summary>
        /// <param name="operation">The operation that should be performed</param>
        /// <returns>true, if the operation had an effect that can be undone, otherwise false</returns>
        Task<bool> PerformOperationAsync(Func<Task> operation);

        /// <summary>
        /// Performs a redo operation
        /// </summary>
        void Redo();

        /// <summary>
        /// Saves the current state of the model
        /// </summary>
        void Save();

        /// <summary>
        /// Saves the current state of the model
        /// </summary>
        /// <param name="target">the target where to save the model</param>
        void Save(Uri target);

        /// <summary>
        /// Performs an undo operation
        /// </summary>
        void Undo();

        /// <summary>
        /// Gets or sets the selected element in this session
        /// </summary>
        IEnumerable<IModelElement> SelectedElements { get; set; }

        /// <summary>
        /// Raised when the selected element changed
        /// </summary>
        event EventHandler SelectedElementChanged;
    }
}