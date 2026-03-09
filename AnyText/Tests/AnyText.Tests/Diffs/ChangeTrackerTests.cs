using NMF.AnyText;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyText.Tests.Diffs
{
    [TestFixture]
    public class ChangeTrackerTests
    {
        private ChangeTracker _tracker = new ChangeTracker();
        private string[] _input;

        [SetUp]
        public void Setup()
        {
            _tracker.Reset();
            _input = ["abcdefgh", "abcdefgh"];
        }

        [Test]
        public void InsertionOnEmpty_KeepsEdit()
        {
            var edit = new TextEdit(new ParsePosition(1, 8), new ParsePosition(1, 8), ["a"]);
            EditAndApply(edit);
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([edit]));
        }

        [Test]
        public void InsertionAfterExisting_KeepsEdit()
        {
            var firstEdit = new TextEdit(new ParsePosition(0, 5), new ParsePosition(0, 6), ["a"]);
            var edit = new TextEdit(new ParsePosition(1, 8), new ParsePosition(1, 8), ["a"]);
            EditAndApply(firstEdit);
            EditAndApply(edit);
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([firstEdit, edit]));
        }

        [Test]
        public void InsertionBeforeExisting_KeepsEdit()
        {
            var firstEdit = new TextEdit(new ParsePosition(0, 5), new ParsePosition(0, 6), ["a"]);
            var edit = new TextEdit(new ParsePosition(1, 8), new ParsePosition(1, 8), ["a"]);
            EditAndApply(edit);
            EditAndApply(firstEdit);
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([firstEdit, edit]));
        }

        [Test]
        public void DeleteInsertion_Empty()
        {
            var firstEdit = new TextEdit(new ParsePosition(1, 8), new ParsePosition(1, 8), ["a"]);
            EditAndApply(firstEdit);
            EditAndApply(new TextEdit(new ParsePosition(1, 8), new ParsePosition(1, 9), [""]));
            Assert.That(_tracker.CurrentEdits, Is.Empty);
        }

        [Test]
        public void DeletionOnEmpty_KeepsEdit()
        {
            var edit = new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 1), [""]);
            EditAndApply(edit);
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([edit]));
        }

        [Test]
        public void InsertDeletion_Empty()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 2), ["c"]));
            Assert.That(_tracker.CurrentEdits, Is.Empty);
        }

        [Test]
        public void InsertDeletionChanged_ChangesEdit()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 2), ["a"]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1,2), new ParsePosition(1,3), ["a"])
                ]));
        }

        [Test]
        public void InsertObsoletesDeletes_ChangesEdit()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            EditAndApply(new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 4), ["abcdefg"]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 5), ["abcdefg"])
                ]));
        }

        [Test]
        public void DeleteAfterInsert_ChangesEdit()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 4), ["abcdefg"]));
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 4), ["abdefg"])
                ]));
        }

        [Test]
        public void InsertAfterInsert_ChangesEdit()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 4), ["abcdefg"]));
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 2), ["h"]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 4), ["abhcdefg"])
                ]));
        }

        [Test]
        public void DeletionObsoletesDeletions_ChangesEdit()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            EditAndApply(new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 4), [""]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 5), [""])
                ]));
        }

        [Test]
        public void InsertAfterInsert_CombinesEdits()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 2), ["a"]));
            EditAndApply(new TextEdit(new ParsePosition(1, 3), new ParsePosition(1, 3), ["b"]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 2), ["ab"])
                ]));
        }

        [Test]
        public void DeleteAfterDelete_CombinesEdits()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 3), new ParsePosition(1, 4), [""]));
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 4), [""])
                ]));
        }

        [Test]
        public void DeleteAfterDelete2_CombinesEdits()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), [""]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 4), [""])
                ]));
        }

        [Test]
        public void OverlappingInsertions_CombinesEdits()
        {
            EditAndApply(new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 2), ["abcdefg"]));
            EditAndApply(new TextEdit(new ParsePosition(1, 4), new ParsePosition(1, 14), ["inserted"]));
            Assert.That(_tracker.CurrentEdits, Is.EquivalentTo([
                new TextEdit(new ParsePosition(1,2), new ParsePosition(1,6), ["abinserted"])
                ]));
        }

        private void EditAndApply(TextEdit edit)
        {
            _tracker.AddEdit(edit, _input);
            _input = edit.Apply(_input);
        }
    }
}
