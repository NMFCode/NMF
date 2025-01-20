using NMF.Models.Changes;
using NMF.Models.Services;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Server.UndoRedo
{
    /// <summary>
    /// Denotes an undo stack for GLSP
    /// </summary>
    public class GlspUndoStack
    {
        private readonly List<UndoStackEntry> _recordedChanges = new();
        private int _index = 0;

        /// <summary>
        /// Notifies that a custom change set was performed
        /// </summary>
        /// <param name="changeSet">The change set that was performed</param>
        public void Notify(ModelChangeSet changeSet)
        {
            _recordedChanges.Insert(_index, new ChangeSetEntry(changeSet));
            CutAfterCurrentIndex();
        }

        /// <summary>
        /// Notifies that a model operation was performed
        /// </summary>
        public void NotifyModelOperation()
        {
            _recordedChanges.Insert(_index, TransactionStackEntry.Instance);
            CutAfterCurrentIndex();
        } 

        private void CutAfterCurrentIndex()
        {
            for (int i = _recordedChanges.Count - 1; i > _index; i--)
            {
                _recordedChanges.RemoveAt(i);
            }
            _index++;
        }

        /// <summary>
        /// True, of an undo operation is currently supported, otherwise false
        /// </summary>
        public bool CanUndo => _index > 0;

        /// <summary>
        /// True, if a redo operation is currently supported, otherwise false
        /// </summary>
        public bool CanRedo => _index < _recordedChanges.Count;

        /// <summary>
        /// Performs an undo operation
        /// </summary>
        /// <param name="modelSession">the model session for which to perform the undo operation</param>
        /// <exception cref="InvalidOperationException">Thrown if undo is currently not allowed</exception>
        public void Undo(IModelSession modelSession)
        {
            _index--;

            if (_index < 0) throw new InvalidOperationException("Cannot undo");

            var toRevert = _recordedChanges[_index];
            toRevert.Revert(modelSession);
        }

        /// <summary>
        /// Performs a redo operation
        /// </summary>
        /// <param name="modelSession">the model session for which to perform the redo operation</param>
        /// <exception cref="InvalidOperationException">Thrown if redo is currently not allowed</exception>
        public void Redo(IModelSession modelSession)
        {
            if (_index >= _recordedChanges.Count) throw new InvalidOperationException("Cannot redo");

            var toReapply = _recordedChanges[_index];
            toReapply.Apply(modelSession);

            _index++;
        }

#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
        private abstract class UndoStackEntry
#pragma warning restore S1694 // An abstract class should have both abstract and concrete methods
        {
            public abstract void Apply(IModelSession modelSession);

            public abstract void Revert(IModelSession modelSession);
        }

        private sealed class ChangeSetEntry : UndoStackEntry
        {
            private readonly ModelChangeSet _changeSet;

            public ChangeSetEntry(ModelChangeSet changeSet)
            {
                _changeSet = changeSet;
            }

            public override void Apply(IModelSession modelSession)
            {
                _changeSet.Apply();
            }

            public override void Revert(IModelSession modelSession)
            {
                _changeSet.Invert();
            }
        }

        private sealed class TransactionStackEntry : UndoStackEntry
        {
            public static readonly TransactionStackEntry Instance = new TransactionStackEntry();

            public override void Apply(IModelSession modelSession)
            {
                modelSession.Redo();
            }

            public override void Revert(IModelSession modelSession)
            {
                modelSession.Undo();
            }
        }
    }
}
