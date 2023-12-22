using NMF.Expressions;
using NMF.Models.Changes;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes the abstract base of the standard implementation for a model session
    /// </summary>
    public abstract class ModelSession
    {
        private readonly TransactionUndoStack _undoStack = new TransactionUndoStack();
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder();
        private bool _isDirty;
        private IModelElement _selectedElement;

        private readonly ModelServer _server;
        private readonly string _path;

        /// <summary>
        /// Creates a new model session for the given server
        /// </summary>
        /// <param name="server">The model server</param>
        /// <param name="element">The element for which the session is opened</param>
        /// <param name="path">The file system path</param>
        public ModelSession(ModelServer server, IModelElement element, string path)
        {
            _server = server;
            _path = path;
            _recorder.Start(element);
        }

        /// <summary>
        /// Performs an undo operation
        /// </summary>
        public void Undo() => _undoStack.Undo();

        /// <summary>
        /// Performs a redo operation
        /// </summary>
        public void Redo() => _undoStack.Redo();

        /// <summary>
        /// True, if an undo operation is currently available, otherwise False
        /// </summary>
        public bool CanUndo => _undoStack.CanUndo;

        /// <summary>
        /// True, if a redo operation is currently available, otherwise False
        /// </summary>
        public bool CanRedo => _undoStack.CanRedo;

        /// <summary>
        /// Performs the given operation on the model stored in this session
        /// </summary>
        /// <param name="operation">The operation that should be performed</param>
        public void PerformOperation(Action operation)
        {
            if (operation == null) return;
            operation();
            _recorder.Stop(detachAll: false);
            var transaction = _recorder.GetModelChanges();
            if (transaction.Changes.Count > 0)
            {
                _undoStack.Notify(transaction);
                IsDirty = true;
                PerformedOperation?.Invoke(this, EventArgs.Empty);
                _server.InformOtherSessions(this);
            }
            _recorder.Start();
        }

        internal void NotifyOperationPerformed()
        {
            _recorder.Stop(detachAll: false);
            var transaction = _recorder.GetModelChanges();
            if (transaction.Changes.Count > 0)
            {
                PerformedOperation?.Invoke(this, EventArgs.Empty);
            }
            _recorder.Start();
        }

        /// <summary>
        /// Saves the current state of the model
        /// </summary>
        public void Save()
        {
            _server.Repository.Save(Element, _path);
            IsDirty = false;
        }

        /// <summary>
        /// The root element of this session
        /// </summary>
        protected abstract IModelElement Element { get; }


        /// <summary>
        /// True, if there are unsaved changes for this session, otherwise False
        /// </summary>
        public bool IsDirty
        {
            get => _isDirty;
            private set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    IsDirtyChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised when an operation was performed
        /// </summary>
        public event EventHandler PerformedOperation;

        /// <summary>
        /// Raised when the propery IsDirty changes
        /// </summary>
        public event EventHandler IsDirtyChanged;

        /// <summary>
        /// Gets or sets the selected element in this session
        /// </summary>
        public IModelElement SelectedElement
        {
            get => _selectedElement;
            set
            {
                if (_selectedElement != value)
                {
                    var old = _selectedElement;
                    _selectedElement = value;
                    SelectedElementChanged?.Invoke(this, new ValueChangedEventArgs(old, value));
                }
            }
        }

        /// <summary>
        /// Raised when the selected element changed
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> SelectedElementChanged;
    }

    /// <summary>
    /// Default implementation for a model session
    /// </summary>
    /// <typeparam name="T">The element type for the model session</typeparam>
    public class ModelSession<T> : ModelSession, IModelSession<T> where T : IModelElement
    {
        private readonly T _element;

        /// <summary>
        /// Creates a new model session for the given server
        /// </summary>
        /// <param name="server">The model server</param>
        /// <param name="element">The element for which the session is opened</param>
        /// <param name="path">The file system path</param>
        public ModelSession(ModelServer server, T element, string path) : base(server, element, path)
        {
            _element = element;
        }


        /// <inheritdoc />
        public T Root => _element;

        /// <inheritdoc />
        protected override IModelElement Element => _element;
    }
}
