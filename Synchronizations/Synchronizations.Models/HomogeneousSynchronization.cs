using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Synchronizations.Models
{
    public class HomogeneousSynchronization<T> : ReflectiveSynchronization
    {
        private static Model targetModel = MetaRepository.Instance.ResolveClass(typeof(T)).Model;

        protected override IEnumerable<SynchronizationRuleBase> CreateDefaultSynchronizationRules()
        {
            var dict = new Dictionary<System.Type, SynchronizationRuleBase>();
            foreach (var cl in targetModel.Descendants().OfType<IClass>())
            {
                FillDictForClass(dict, cl);
            }
            FillDictForClass(dict, MetaRepository.Instance.ResolveClass(typeof(Model)) as IClass);
            return dict.Values;
        }

        private static void FillDictForClass(Dictionary<System.Type, SynchronizationRuleBase> dict, IClass cl)
        {
            var mapping = cl.GetExtension<MappedType>();
            if (mapping != null)
            {
                var iface = mapping.SystemType;
                if (iface != null)
                {
                    if (dict.ContainsKey(iface)) return;
                    var rule = (SynchronizationRuleBase)Activator.CreateInstance(typeof(ModelCopyRule<>).MakeGenericType(iface));
                    dict.Add(iface, rule);
                    if (iface != typeof(IModelElement))
                    {
                        foreach (var baseType in cl.BaseTypes)
                        {
                            FillDictForClass(dict, baseType);
                        }
                        foreach (var referencedType in cl.References.Select(r => r.ReferenceType).OfType<IClass>())
                        {
                            FillDictForClass(dict, referencedType);
                        }
                    }
                }
            }
        }
    }
}
