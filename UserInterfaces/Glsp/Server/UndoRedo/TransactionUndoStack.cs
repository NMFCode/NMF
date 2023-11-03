using NMF.Models.Changes;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Server.UndoRedo
{
    public class TransactionUndoStack
    {
        private readonly List<ModelChangeSet> _recordedChanges = new List<ModelChangeSet>();
        private int _index = 0;

        public void Notify(ModelChangeSet changeSet)
        {
            _recordedChanges.Insert(_index, changeSet);
            _index++;
        }

        public bool CanUndo => _index > 0;

        public bool CanRedo => _index < _recordedChanges.Count;

        public void Undo()
        {
            _index--;

            if (_index < 0) throw new InvalidOperationException("Cannot undo");

            var toRevert = _recordedChanges[_index];
            toRevert.Invert();
        }

        public void Redo()
        {
            if (_index >= _recordedChanges.Count) throw new InvalidOperationException("Cannot redo");

            var toReapply = _recordedChanges[_index];
            toReapply.Apply();

            _index++;
        }
    }
}
