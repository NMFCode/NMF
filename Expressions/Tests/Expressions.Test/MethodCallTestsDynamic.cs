  
using System;
using System.Collections.Generic;
using NMF.Expressions.Linq;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{ 
    [TestClass]
    public class MethodCallTestsDynamic
    {        
        #region New tests with 1 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter1()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 1; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1", test.Value);
            Assert.IsFalse(update);

            dummy[0].Item = 2;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter1()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 1; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[0].Item = "2";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter1()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 1; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1", e.OldValue);
                Assert.AreEqual("2", e.NewValue);
            };
            Assert.AreEqual("1", test.Value);
            Assert.IsFalse(update);

            dummy[0].Item = 2;

            Assert.IsTrue(update);
            Assert.AreEqual("2", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter1()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 1; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1", String.Join(", ", e.OldValue));
                Assert.AreEqual("2", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[0].Item = "2";

            Assert.IsTrue(update);
            Assert.AreEqual("2", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 2 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter2()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 2; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2", test.Value);
            Assert.IsFalse(update);

            dummy[1].Item = 3;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter2()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 2; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[1].Item = "3";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter2()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 2; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2", e.OldValue);
                Assert.AreEqual("1, 3", e.NewValue);
            };
            Assert.AreEqual("1, 2", test.Value);
            Assert.IsFalse(update);

            dummy[1].Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 3", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter2()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 2; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 3", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[1].Item = "3";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 3", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 3 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter3()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 3; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3", test.Value);
            Assert.IsFalse(update);

            dummy[2].Item = 4;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter3()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 3; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[2].Item = "4";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter3()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 3; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3", e.OldValue);
                Assert.AreEqual("1, 2, 4", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3", test.Value);
            Assert.IsFalse(update);

            dummy[2].Item = 4;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 4", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter3()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 3; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 4", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[2].Item = "4";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 4", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 4 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter4()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 4; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4", test.Value);
            Assert.IsFalse(update);

            dummy[3].Item = 5;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter4()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 4; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[3].Item = "5";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter4()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 4; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4", e.OldValue);
                Assert.AreEqual("1, 2, 3, 5", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4", test.Value);
            Assert.IsFalse(update);

            dummy[3].Item = 5;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 5", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter4()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 4; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 5", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[3].Item = "5";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 5", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 5 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter5()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 5; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5", test.Value);
            Assert.IsFalse(update);

            dummy[4].Item = 6;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter5()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 5; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[4].Item = "6";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter5()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 5; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 6", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5", test.Value);
            Assert.IsFalse(update);

            dummy[4].Item = 6;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 6", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter5()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 5; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 6", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[4].Item = "6";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 6", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 6 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter6()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 6; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6", test.Value);
            Assert.IsFalse(update);

            dummy[5].Item = 7;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter6()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 6; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[5].Item = "7";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter6()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 6; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 7", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6", test.Value);
            Assert.IsFalse(update);

            dummy[5].Item = 7;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 7", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter6()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 6; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 7", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[5].Item = "7";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 7", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 7 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter7()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 7; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7", test.Value);
            Assert.IsFalse(update);

            dummy[6].Item = 8;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter7()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 7; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[6].Item = "8";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter7()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 7; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 8", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7", test.Value);
            Assert.IsFalse(update);

            dummy[6].Item = 8;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 8", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter7()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 7; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 8", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[6].Item = "8";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 8", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 8 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter8()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 8; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8", test.Value);
            Assert.IsFalse(update);

            dummy[7].Item = 9;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter8()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 8; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[7].Item = "9";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter8()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 8; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 9", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8", test.Value);
            Assert.IsFalse(update);

            dummy[7].Item = 9;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 9", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter8()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 8; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 9", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[7].Item = "9";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 9", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 9 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter9()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 9; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", test.Value);
            Assert.IsFalse(update);

            dummy[8].Item = 10;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter9()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 9; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[8].Item = "10";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter9()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 9; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 10", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", test.Value);
            Assert.IsFalse(update);

            dummy[8].Item = 10;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 10", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter9()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 9; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 10", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[8].Item = "10";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 10", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 10 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter10()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 10; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", test.Value);
            Assert.IsFalse(update);

            dummy[9].Item = 11;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter10()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 10; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[9].Item = "11";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter10()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 10; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 11", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", test.Value);
            Assert.IsFalse(update);

            dummy[9].Item = 11;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 11", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter10()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 10; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 11", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[9].Item = "11";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 11", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 11 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter11()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 11; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11", test.Value);
            Assert.IsFalse(update);

            dummy[10].Item = 12;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter11()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 11; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[10].Item = "12";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter11()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 11; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11", test.Value);
            Assert.IsFalse(update);

            dummy[10].Item = 12;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter11()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 11; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[10].Item = "12";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 12 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter12()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 12; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12", test.Value);
            Assert.IsFalse(update);

            dummy[11].Item = 13;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter12()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 12; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[11].Item = "13";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter12()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 12; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12", test.Value);
            Assert.IsFalse(update);

            dummy[11].Item = 13;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter12()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 12; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[11].Item = "13";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 13 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter13()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 13; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13", test.Value);
            Assert.IsFalse(update);

            dummy[12].Item = 14;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter13()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 13; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[12].Item = "14";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter13()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 13; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13", test.Value);
            Assert.IsFalse(update);

            dummy[12].Item = 14;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter13()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 13; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[12].Item = "14";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 14 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter14()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 14; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14", test.Value);
            Assert.IsFalse(update);

            dummy[13].Item = 15;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter14()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 14; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[13].Item = "15";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter14()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 14; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14", test.Value);
            Assert.IsFalse(update);

            dummy[13].Item = 15;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter14()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 14; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[13].Item = "15";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15", String.Join(", ", test.Value));
        }






        #endregion

        #region New tests with 15 parameters
 
        [TestMethod]
        public void MethodCall_NoObservableTarget_Parameter15()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<int>>();
            
            for(int j = 1; j<= 15; j++)
            {
                dummy.Add( new Dummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15", test.Value);
            Assert.IsFalse(update);

            dummy[14].Item = 16;

            Assert.IsFalse(update);
        }


        [TestMethod]
        public void MethodCall_NoObservableArgument_Parameter15()
        {
            var update = false;
            var dummy = new NotifyCollection<Dummy<string>>();
            
            for(int j = 1; j<= 15; j++)
            {
                dummy.Add( new Dummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) => update = true;
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[14].Item = "16";

            Assert.IsFalse(update);
        }


 
        [TestMethod]
        public void MethodCall_ObservableTarget_Parameter15()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<int>>();
            
            for(int j = 1; j<= 15; j++)
            {
                dummy.Add( new ObservableDummy<int>() { Item = j } );
            }

            var test = Observable.Expression<string>(() => String.Join(", ", dummy.Select(x => x.Item.ToString()) ) );
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15", e.OldValue);
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16", e.NewValue);
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15", test.Value);
            Assert.IsFalse(update);

            dummy[14].Item = 16;

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16", test.Value);
        }


        [TestMethod]
        public void MethodCall_ObservableArgument_Parameter15()
        {
            var update = false;
            var dummy = new NotifyCollection<ObservableDummy<string>>();
            
            for(int j = 1; j<= 15; j++)
            {
                dummy.Add( new ObservableDummy<string>() { Item = j.ToString() } );
            }

            var test = Observable.Expression<int[]>(() => dummy.Select(x => Convert.ToInt32(x.Item)).ToArray());
            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15", String.Join(", ", e.OldValue));
                Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16", String.Join(", ", e.NewValue));
            };
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15", String.Join(", ", test.Value));
            Assert.IsFalse(update);

            dummy[14].Item = "16";

            Assert.IsTrue(update);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16", String.Join(", ", test.Value));
        }






        #endregion

    }
}
