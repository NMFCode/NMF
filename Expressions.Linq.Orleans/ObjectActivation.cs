using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Utilities
{
    public static class Foo
    {
        public static void Bar() { }
    }


    /// <summary>
    ///     Class supporting activation of objects via lambda expressions and a constructor. Much faster than
    ///     using Activator.CreateInstance().
    ///     Taken from https://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/.
    /// </summary>
    public static class ObjectActivation
    {
        public static ObjectActivator<T> GetActivator<T>(ConstructorInfo ctor)
        {
            var type = ctor.DeclaringType;
            var paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            var param =
                Expression.Parameter(typeof(object[]), "args");

            var argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            var newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            var lambda =
                Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

            //compile it
            var compiled = (ObjectActivator<T>) lambda.Compile();
            return compiled;
        }
    }

    public delegate T ObjectActivator<out T>(params object[] args);
}