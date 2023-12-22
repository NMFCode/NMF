using NMF.Models.Changes;
using System;
using System.Collections.Generic;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes an undo/redo stack based on model changes
    /// </summary>
    public class TransactionUndoStack
    {
        private readonly List<ModelChangeSet> _recordedChanges = new List<ModelChangeSet>();
        private int _index = 0;

        /// <summary>
        /// Notifies that the given change was performed
        /// </summary>
        /// <param name="changeSet"></param>
        public void Notify(ModelChangeSet changeSet)
        {
            _recordedChanges.Insert(_index, changeSet);
            _index++;
        }

        /// <summary>
        /// True, if an undo operation can be performed, otherwise False
        /// </summary>
        public bool CanUndo => _index > 0;

        /// <summary>
        /// True, if a redo operation can be performed, otherwise False
        /// </summary>
        public bool CanRedo => _index < _recordedChanges.Count;

        /// <summary>
        /// Performs an undo operation
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no undo operation can be performed</exception>
        public void Undo()
        {
            _index--;

            if (_index < 0) throw new InvalidOperationException("Cannot undo");

            var toRevert = _recordedChanges[_index];
            toRevert.Invert();
        }

        /// <summary>
        /// Performs a redo-operation
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no redo operation can be performed</exception>
        public void Redo()
        {
            if (_index >= _recordedChanges.Count) throw new InvalidOperationException("Cannot redo");

            var toReapply = _recordedChanges[_index];
            toReapply.Apply();

            _index++;
        }
    }
}
