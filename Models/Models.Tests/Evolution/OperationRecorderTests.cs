using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Meta;
using NMF.Models.Changes;
using NMF.Models.Tests.Architecture;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models.Tests.Evolution
{
    [TestClass]
    public class OperationRecorderTests
    {
        private ClassModel model;
        private Architecture.Class a;
        private Architecture.Class b;

        [TestInitialize]
        public void SetupModel()
        {
            model = new ClassModel();
            a = new Architecture.Class { Name = "A" };
            b = new Architecture.Class { Name = "B" };
            model.Classes.Add(a);
            model.Classes.Add(b);

            b.BaseType = a;

            a.Encapsulates.Add(new Architecture.Attribute
            {
                Name = "a"
            });
            b.Encapsulates.Add(new Method
            {
                Name = "b"
            });

            var pullUp = Architecture.Class.ClassInstance.LookupOperation("pullUpFeature");

            OperationBroker.Instance.RegisterDelegate(pullUp,
                new Action<Architecture.IClass, string>((c, name) =>
                {
                    if (c.BaseType == null) return;
                    var op = c.Encapsulates.FirstOrDefault(f => f.Name == name);
                    if (op != null)
                    {
                        c.BaseType.Encapsulates.Add(op);
                    }
                }));
        }

        [TestMethod]
        public void TestPullUpCorrectlyRecorded()
        {
            var recorder = new ModelChangeRecorder();
            recorder.Start(model);
            b.PullUpFeature("b");
            recorder.Stop();
            var changes = recorder.GetModelChanges();

            Assert.AreEqual(1, changes.Changes.Count);
            Assert.IsInstanceOfType(changes.Changes[0], typeof(ChangeTransaction));
            var t = changes.Changes[0] as ChangeTransaction;
            Assert.IsInstanceOfType(t.SourceChange, typeof(OperationCall));
            var call = t.SourceChange as OperationCall;

            Assert.AreEqual("b", (call.Arguments[0] as ValueArgument).Value);
        }

        [TestMethod]
        public void TestPullUpInverted()
        {
            var method = b.Encapsulates[0];
            var recorder = new ModelChangeRecorder();
            recorder.Start(model);
            b.PullUpFeature("b");
            recorder.Stop();
            var changes = recorder.GetModelChanges();

            Assert.AreEqual(0, b.Encapsulates.Count);

            changes.Invert();

            Assert.AreEqual(1, b.Encapsulates.Count);
            Assert.AreEqual(method, b.Encapsulates[0]);
        }

        [TestMethod]
        public void OperationDoesNotDoAnything()
        {
            var recorder = new ModelChangeRecorder();
            recorder.Start(model);
            b.PullUpFeature("no such feature");
            recorder.Stop();
            var changes = recorder.GetModelChanges();

            Assert.AreEqual(1, changes.Changes.Count);
            Assert.IsInstanceOfType(changes.Changes[0], typeof(OperationCall));
        }
    }
}
