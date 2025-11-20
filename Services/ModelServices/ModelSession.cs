using NMF.Expressions;
using NMF.Models.Changes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes the standard implementation for a model session
    /// </summary>
    public class ModelSession : IModelSession
    {
        private readonly TransactionUndoStack _undoStack = new TransactionUndoStack();
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder();
        private bool _isDirty;
        private readonly List<IModelElement> _selectedElements = [];
        private IModelElement _rootElement;
        private Model _model;

        /// <summary>
        /// Creates a new model session for the given server
        /// </summary>
        /// <param name="element">The element for which the session is opened</param>
        /// <param name="model">The encapsulated model</param>
        public ModelSession(IModelElement element, Model model)
        {
            ArgumentNullException.ThrowIfNull(model);

            _recorder.Start(model);
            _model = model;
            Root = element;
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
        public bool PerformOperation(Action operation)
        {
            if (operation == null) return false;
            try
            {
                operation();
                _recorder.Stop(detachAll: false);
                var transaction = _recorder.GetModelChanges();
                if (transaction.Changes.Count > 0)
                {
                    _undoStack.Notify(transaction);
                    IsDirty = true;
                    PerformedOperation?.Invoke(this, transaction);
                    OnModelChanged();
                }
                return transaction.Changes.Count > 0;
            }
            catch
            {
                if (_recorder.IsRecording)
                {
                    _recorder.Stop(detachAll: false);
                }
                var transaction = _recorder.GetModelChanges();
                if (transaction.Changes.Count > 0)
                {
                    transaction.Invert();
                }
                throw;
            }
            finally
            {
                _recorder.Reset();
                _recorder.Start();
            }
        }

        /// <summary>
        /// Gets called when a change of the model neeeds to be propagated
        /// </summary>
        protected virtual void OnModelChanged()
        {
        }

        internal void NotifyOperationPerformed()
        {
            _recorder.Stop(detachAll: false);
            var transaction = _recorder.GetModelChanges();
            if (transaction.Changes.Count > 0)
            {
                PerformedOperation?.Invoke(this, transaction);
            }
            _recorder.Start();
        }

        /// <inheritdoc />
        public virtual void Save(Uri target)
        {
            IsDirty = false;
        }

        /// <inheritdoc />
        public void Save() => Save(null);

        /// <summary>
        /// Performs the given operation on the model stored in this session
        /// </summary>
        /// <param name="operation">The operation that should be performed</param>
        public async Task<bool> PerformOperationAsync(Func<Task> operation)
        {
            if (operation == null) return false;
            try
            {
                await operation();
                _recorder.Stop(detachAll: false);
                var transaction = _recorder.GetModelChanges();
                if (transaction.Changes.Count > 0)
                {
                    _undoStack.Notify(transaction);
                    IsDirty = true;
                    PerformedOperation?.Invoke(this, transaction);
                    OnModelChanged();
                }
                return transaction.Changes.Count > 0;
            }
            catch
            {
                if (_recorder.IsRecording)
                {
                    _recorder.Stop(detachAll: false);
                }
                var transaction = _recorder.GetModelChanges();
                if (transaction.Changes.Count > 0)
                {
                    transaction.Invert();
                }
                throw;
            }
            finally
            {
                _recorder.Reset();
                _recorder.Start();
            }
        }

        /// <summary>
        /// The root element of this session
        /// </summary>
        public IModelElement Root
        {
            get => _rootElement;
            set
            {
                if (_rootElement != value)
                {
                    _rootElement = value;
                    _selectedElements.Clear();
                    _isDirty = false;
                    if (_model == null)
                    {
                        _model = value?.Model;
                    }
                    else if (value != null && value.Model != _model)
                    {
                        _model.RootElements.Add(value);
                    }
                    RootElementChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }


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
        public event EventHandler<ModelChangeSet> PerformedOperation;

        /// <summary>
        /// Raised when the propery IsDirty changes
        /// </summary>
        public event EventHandler IsDirtyChanged;

        /// <summary>
        /// Raised when the root element changes
        /// </summary>
        public event EventHandler RootElementChanged;

        /// <summary>
        /// Gets or sets the selected element in this session
        /// </summary>
        public IEnumerable<IModelElement> SelectedElements
        {
            get => _selectedElements;
            set
            {
                if (value != null)
                {
                    if (_selectedElements.SequenceEqual(value)) return;
                    _selectedElements.Clear();
                    _selectedElements.AddRange(value);
                    SelectedElementChanged?.Invoke(this, EventArgs.Empty);
                }
                else if (_selectedElements.Count > 0)
                {
                    _selectedElements.Clear();
                    SelectedElementChanged?.Invoke(this, EventArgs.Empty);
                }
                OnElementSelect(value);
            }
        }

        /// <inheritdoc />
        public Model Model => _model;

        /// <inheritdoc />
        public virtual string LocalPath => _model.ModelUri.IsFile ? _model.ModelUri.LocalPath : null;

        /// <summary>
        /// Gets called when an element is selected
        /// </summary>
        /// <param name="selected">the selected elements</param>
        protected virtual void OnElementSelect(IEnumerable<IModelElement> selected) { }

        /// <summary>
        /// Raised when the selected element changed
        /// </summary>
        public event EventHandler SelectedElementChanged;
    }
}
