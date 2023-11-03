using NMF.Utilities;
using System;

namespace NMF.Glsp.Processing
{
    internal class InstantiationHelper
    {
        private static Type GetImplementationType(Type type)
        {
            if (!type.IsAbstract && !type.IsInterface) return type;

            var customs = type.GetCustomAttributes(typeof(DefaultImplementationTypeAttribute), false);
            if (customs != null && customs.Length > 0)
            {
                var defaultImplAtt = customs[0] as DefaultImplementationTypeAttribute;
                return defaultImplAtt.DefaultImplementationType;
            }
            return null;
        }

        public static bool CanCreateInstance<T>() => Helper<T>.Instantiator != null;

        public static T CreateInstance<T>() => Helper<T>.Instantiator();

        private class Helper<T>
        {
            public static Func<T> Instantiator = CreateInstantiationFunc();

            private static Func<T> CreateInstantiationFunc()
            {
                var implementationType = GetImplementationType(typeof(T));
                if (implementationType == null) return null;
                return () => (T)Activator.CreateInstance(implementationType);
            }
        }
    }
}
