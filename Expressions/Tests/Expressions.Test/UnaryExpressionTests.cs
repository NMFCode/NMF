using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class UnaryExpressionTests
    {
        [TestMethod]
        public void Cast_String_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<object>() { Item = "23" };

            var test = Observable.Expression<string>(() => (string)dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", test.Value);
            Assert.IsFalse(update);

            dummy.Item = "42";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Cast_String_Observable_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<object>() { Item = "23" };

            var test = Observable.Expression<string>(() => (string)dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("23", e.OldValue);
                Assert.AreEqual("42", e.NewValue);
            };

            Assert.AreEqual("23", test.Value);
            Assert.IsFalse(update);

            dummy.Item = "42";

            Assert.IsTrue(update);
            Assert.AreEqual("42", test.Value);
        }

        [TestMethod]
        public void Cast_Int_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<object>() { Item = 23 };

            var test = Observable.Expression<int>(() => (int)dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Cast_Int_Observable_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<object>() { Item = 23 };

            var test = Observable.Expression<int>(() => (int)dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, e.OldValue);
                Assert.AreEqual(42, e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryMinus_Int_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>() { Item = 23 };

            var test = Observable.Expression<int>(() => -dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryMinus_Int_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = Observable.Expression<int>(() => -dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-23, (int)e.OldValue);
                Assert.AreEqual(42, (int)e.NewValue);
            };

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryMinus_Long_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<long>() { Item = 23 };

            var test = Observable.Expression<long>(() => -dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryMinus_Long_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<long>() { Item = 23 };

            var test = Observable.Expression<long>(() => -dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-23, (long)e.OldValue);
                Assert.AreEqual(42, (long)e.NewValue);
            };

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryMinus_Double_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<double>() { Item = 23 };

            var test = Observable.Expression<double>(() => -dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryMinus_Double_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<double>() { Item = 23 };

            var test = Observable.Expression<double>(() => -dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-23, (double)e.OldValue);
                Assert.AreEqual(42, (double)e.NewValue);
            };

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryMinus_Float_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<float>() { Item = 23 };

            var test = Observable.Expression<float>(() => -dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryMinus_Float_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<float>() { Item = 23 };

            var test = Observable.Expression<float>(() => -dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-23, (float)e.OldValue);
                Assert.AreEqual(42, (float)e.NewValue);
            };

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryMinus_Decimal_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<decimal>() { Item = 23 };

            var test = Observable.Expression<decimal>(() => -dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryMinus_Decimal_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<decimal>() { Item = 23 };

            var test = Observable.Expression<decimal>(() => -dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-23, (decimal)e.OldValue);
                Assert.AreEqual(42, (decimal)e.NewValue);
            };

            Assert.AreEqual(-23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = -42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }


        [TestMethod]
        public void UnaryMinusChecked_Int_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<int>() { Item = 23 };

                var test = Observable.Expression<int>(() => -dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsFalse(update); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Int_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<int>() { Item = 23 };

                var test = Observable.Expression<int>(() => -dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(-23, (int)e.OldValue);
                    Assert.AreEqual(42, (int)e.NewValue);
                };

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Long_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<long>() { Item = 23 };

                var test = Observable.Expression<long>(() => -dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsFalse(update); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Long_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<long>() { Item = 23 };

                var test = Observable.Expression<long>(() => -dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(-23, (long)e.OldValue);
                    Assert.AreEqual(42, (long)e.NewValue);
                };

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Double_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<double>() { Item = 23 };

                var test = Observable.Expression<double>(() => -dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsFalse(update); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Double_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<double>() { Item = 23 };

                var test = Observable.Expression<double>(() => -dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(-23, (double)e.OldValue);
                    Assert.AreEqual(42, (double)e.NewValue);
                };

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Float_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<float>() { Item = 23 };

                var test = Observable.Expression<float>(() => -dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsFalse(update); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Float_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<float>() { Item = 23 };

                var test = Observable.Expression<float>(() => -dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(-23, (float)e.OldValue);
                    Assert.AreEqual(42, (float)e.NewValue);
                };

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Decimal_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<decimal>() { Item = 23 };

                var test = Observable.Expression<decimal>(() => -dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsFalse(update); 
            }
        }

        [TestMethod]
        public void UnaryMinusChecked_Decimal_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<decimal>() { Item = 23 };

                var test = Observable.Expression<decimal>(() => -dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(-23, (decimal)e.OldValue);
                    Assert.AreEqual(42, (decimal)e.NewValue);
                };

                Assert.AreEqual(-23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = -42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value); 
            }
        }

        [TestMethod]
        public void UnaryPlus_Int_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>() { Item = 23 };

            var test = Observable.Expression<int>(() => +dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryPlus_Int_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = Observable.Expression<int>(() => +dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, (int)e.OldValue);
                Assert.AreEqual(42, (int)e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryPlus_Long_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<long>() { Item = 23 };

            var test = Observable.Expression<long>(() => +dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryPlus_Long_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<long>() { Item = 23 };

            var test = Observable.Expression<long>(() => +dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, (long)e.OldValue);
                Assert.AreEqual(42, (long)e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryPlus_Double_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<double>() { Item = 23 };

            var test = Observable.Expression<double>(() => +dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryPlus_Double_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<double>() { Item = 23 };

            var test = Observable.Expression<double>(() => +dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, (double)e.OldValue);
                Assert.AreEqual(42, (double)e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryPlus_Float_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<float>() { Item = 23 };

            var test = Observable.Expression<float>(() => dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryPlus_Float_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<float>() { Item = 23 };

            var test = Observable.Expression<float>(() => +dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, (float)e.OldValue);
                Assert.AreEqual(42, (float)e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void UnaryPlus_Decimal_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<decimal>() { Item = 23 };

            var test = Observable.Expression<decimal>(() => +dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void UnaryPlus_Decimal_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<decimal>() { Item = 23 };

            var test = Observable.Expression<decimal>(() => +dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, (decimal)e.OldValue);
                Assert.AreEqual(42, (decimal)e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }


        [TestMethod]
        public void UnaryPlusChecked_Int_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<int>() { Item = 23 };

                var test = Observable.Expression<int>(() => +dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsFalse(update);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Int_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<int>() { Item = 23 };

                var test = Observable.Expression<int>(() => +dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(23, (int)e.OldValue);
                    Assert.AreEqual(42, (int)e.NewValue);
                };

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = +42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Long_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<long>() { Item = 23 };

                var test = Observable.Expression<long>(() => +dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsFalse(update);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Long_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<long>() { Item = 23 };

                var test = Observable.Expression<long>(() => +dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(23, (long)e.OldValue);
                    Assert.AreEqual(42, (long)e.NewValue);
                };

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Double_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<double>() { Item = 23 };

                var test = Observable.Expression<double>(() => +dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsFalse(update);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Double_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<double>() { Item = 23 };

                var test = Observable.Expression<double>(() => +dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(23, (double)e.OldValue);
                    Assert.AreEqual(42, (double)e.NewValue);
                };

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Float_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<float>() { Item = 23 };

                var test = Observable.Expression<float>(() => +dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsFalse(update);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Float_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<float>() { Item = 23 };

                var test = Observable.Expression<float>(() => +dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(23, (float)e.OldValue);
                    Assert.AreEqual(42, (float)e.NewValue);
                };

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Decimal_NoObservable_NoUpdates()
        {
            checked
            {
                var update = false;
                var dummy = new Dummy<decimal>() { Item = 23 };

                var test = Observable.Expression<decimal>(() => +dummy.Item);

                test.ValueChanged += (o, e) => update = true;

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsFalse(update);
            }
        }

        [TestMethod]
        public void UnaryPlusChecked_Decimal_Observable_Updates()
        {
            checked
            {
                var update = false;
                var dummy = new ObservableDummy<decimal>() { Item = 23 };

                var test = Observable.Expression<decimal>(() => +dummy.Item);

                test.ValueChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(23, (decimal)e.OldValue);
                    Assert.AreEqual(42, (decimal)e.NewValue);
                };

                Assert.AreEqual(23, test.Value);
                Assert.IsFalse(update);

                dummy.Item = 42;

                Assert.IsTrue(update);
                Assert.AreEqual(42, test.Value);
            }
        }

        [TestMethod]
        public void Negate_Bool_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<bool>() { Item = true };

            var test = Observable.Expression<bool>(() => !dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            dummy.Item = false;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Negate_Bool_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<bool>() { Item = true };

            var test = Observable.Expression<bool>(() => !dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            dummy.Item = false;

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }
    }
}
