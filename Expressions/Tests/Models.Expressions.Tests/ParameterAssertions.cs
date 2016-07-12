using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    internal static class ParameterAssertions
    {

        public static void AssertOnlyParameters(Expression expression, params ParameterExpression[] parameters)
        {
            var lambda = expression as LambdaExpression;
            if (lambda != null)
            {
                expression = lambda.Body;
            }
            var parameterCollector = new ParameterCollector();
            parameterCollector.Visit(expression);
            Assert.AreEqual(parameters.Length, parameterCollector.Parameters.Count, string.Format("The expression '{0}' was expected to have {1} parameters but had {2}",
                expression, parameters.Length, parameterCollector.Parameters.Count));
            for (int i = 0; i < parameters.Length; i++)
            {
                Assert.IsTrue(parameterCollector.Parameters.Contains(parameters[i]));
            }
        }
    }
}
