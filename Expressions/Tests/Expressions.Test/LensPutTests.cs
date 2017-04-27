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
    }
}
