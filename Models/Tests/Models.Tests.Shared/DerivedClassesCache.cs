using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Models.Tests.Shared
{
    class DerivedClassesCache
    {
        private static readonly Dictionary<IClass, List<IClass>> cachedDerived;

        static DerivedClassesCache()
        {
            cachedDerived = new Dictionary<IClass, List<IClass>>();
            foreach (var metaclass in MetaRepository.Instance.Models.Values.SelectMany(m => m.Descendants().OfType<IClass>()))
            {
                foreach (var baseCl in metaclass.BaseTypes)
                {
                    List<IClass> classes;
                    if (!cachedDerived.TryGetValue(baseCl, out classes))
                    {
                        classes = new List<IClass>();
                        cachedDerived.Add(baseCl, classes);
                    }
                    classes.Add(metaclass);
                }
            }
        }

        public static IClass FindNonAbstractDerived(IClass abstractBase)
        {
            var toInvestigate = new Queue<IClass>();

            toInvestigate.Enqueue(abstractBase);

            while (toInvestigate.Count > 0)
            {
                var candidate = toInvestigate.Dequeue();
                if (!candidate.IsAbstract)
                {
                    return candidate;
                }
                else if (cachedDerived.TryGetValue(candidate, out var derived))
                {
                    foreach (var item in derived)
                    {
                        if (item != Model.ClassInstance)
                        {
                            toInvestigate.Enqueue(item);
                        }
                    }
                }
            }

            return null;
        }
    }
}
