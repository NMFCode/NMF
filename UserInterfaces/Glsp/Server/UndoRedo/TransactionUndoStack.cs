using NMF.Models.Changes;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Server.UndoRedo
{
    public class TransactionUndoStack
    {
        private readonly List<ModelChangeSet> _recordedChanges = new List<ModelChangeSet>();
        private readonly List<ModelChangeSet> _recordedLayoutChanges = new List<ModelChangeSet>();
        private int _index = 0;

        public void Notify(ModelChangeSet changeSet, ModelChangeSet layoutChangeSet)
        {
            _recordedChanges.Insert(_index, changeSet.Changes.Count > 0 ? changeSet : null);
            _recordedLayoutChanges.Insert(_index, layoutChangeSet);
            _index++;
        }

        public bool CanUndo => _index > 0;

        public bool CanRedo => _index < _recordedChanges.Count;

        public void Undo()
        {
            _index--;

            if (_index < 0) throw new InvalidOperationException("Cannot undo");

            var toRevert = _recordedChanges[_index];
            if ( toRevert == null)
            {
                toRevert = _recordedLayoutChanges[_index];
            }
            toRevert.Invert();
        }

        public void Redo()
        {
            if (_index >= _recordedChanges.Count) throw new InvalidOperationException("Cannot redo");

            var toReapply = _recordedChanges[_index];
            if (toReapply == null)
            {
                toReapply = _recordedLayoutChanges[_index];
            }
            toReapply.Apply();

            _index++;
        }
    }
}
