using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions.Test
{
    static class Helpers
    {
        [LensPut(typeof(Helpers), "PutFirst")]
        public static T FirstOrDefault<T>(this T[] array)
        {
            return array != null && array.Length > 0 ? array[0] : default(T);
        }

        public static T[] PutFirst<T>(this T[] array, T element)
        {
            if (array != null && array.Length > 0)
            {
                array[0] = element;
                return array;
            }
            else if (EqualityComparer<T>.Default.Equals(element, default(T)))
            {
                return array;
            }
            else
            {
                return new T[] { element };
            }
        }

        private static ObservingFunc<Dummy<string>, Dummy<string>, string> combineFunc = new ObservingFunc<Dummy<string>, Dummy<string>, string>((d1, d2) => d1.Item + d2.Item);


        [LensPut(typeof(Helpers), "PutCombine")]
        [ObservableProxy(typeof(Helpers), "CombineProxy")]
        public static string Combine(Dummy<string> arg1, Dummy<string> arg2)
        {
            return combineFunc.Evaluate(arg1, arg2);
        }

        [LensPut(typeof(Helpers), "PutCombineInc")]
        [ObservableProxy(typeof(Helpers), "CombineProxyInc")]
        public static string CombineInc(Dummy<string> arg1, Dummy<string> arg2)
        {
            return combineFunc.Evaluate(arg1, arg2);
        }
        
        public static INotifyValue<string> CombineProxy(Dummy<string> arg1, Dummy<string> arg2)
        {
            return combineFunc.Observe(arg1, arg2);
        }
        
        public static INotifyValue<string> CombineProxyInc(INotifyValue<Dummy<string>> arg1, INotifyValue<Dummy<string>> arg2)
        {
            return combineFunc.Observe(arg1, arg2);
        }

        public static void PutCombine(Dummy<string> arg1, Dummy<string> arg2, string value)
        {
            arg1.Item = value.Substring(0, 1);
            arg2.Item = value.Substring(1);
        }

        public static void PutCombineInc(Dummy<string> arg1, Dummy<string> arg2, string value)
        {
            arg1.Item = value.Substring(0, 1);
            arg2.Item = value.Substring(1);
        }
    }

    [TestClass]
    public class LensPutTests
    {
        private Func<Dummy<int[]>, int> getter;
        private Action<Dummy<int[]>, int> setter;

        [TestInitialize]
        public void CreateGetterAndSetter()
        {
            Expression<Func<Dummy<int[]>, int>> expression = dummy => dummy.Item.FirstOrDefault();

            getter = expression.Compile();
            var setExp = SetExpressionRewriter.CreateSetter(expression);
            setter = setExp.Compile();
        }

        [TestMethod]
        public void PutGetForEmptyArray()
        {
            var dummy = new ObservableDummy<int[]>(new int[] { });
            var test = Observable.Reversable(() => dummy.Item.FirstOrDefault());

            Assert.AreEqual(0, test.Value);
            test.Value = 42;
            Assert.AreEqual(42, dummy.Item[0]);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void PutGetForNullArray()
        {
            var dummy = new ObservableDummy<int[]>(null);
            var test = Observable.Reversable(() => dummy.Item.FirstOrDefault());

            Assert.AreEqual(0, test.Value);
            test.Value = 42;
            Assert.AreEqual(42, dummy.Item[0]);
            Assert.AreEqual(42, test.Value);
        }


        [TestMethod]
        public void PutGetForInitializedArray()
        {
            var dummy = new ObservableDummy<int[]>(new int[] { 0, 8, 15 });
            var test = Observable.Reversable(() => dummy.Item.FirstOrDefault());

            Assert.AreEqual(0, test.Value);
            test.Value = 42;
            Assert.AreEqual(42, dummy.Item[0]);
            // Arrays do not support an update notification for entries and therefore, the following assertions fails
            //Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void GetPutForEmptyArray()
        {
            var array = new int[] { };
            var dummy = new Dummy<int[]>(array);
            var test = Observable.Reversable(() => dummy.Item.FirstOrDefault());

            Assert.AreEqual(0, test.Value);
            test.Value = 0;
            Assert.AreEqual(array, dummy.Item);
        }

        [TestMethod]
        public void GetPutForNullArray()
        {
            var dummy = new Dummy<int[]>(null);
            var test = Observable.Reversable(() => dummy.Item.FirstOrDefault());

            Assert.AreEqual(0, test.Value);
            test.Value = 0;
            Assert.AreEqual(null, dummy.Item);
        }


        [TestMethod]
        public void GetPutForInitializedArray()
        {
            var array = new int[] { 0, 8, 15 };
            var dummy = new Dummy<int[]>(array);
            var test = Observable.Reversable(() => dummy.Item.FirstOrDefault());

            Assert.AreEqual(0, test.Value);
            test.Value = 0;
            Assert.AreEqual(array, dummy.Item);
        }


        [TestMethod]
        public void PutGetForEmptyArrayWithFunc()
        {
            var dummy = new ObservableDummy<int[]>(new int[] { });
            var func = Observable.Func<Dummy<int[]>, int>(d => d.Item.FirstOrDefault());
            var test = func.InvokeReversable(dummy);

            Assert.AreEqual(0, test.Value);
            test.Value = 42;
            Assert.AreEqual(42, dummy.Item[0]);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void PutGetForNullArrayWithFunc()
        {
            var dummy = new ObservableDummy<int[]>(null);
            var func = Observable.Func<Dummy<int[]>, int>(d => d.Item.FirstOrDefault());
            var test = func.InvokeReversable(dummy);

            Assert.AreEqual(0, test.Value);
            test.Value = 42;
            Assert.AreEqual(42, dummy.Item[0]);
            Assert.AreEqual(42, test.Value);
        }


        [TestMethod]
        public void PutGetForInitializedArrayWithFunc()
        {
            var dummy = new ObservableDummy<int[]>(new int[] { 0, 8, 15 });
            var func = Observable.Func<Dummy<int[]>, int>(d => d.Item.FirstOrDefault());
            var test = func.InvokeReversable(dummy);

            Assert.AreEqual(0, test.Value);
            test.Value = 42;
            Assert.AreEqual(42, dummy.Item[0]);
            // Arrays do not support an update notification for entries and therefore, the following assertions fails
            //Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void GetPutForEmptyArrayWithFunc()
        {
            var array = new int[] { };
            var dummy = new Dummy<int[]>(array);
            var func = Observable.Func<Dummy<int[]>, int>(d => d.Item.FirstOrDefault());
            var test = func.InvokeReversable(dummy);

            Assert.AreEqual(0, test.Value);
            test.Value = 0;
            Assert.AreEqual(array, dummy.Item);
        }

        [TestMethod]
        public void GetPutForNullArrayWithFunc()
        {
            var dummy = new Dummy<int[]>(null);
            var func = Observable.Func<Dummy<int[]>, int>(d => d.Item.FirstOrDefault());
            var test = func.InvokeReversable(dummy);

            Assert.AreEqual(0, test.Value);
            test.Value = 0;
            Assert.AreEqual(null, dummy.Item);
        }


        [TestMethod]
        public void GetPutForInitializedArrayWithFunc()
        {
            var array = new int[] { 0, 8, 15 };
            var dummy = new Dummy<int[]>(array);
            var func = Observable.Func<Dummy<int[]>, int>(d => d.Item.FirstOrDefault());
            var test = func.InvokeReversable(dummy);

            Assert.AreEqual(0, test.Value);
            test.Value = 0;
            Assert.AreEqual(array, dummy.Item);
        }

        [TestMethod]
        public void PutGetForCompiledEmptyArray()
        {
            var dummy = new Dummy<int[]>(new int[] { });

            Assert.AreEqual(0, getter(dummy));
            setter(dummy, 42);
            Assert.AreEqual(42, dummy.Item[0]);
            Assert.AreEqual(42, getter(dummy));
        }

        [TestMethod]
        public void PutGetForCompiledNullArray()
        {
            var dummy = new Dummy<int[]>(null);

            Assert.AreEqual(0, getter(dummy));
            setter(dummy, 42);
            Assert.AreEqual(42, dummy.Item[0]);
            Assert.AreEqual(42, getter(dummy));
        }


        [TestMethod]
        public void PutGetForCompiledInitializedArray()
        {
            var dummy = new Dummy<int[]>(new int[] { 0, 8, 15 });

            Assert.AreEqual(0, getter(dummy));
            setter(dummy, 42);
            Assert.AreEqual(42, dummy.Item[0]);
            Assert.AreEqual(42, getter(dummy));
        }

        [TestMethod]
        public void GetPutForCompiledEmptyArray()
        {
            var array = new int[] { };
            var dummy = new Dummy<int[]>(array);

            Assert.AreEqual(0, getter(dummy));
            setter(dummy, 0);
            Assert.AreEqual(array, dummy.Item);
        }

        [TestMethod]
        public void GetPutForCompiledNullArray()
        {
            var dummy = new Dummy<int[]>(null);

            Assert.AreEqual(0, getter(dummy));
            setter(dummy, 0);
            Assert.AreEqual(null, dummy.Item);
        }


        [TestMethod]
        public void GetPutForCompiledInitializedArray()
        {
            var array = new int[] { 0, 8, 15 };
            var dummy = new Dummy<int[]>(array);

            Assert.AreEqual(0, getter(dummy));
            setter(dummy, 0);
            Assert.AreEqual(array, dummy.Item);
        }

        [TestMethod]
        public void LensPutWithProxy()
        {
            var dummy1 = new ObservableDummy<string>("A");
            var dummy2 = new ObservableDummy<string>("B");

            var test = Observable.Reversable(() => Helpers.Combine(dummy1, dummy2));
            Assert.AreEqual("AB", test.Value);

            var updated = false;
            test.ValueChanged += (o, e) =>
            {
                updated = true;
            };

            var dummy1Updated = false;
            dummy1.ItemChanged += (o, e) =>
            {
                dummy1Updated = true;
            };
            var dummy2Updated = false;
            dummy2.ItemChanged += (o, e) =>
            {
                dummy2Updated = true;
            };

            dummy2.Item = "C";

            Assert.IsTrue(updated);
            Assert.IsFalse(dummy1Updated);
            Assert.IsTrue(dummy2Updated);
            Assert.AreEqual("AC", test.Value);

            updated = false;
            dummy2Updated = false;
            test.Value = "BC";

            Assert.IsTrue(dummy1Updated);
            Assert.AreEqual("B", dummy1.Item);
            Assert.IsTrue(updated);
            Assert.IsFalse(dummy2Updated);
        }

        [TestMethod]
        public void LensPutWithProxyInc()
        {
            // not yet implemented
            //var dummy1 = new ObservableDummy<string>("A");
            //var dummy2 = new ObservableDummy<string>("B");

            //var test = Observable.Reversable(() => Helpers.CombineInc(dummy1, dummy2));
            //Assert.AreEqual("AB", test.Value);

            //var updated = false;
            //test.ValueChanged += (o, e) =>
            //{
            //    updated = true;
            //};

            //var dummy1Updated = false;
            //dummy1.ItemChanged += (o, e) =>
            //{
            //    dummy1Updated = true;
            //};
            //var dummy2Updated = false;
            //dummy2.ItemChanged += (o, e) =>
            //{
            //    dummy2Updated = true;
            //};

            //dummy2.Item = "C";

            //Assert.IsTrue(updated);
            //Assert.IsFalse(dummy1Updated);
            //Assert.IsTrue(dummy2Updated);
            //Assert.AreEqual("AC", test.Value);

            //updated = false;
            //dummy2Updated = false;
            //test.Value = "BC";

            //Assert.IsTrue(dummy1Updated);
            //Assert.AreEqual("B", dummy1.Item);
            //Assert.IsTrue(updated);
            //Assert.IsFalse(dummy2Updated);
        }
    }
}
