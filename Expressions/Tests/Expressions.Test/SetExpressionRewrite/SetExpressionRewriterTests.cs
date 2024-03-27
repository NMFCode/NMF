using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace NMF.Expressions.Test.SetExpressionRewrite
{
    [TestClass]
    public class SetExpressionRewriterTests
    {
        [TestMethod]
        public void SetExpressionRewriter_WritableProperty_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, int>> test = d => d.WritableProperty;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 42 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_ReadonlyProperty_ReturnsNull()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, int>> test = d => d.ReadOnlyProperty;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNull( rewriter );
        }

        [TestMethod]
        public void SetExpressionRewriter_WritableWithPrivateSetter_ReturnsNull()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, int>> test = d => d.WritableWithPrivateSetter;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNull( rewriter );
        }

        [TestMethod]
        public void SetExpressionRewriter_AddConstant_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, int>> test = d => d.WritableProperty + 1;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 43 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_SubtractConstant_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, int>> test = d => d.WritableProperty - 1;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 41 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_MultiplyConstant_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, int>> test = d => d.WritableProperty * 2;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 84 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_DivideConstant_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, int>> test = d => d.WritableProperty / 2;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 21 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_Convert_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, double>> test = d => d.WritableProperty;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 42 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_Negate_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, double>> test = d => -d.WritableProperty;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, -42 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_SubtractFrom_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, double>> test = d => 100 - d.WritableProperty;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 58 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }

        [TestMethod]
        public void SetExpressionRewriter_DivideFrom_CanBeInverted()
        {
            var dummy = new Dummy();

            Expression<Func<Dummy, double>> test = d =>  84 / d.WritableProperty;

            var rewriter = SetExpressionRewriter.CreateSetter( test );

            Assert.IsNotNull( rewriter );
            var compiled = rewriter.Compile();

            compiled( dummy, 2 );
            Assert.AreEqual( 42, dummy.WritableProperty );
        }



        private class Dummy
        {
            public int WritableProperty { get; set; }

            public int ReadOnlyProperty => WritableProperty;

            public int WritableWithPrivateSetter
            {
                get => WritableProperty;
                private set => WritableProperty = value;
            }
        }

    }
}
