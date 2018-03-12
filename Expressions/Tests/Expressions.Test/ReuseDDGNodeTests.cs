using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class ReuseDDGNodeTests
    {
        [TestMethod]
        public void ReuseSimpleDependencyGraphNode()
        {
            NotifySystem.DefaultSystem = new InstructionLevelNotifySystem();
            var dummy = new ObservableDummy<int>(42);
            var func = Observable.Func<Dummy<int>, int, int>((arg1, arg2) => arg1.Item + arg2 + arg2);
            var evaluateMethod = func.GetType().GetMethod("Evaluate");
            var par = Expression.Parameter(typeof(Dummy<int>), "d");
            var expression = Expression.Lambda<Func<Dummy<int>, int>>(Expression.Call(Expression.Constant(func), evaluateMethod, par, Expression.MakeMemberAccess(par, typeof(Dummy<int>).GetProperty("Item"))), par);
            var func2 = Observable.Func(expression); // d => func.Evaluate(d, d.Item)

            var ddgtemplate = func2.Expression.Closure(node => node.Dependencies);
            Assert.AreEqual(6, ddgtemplate.Count());
            var test = func2.Observe(dummy);
            test.Successors.SetDummy();
            var ddg = test.Closure<INotifiable>(n => n.Dependencies);
            Assert.AreEqual(6, ddg.Count());
        }
    }
}
