using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using NMF.Models;
using NMF.Utilities;
using NMF.Models.Meta;

namespace NMF.Controls.ContextMenu
{
    public class ContextMenuRegistry : IValueConverter
    {
        private Dictionary<IClass, List<IClass>> cachedDerived;
        private Dictionary<System.Type, List<(IReference, System.Type)>> cachedContextMenus = new Dictionary<System.Type, List<(IReference, System.Type)>>();
        private Dictionary<IReferenceType, HashSet<System.Type>> cachedImplementationTypes = new Dictionary<IReferenceType, HashSet<System.Type>>();

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            var element = value as IModelElement;
            if (element == null) return null;
            var type = value.GetType();
            List<(IReference, System.Type)> menuRegistry;
            if (!cachedContextMenus.TryGetValue(type, out menuRegistry))
            {
                menuRegistry = new List<(IReference, System.Type)>();

                var metaClass = (value as IModelElement).GetClass();
                foreach (var containment in metaClass.Closure(cl => cl.BaseTypes).SelectMany(cl => cl.References.Where(r => r.IsContainment)))
                {
                    FillAllReferences(containment, menuRegistry);
                }

                cachedContextMenus.Add(type, menuRegistry);
            }
            return menuRegistry.Select(t => new AddChildMenuItem(t.Item1, t.Item2, element)).ToList();
        }

        private void FillAllReferences(IReference containment, List<(IReference, System.Type)> menuRegistry)
        {
            foreach (var candidateType in GetCandidateClasses(containment.ReferenceType))
            {
                menuRegistry.Add((containment, candidateType));
            }
        }

        private HashSet<System.Type> GetCandidateClasses(IReferenceType type)
        {
            HashSet<System.Type> candidates;
            if (!cachedImplementationTypes.TryGetValue(type, out candidates))
            {
                candidates = new HashSet<System.Type>();
                var metaclass = type as IClass;
                if (metaclass == null)
                {
                    candidates.Add(GetImplementationType(type));
                }
                else
                {
                    AddImplementationClasses(metaclass, candidates);
                }
                cachedImplementationTypes.Add(type, candidates);
            }
            return candidates;
        }

        private void AddImplementationClasses(IClass metaclass, HashSet<System.Type> candidates)
        {
            if (!metaclass.IsAbstract) candidates.Add(GetImplementationType(metaclass));

            if (cachedDerived == null) CreateCachedDerivedClasses();

            List<IClass> derived;
            if (cachedDerived.TryGetValue(metaclass, out derived))
            {
                foreach (var derivedCl in derived)
                {
                    AddImplementationClasses(derivedCl, candidates);
                }
            }
        }

        private void CreateCachedDerivedClasses()
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

        private System.Type GetImplementationType(IReferenceType type)
        {
            var mappedType = type.GetExtension<MappedType>();
            var systemType = mappedType.SystemType;
            if (systemType.IsInterface)
            {
                var defaultImplementation = systemType.GetCustomAttributes(typeof(DefaultImplementationTypeAttribute), false);
                systemType = (defaultImplementation[0] as DefaultImplementationTypeAttribute).DefaultImplementationType;
            }
            return systemType;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
