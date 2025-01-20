using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    internal static class ParameterAssertions
    {

        public static void AssertOnlyParameters(Expression expression, params ParameterExpression[] parameters)
        {
            if (expression is LambdaExpression lambda)
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
