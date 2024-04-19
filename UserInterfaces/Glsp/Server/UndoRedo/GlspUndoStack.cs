using NMF.Models.Changes;
using NMF.Models.Services;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Server.UndoRedo
{
    public class GlspUndoStack
    {
        private readonly List<UndoStackEntry> _recordedChanges = new List<UndoStackEntry>();
        private int _index = 0;

        public void Notify(ModelChangeSet changeSet)
        {
            _recordedChanges.Insert(_index, new ChangeSetEntry(changeSet));
            CutAfterCurrentIndex();
        }

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

        public bool CanUndo => _index > 0;

        public bool CanRedo => _index < _recordedChanges.Count;

        public void Undo(IModelSession modelSession)
        {
            _index--;

            if (_index < 0) throw new InvalidOperationException("Cannot undo");

            var toRevert = _recordedChanges[_index];
            toRevert.Revert(modelSession);
        }

        public void Redo(IModelSession modelSession)
        {
            if (_index >= _recordedChanges.Count) throw new InvalidOperationException("Cannot redo");

            var toReapply = _recordedChanges[_index];
            toReapply.Apply(modelSession);

            _index++;
        }

        private abstract class UndoStackEntry
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
            public static TransactionStackEntry Instance = new TransactionStackEntry();

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
