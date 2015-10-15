using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Synchronizations.Example;
using Fsm = NMF.Synchronizations.Example.FSM;
using Pn = NMF.Synchronizations.Example.PN;
using System.Diagnostics;
using NMF.Transformations;

namespace NMF.Synchronizations.Tests.BigPictureTests
{
    [TestClass]
    public class Fsm2PnTest
    {
        private FSM2PN fsm2pn = new FSM2PN();

        private Fsm.FiniteStateMachine fsm = new Fsm.FiniteStateMachine();
        private Pn.PetriNet pn = new Pn.PetriNet();

        private void FillStateMachine()
        {
            var s1 = new Fsm.State() { Name = "s1", IsStartState = true };
            var s2 = new Fsm.State() { Name = "s2" };
            var s3 = new Fsm.State() { Name = "s3", IsEndState = true };

            fsm.States.Add(s1);
            fsm.States.Add(s2);
            fsm.States.Add(s3);

            var t1 = new Fsm.Transition()
            {
                StartState = s1,
                EndState = s2,
                Input = "a"
            };

            var t2 = new Fsm.Transition()
            {
                StartState = s2,
                EndState = s3,
                Input = "a"
            };

            var t3 = new Fsm.Transition()
            {
                StartState = s2,
                EndState = s1,
                Input = "b"
            };

            var t4 = new Fsm.Transition()
            {
                StartState = s1,
                EndState = s1,
                Input = "b"
            };

            fsm.Transitions.Add(t1);
            fsm.Transitions.Add(t2);
            fsm.Transitions.Add(t3);
            fsm.Transitions.Add(t4);
        }

        private void FillPetriNet()
        {
            var p1 = new Pn.Place()
            {
                Id = "s1"
            };

            var p2 = new Pn.Place()
            {
                Id = "s2"
            };

            var p3 = new Pn.Place()
            {
                Id = "s4"
            };

            var t1 = new Pn.Transition()
            {
                Input = "a"
            };

            var t2 = new Pn.Transition()
            {
                Input = "c"
            };

            t1.From.Add(p1);
            t1.To.Add(p2);

            t2.From.Add(p1);
            t2.To.Add(p3);
            t2.To.Add(p2);

            pn.Places.Add(p1);
            pn.Places.Add(p2);
            pn.Places.Add(p3);
            pn.Transitions.Add(t1);
            pn.Transitions.Add(t2);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRight_NoUpdate_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRight, ChangePropagationMode.None, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRight_NoUpdate_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRight, ChangePropagationMode.None, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRight_OneWay_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRight, ChangePropagationMode.OneWay, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRight_OneWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRight, ChangePropagationMode.OneWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRight_TwoWay_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRight, ChangePropagationMode.TwoWay, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRight_TwoWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRight, ChangePropagationMode.TwoWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRightForced_NoUpdate_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRightForced, ChangePropagationMode.None, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRightForced_NoUpdate_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRightForced, ChangePropagationMode.None, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRightForced_OneWay_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRightForced, ChangePropagationMode.OneWay, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRightForced_OneWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRightForced, ChangePropagationMode.OneWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRightForced_TwoWay_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRightForced, ChangePropagationMode.TwoWay, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftToRightForced_TwoWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftToRightForced, ChangePropagationMode.TwoWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftWins_NoUpdate_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftWins, ChangePropagationMode.None, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftWins_NoUpdate_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftWins, ChangePropagationMode.None, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftWins_OneWay_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftWins, ChangePropagationMode.OneWay, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftWins_OneWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftWins, ChangePropagationMode.OneWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_LeftWins_TwoWay_InitialFsm_EmptyPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftWins, ChangePropagationMode.TwoWay, true, false);
        }

        [TestMethod]
        public void Fsm2Pn_LeftWins_TwoWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.LeftWins, ChangePropagationMode.TwoWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeft_NoUpdate_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeft, ChangePropagationMode.None, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeft_NoUpdate_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeft, ChangePropagationMode.None, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeft_OneWay_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeft, ChangePropagationMode.OneWay, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeft_OneWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeft, ChangePropagationMode.OneWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeft_TwoWay_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeft, ChangePropagationMode.TwoWay, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeft_TwoWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeft, ChangePropagationMode.TwoWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeftForced_NoUpdate_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeftForced, ChangePropagationMode.None, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeftForced_NoUpdate_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeftForced, ChangePropagationMode.None, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeftForced_OneWay_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeftForced, ChangePropagationMode.OneWay, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeftForced_OneWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeftForced, ChangePropagationMode.OneWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeftForced_TwoWay_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeftForced, ChangePropagationMode.TwoWay, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightToLeftForced_TwoWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightToLeftForced, ChangePropagationMode.TwoWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightWins_NoUpdate_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightWins, ChangePropagationMode.None, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightWins_NoUpdate_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightWins, ChangePropagationMode.None, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightWins_OneWay_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightWins, ChangePropagationMode.OneWay, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightWins_OneWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightWins, ChangePropagationMode.OneWay, true, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightWins_TwoWay_EmptyFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightWins, ChangePropagationMode.TwoWay, false, true);
        }

        [TestMethod]
        public void Fsm2Pn_RightWins_TwoWay_InitialFsm_InitialPn()
        {
            TestFsm2Pn(SynchronizationDirection.RightWins, ChangePropagationMode.TwoWay, true, true);
        }

        private void TestFsm2Pn(SynchronizationDirection direction, ChangePropagationMode changePropagartion, bool initializeFsm, bool initializePn)
        {
            Assert.IsTrue(initializeFsm | initializePn);

            var fsm = this.fsm;
            var pn = this.pn;

            if (initializeFsm) FillStateMachine();
            if (initializePn) FillPetriNet();

            fsm2pn.Initialize();

            var context = fsm2pn.Synchronize(fsm2pn.SynchronizationRule<FSM2PN.AutomataToNet>(), ref fsm, ref pn, direction, changePropagartion);
            var isLeftToRight = direction == SynchronizationDirection.LeftToRight || direction == SynchronizationDirection.LeftToRightForced || direction == SynchronizationDirection.LeftWins;
            var isForced = direction == SynchronizationDirection.LeftToRightForced || direction == SynchronizationDirection.RightToLeftForced;
            var isJoined = direction == SynchronizationDirection.LeftWins || direction == SynchronizationDirection.RightWins;

            Fsm.State s1;
            Pn.Place p1;

            if (initializeFsm && initializePn)
            {
                if (isForced)
                {
                    if (isLeftToRight)
                    {
                        s1 = AssertOriginalFsm(fsm, context);
                    }
                    else
                    {
                        s1 = AssertPetriNetLikeFsm(fsm, context);
                    }
                }
                else if (isJoined || !isLeftToRight)
                {
                    s1 = AssertJoinedFsm(fsm, context);
                }
                else
                {
                    s1 = AssertOriginalFsm(fsm, context);
                }
            }
            else if (!initializeFsm)
            {
                if (!isLeftToRight || isJoined)
                {
                    s1 = AssertPetriNetLikeFsm(fsm, context);
                }
                else
                {
                    s1 = AssertEmptyFsm(fsm);
                }
            }
            else if (!initializePn)
            {
                if (isForced && !isLeftToRight)
                {
                    s1 = AssertEmptyFsm(fsm);
                }
                else
                {
                    s1 = AssertOriginalFsm(fsm, context);
                }
            }
            else
            {
                s1 = null;
                Assert.Fail();
            }

            if (initializeFsm && initializePn)
            {
                if (isForced)
                {
                    if (!isLeftToRight)
                    {
                        p1 = AssertOriginalPetriNet(pn, context, s1);
                    }
                    else
                    {
                        p1 = AssertFsmLikePetriNet(pn, context, s1);
                    }
                }
                else if (isJoined || isLeftToRight)
                {
                    p1 = AssertJoinedPetriNet(pn, context, s1);
                }
                else
                {
                    p1 = AssertOriginalPetriNet(pn, context, s1);
                }
            }
            else if (!initializePn)
            {
                if (isLeftToRight || isJoined)
                {
                    p1 = AssertFsmLikePetriNet(pn, context, s1);
                }
                else
                {
                    p1 = AssertEmptyPetriNet(pn);
                }
            }
            else if (!initializeFsm)
            {
                if (isForced && isLeftToRight)
                {
                    p1 = AssertEmptyPetriNet(pn);
                }
                else
                {
                    p1 = AssertOriginalPetriNet(pn, context, s1);
                }
            }
            else
            {
                p1 = null;
                Assert.Fail();
            }

            if (changePropagartion == ChangePropagationMode.TwoWay ||
                (changePropagartion == ChangePropagationMode.OneWay && isLeftToRight))
            {
                AssertOneWayUpdatesFsmToPetriNet(s1, p1);
            }

            if (changePropagartion == ChangePropagationMode.TwoWay ||
                (changePropagartion == ChangePropagationMode.OneWay && !isLeftToRight))
            {
                AssertOneWayUpdatesPetriNetToFsm(p1, s1);
            }
        }

        private void AssertOneWayUpdatesFsmToPetriNet(Fsm.State s1, Pn.Place s1Place)
        {
            if (s1 == null) return;

            s1.Name = "foo1";

            Assert.AreEqual("foo1", s1Place.Id);

            var oldTransitions = pn.Transitions.Count;

            s1.IsEndState = true;

            Assert.AreEqual(oldTransitions + 1, pn.Transitions.Count);
            Assert.IsTrue(s1Place.Outgoing.Any(t => t.To.Count == 0));

            s1.IsEndState = false;

            Assert.AreEqual(oldTransitions, pn.Transitions.Count);
            Assert.IsFalse(s1Place.Outgoing.Any(t => t.To.Count == 0));
        }

        private void AssertOneWayUpdatesPetriNetToFsm(Pn.Place p1, Fsm.State p1State)
        {
            if (p1 == null) return;

            p1.Id = "foo2";

            Assert.AreEqual("foo2", p1State.Name);
            Assert.IsFalse(p1State.IsEndState);

            var t = new Pn.Transition();
            t.From.Add(p1);
            pn.Transitions.Add(t);

            Assert.IsTrue(p1State.IsEndState);
        }

        private Pn.Place AssertEmptyPetriNet(Pn.PetriNet pn)
        {
            Assert.AreSame(pn, this.pn);
            Assert.AreEqual(0, pn.Places.Count);
            Assert.AreEqual(0, pn.Transitions.Count);
            return null;
        }

        private Pn.Place AssertJoinedPetriNet(Pn.PetriNet pn, ISynchronizationContext context, Fsm.State s1)
        {
            Assert.AreSame(pn, this.pn);
            Assert.AreEqual(4, pn.Places.Count);
            Assert.AreEqual(6, pn.Transitions.Count);

            var s1Place = context.Trace.ResolveIn(fsm2pn.SynchronizationRule<FSM2PN.StateToPlace>().LeftToRight, s1);

            Assert.IsNotNull(s1);
            Assert.IsNotNull(s1Place);

            Assert.AreEqual(3, s1Place.Outgoing.Count);
            Assert.AreEqual(2, s1Place.Incoming.Count);

            Assert.AreEqual("s1", s1Place.Id);

            return s1Place;
        }

        private Pn.Place AssertFsmLikePetriNet(Pn.PetriNet pn, ISynchronizationContext context, Fsm.State s1)
        {
            Assert.AreSame(pn, this.pn);
            Assert.AreEqual(3, pn.Places.Count);
            Assert.AreEqual(5, pn.Transitions.Count);

            var s1Place = context.Trace.ResolveIn(fsm2pn.SynchronizationRule<FSM2PN.StateToPlace>().LeftToRight, s1);

            Assert.IsNotNull(s1Place);

            Assert.AreEqual(2, s1Place.Outgoing.Count);
            Assert.AreEqual(2, s1Place.Incoming.Count);

            Assert.AreEqual("s1", s1Place.Id);

            return s1Place;
        }

        private Pn.Place AssertOriginalPetriNet(Pn.PetriNet pn, ISynchronizationContext context, Fsm.State s1)
        {
            Assert.AreSame(pn, this.pn);
            Assert.AreEqual(3, pn.Places.Count);
            Assert.AreEqual(2, pn.Transitions.Count);

            var place = pn.Places.FirstOrDefault(p => p.Id == "s1");

            Assert.IsNotNull(place);

            Assert.AreEqual(2, place.Outgoing.Count);
            Assert.AreEqual(0, place.Incoming.Count);

            if (s1 != null)
            {
                Assert.AreSame(place, context.Trace.ResolveIn(fsm2pn.SynchronizationRule<FSM2PN.StateToPlace>().LeftToRight, s1));
            }
            return place;
        }

        private Fsm.State AssertEmptyFsm(Fsm.FiniteStateMachine fsm)
        {
            Assert.AreSame(fsm, this.fsm);
            Assert.AreEqual(0, fsm.States.Count);
            Assert.AreEqual(0, fsm.Transitions.Count);
            return null;
        }

        private Fsm.State AssertOriginalFsm(Fsm.FiniteStateMachine fsm, ISynchronizationContext context)
        {
            Assert.AreSame(fsm, this.fsm);
            Assert.AreEqual(3, fsm.States.Count);
            Assert.AreEqual(4, fsm.Transitions.Count);
            
            var s1 = fsm.States.Where(s => s.IsStartState).FirstOrDefault();
            var s2 = fsm.States.Where(s => !s.IsStartState && !s.IsEndState).FirstOrDefault();
            var s3 = fsm.States.Where(s => s.IsEndState);

            Assert.IsNotNull(s1);
            Assert.IsNotNull(s2);
            Assert.IsNotNull(s3);
            
            var t1 = s1.Transitions.FirstOrDefault(t => t.Input == "a");
            var t2 = s2.Transitions.FirstOrDefault(t => t.Input == "a");
            var t3 = s2.Transitions.FirstOrDefault(t => t.Input == "b");
            var t4 = s1.Transitions.FirstOrDefault(t => t.Input == "b");

            Assert.IsNotNull(t1);
            Assert.IsNotNull(t2);
            Assert.IsNotNull(t3);
            Assert.IsNotNull(t4);

            return s1;
        }

        private Fsm.State AssertPetriNetLikeFsm(Fsm.FiniteStateMachine fsm, ISynchronizationContext context)
        {
            Assert.AreSame(fsm, this.fsm);
            Assert.AreEqual(3, fsm.States.Count);
            Assert.AreEqual(2, fsm.Transitions.Count);

            var s1 = fsm.States.FirstOrDefault(s => s.Name == "s1");
            var s2 = fsm.States.FirstOrDefault(s => s.Name == "s2");
            var s4 = fsm.States.FirstOrDefault(s => s.Name == "s4");

            Assert.IsNotNull(s1);
            Assert.IsNotNull(s2);
            Assert.IsNotNull(s4);

            return s1;
        }

        private Fsm.State AssertJoinedFsm(Fsm.FiniteStateMachine fsm, ISynchronizationContext context)
        {
            Assert.AreSame(fsm, this.fsm);
            Assert.AreEqual(4, fsm.States.Count);
            Assert.AreEqual(5, fsm.Transitions.Count);

            var s1 = fsm.States.FirstOrDefault(s => s.Name == "s1");
            var s2 = fsm.States.FirstOrDefault(s => s.Name == "s2");
            var s3 = fsm.States.FirstOrDefault(s => s.Name == "s3");
            var s4 = fsm.States.FirstOrDefault(s => s.Name == "s4");

            Assert.IsNotNull(s1);
            Assert.IsNotNull(s2);
            Assert.IsNotNull(s3);
            Assert.IsNotNull(s4);

            return s1;
        }
    }
}
