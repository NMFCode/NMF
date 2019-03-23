using NMF.Collections.ObjectModel;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Models.Tests.Shared
{
    public abstract class ModelTestBase<T> where T : IModelElement, new()
    {
        protected readonly IClass Class;
        protected readonly IEnumerable<IClass> AllAncestors;

        protected readonly HashSet<IAttribute> fixedAttributes = new HashSet<IAttribute>();
        protected readonly Dictionary<IReference, IReference> refinesLookup = new Dictionary<IReference, IReference>();

        public ModelTestBase()
        {
            Class = MetaRepository.Instance.ResolveClass(typeof(T)) as IClass;
            AllAncestors = Class.Closure(c => c.BaseTypes).ToList();
            foreach (var r in AllAncestors.SelectMany(c => c.References))
            {
                if (r.Refines != null && !refinesLookup.ContainsKey(r.Refines))
                {
                    refinesLookup.Add(r.Refines, r);
                }
            }
            foreach (var attC in AllAncestors.SelectMany(c => c.AttributeConstraints))
            {
                fixedAttributes.Add(attC.Constrains);
            }
            foreach (var refC in AllAncestors.SelectMany(c => c.ReferenceConstraints))
            {
                if (!refinesLookup.ContainsKey(refC.Constrains))
                {
                    refinesLookup.Add(refC.Constrains, null);
                }
            }
        }

        #region Abstract members

        protected abstract void AssertIsTrue(bool condition, string message = null);

        protected abstract void AssertIsFalse(bool condition, string message = null);

        protected abstract void AssertThrowsException<TException>(Action toPerform, string message = null)
            where TException : Exception;

        protected abstract void AssertAreEqual(object expected, object actual, string message = null);

        #endregion

        #region Helper Methods

        protected void CheckForAllAttributes(Action<IAttribute> toCheck, [CallerMemberName] string task = null)
        {
            foreach (var attribute in AllAncestors.SelectMany(c => c.Attributes))
            {
                if (attribute.DeclaringType.Name == "ModelElement") continue;
                if (fixedAttributes.Contains(attribute)) continue;
                if (ShouldCheckAttribute(attribute, task))
                {
                    toCheck?.Invoke(attribute);
                }
            }
        }

        protected virtual bool ShouldCheckAttribute(IAttribute attribute, string task)
        {
            return true;
        }

        protected void CheckForAllReferences(Action<IReference> toCheck, [CallerMemberName] string task = null)
        {
            foreach (var reference in AllAncestors.SelectMany(c => c.References))
            {
                if (reference.DeclaringType.Name == "ModelElement") continue;
                if (refinesLookup.TryGetValue(reference, out var refType) && refType == null) continue;
                if (ShouldCheckReference(reference, task))
                {
                    toCheck?.Invoke(reference);
                }
            }
        }

        protected virtual bool ShouldCheckReference(IReference reference, string task)
        {
            return true;
        }

        protected void CheckForAllContainments(Action<IReference> toCheck, [CallerMemberName] string task = null)
        {
            foreach (var reference in AllAncestors.SelectMany(c => c.References))
            {
                if (!reference.IsContainment) continue;
                if (reference.DeclaringType.Name == "ModelElement") continue;
                if (refinesLookup.TryGetValue(reference, out var refType) && refType == null) continue;
                if (ShouldCheckReference(reference, task))
                {
                    toCheck?.Invoke(reference);
                }
            }
        }

        protected virtual object GetPrimaryValue(ITypedElement feature)
        {
            return GetValue(feature, false);
        }

        protected virtual object GetOtherValue(ITypedElement feature)
        {
            return GetValue(feature, true);
        }

        private object GetValue(ITypedElement feature, bool other)
        {
            var type = feature.Type;
            if (feature is IReference reference)
            {
                while (refinesLookup.TryGetValue(reference, out var refiningReference))
                {
                    reference = refiningReference;
                }
                type = reference.Type;
            }
            switch (type)
            {
                case PrimitiveType primitive:
                    return GetValueCore(primitive, other);
                case Class @class:
                    return GetValueCore(DerivedClassesCache.FindNonAbstractDerived(@class), other);
                case Enumeration enumeration:
                    return GetValueCore(enumeration, other);
                default:
                    break;
            }
            return null;
        }

        private object GetValueCore(Enumeration enumeration, bool other)
        {
            var systemType = MappedType.FromType(enumeration).SystemType;
            return Enum.GetValues(systemType).GetValue(other ? 1 : 0);
        }

        private object GetValueCore(PrimitiveType type, bool other)
        {
            var systemType = MappedType.FromType(type).SystemType;
            if (systemType == typeof(bool)) return other;
            if (systemType == typeof(Uri)) return other ?
                new Uri("foo://bar") : new Uri("bar://foo");
            if (other)
            {
                return Convert.ChangeType(42, systemType);
            }
            else
            {
                return Convert.ChangeType(23, systemType);
            }
        }

        private object GetValueCore(IClass @class, bool other)
        {
            if (@class == null) return null;
            var systemType = MappedType.FromType(@class).SystemType;
            var defaultImplementation = systemType.GetCustomAttributes(typeof(DefaultImplementationTypeAttribute), false);
            return Activator.CreateInstance((defaultImplementation[0] as DefaultImplementationTypeAttribute).DefaultImplementationType);
        }

        private void AssertEventsRaised(ITypedElement feature, IModelElement element, bool changing, bool changed)
        {
            AssertIsTrue(changing, $"{element} should have fired {(feature.UpperBound == 1 ? "PropertyChanging" : "CollectionChanging")} event");
            AssertIsTrue(changed, $"{element} should have fired {(feature.UpperBound == 1 ? "PropertyChanged" : "CollectionChanged")} event");
        }

        #endregion

        #region Test methods

        public virtual void ResolveOfRelativeUri_SucceedsAndFindsCorrectModelElement()
        {
            CheckForAllContainments(con =>
            {
                var model = new Model();
                var element = new T();
                model.RootElements.Add(element);
                var child = GetPrimaryValue(con) as IModelElement;
                var identifier = child.GetClass().RetrieveIdentifier();
                if (identifier.Identifier != null)
                {
                    child.SetAttributeValue(identifier.Identifier, GetPrimaryValue(identifier.Identifier));
                }
                if (con.UpperBound == 1)
                {
                    element.SetReferencedElement(con, child);
                }
                else
                {
                    element.GetReferencedElements(con).Add(child);
                }
                AssertIsTrue(element.Children.Contains(child));
                var uri = child.RelativeUri;
                if (con.IsOrdered || child.IsIdentified)
                {
                    AssertIsFalse(uri == null, "relative Uri must exist");
                    AssertAreEqual(child, model.Resolve(uri));
                }
            });
        }

        public virtual void AttributeChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            CheckForAllAttributes(att =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                if (att.UpperBound == 1)
                {
                    element.SetAttributeValue(att, GetPrimaryValue(att));

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;

                    element.SetAttributeValue(att, GetOtherValue(att));
                }
                else
                {
                    var coll = element.GetAttributeValues(att);
                    coll.Add(GetPrimaryValue(att));

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;

                    coll.Add(GetOtherValue(att));
                }
                AssertEventsRaised(att, element, changing, changed);
            });
        }
        
        public virtual void AttributeChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            CheckForAllAttributes(att =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                var model = new Model();
                model.RootElements.Add(element);
                var unlockRequested = false;
                model.UnlockRequested += (o, e) =>
                {
                    e.MayUnlock = true;
                    unlockRequested = true;
                };
                if (att.UpperBound == 1)
                {
                    element.SetAttributeValue(att, GetPrimaryValue(att));
                    element.Lock();

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;

                    element.SetAttributeValue(att, GetOtherValue(att));

                }
                else
                {
                    var coll = element.GetAttributeValues(att);
                    coll.Add(GetPrimaryValue(att));
                    element.Lock();

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;

                    coll.Add(GetOtherValue(att));
                }
                AssertEventsRaised(att, element, changing, changed);
                AssertIsTrue(unlockRequested, "Unlock request should have been issued");
            });
        }
        
        public virtual void AttributeChange_StateLockedNoUnlock_ThrowsException()
        {
            CheckForAllAttributes(att =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                var model = new Model();
                model.RootElements.Add(element);
                model.UnlockRequested += (o, e) =>
                {
                    e.MayUnlock = false;
                };

                if (att.UpperBound == 1)
                {

                    element.SetAttributeValue(att, GetPrimaryValue(att));

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;
                    element.Lock();

                    AssertThrowsException<LockedException>(() => element.SetAttributeValue(att, GetOtherValue(att)));

                }
                else
                {
                    var coll = element.GetAttributeValues(att);
                    coll.Add(GetPrimaryValue(att));

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;
                    element.Lock();

                    AssertThrowsException<LockedException>(() => coll.Add(GetOtherValue(att)));
                }
                AssertIsFalse(changing);
                AssertIsFalse(changed);
            });
        }
        
        public virtual void AttributeChange_StateFixed_ThrowsException()
        {
            CheckForAllAttributes(att =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                var model = new Model();
                model.RootElements.Add(element);
                var unlockRequested = false;
                model.UnlockRequested += (o, e) =>
                {
                    e.MayUnlock = true;
                    unlockRequested = true;
                };

                if (att.UpperBound == 1)
                {

                    element.SetAttributeValue(att, GetPrimaryValue(att));

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;
                    element.Freeze();

                    AssertThrowsException<LockedException>(() => element.SetAttributeValue(att, GetOtherValue(att)));

                }
                else
                {
                    var coll = element.GetAttributeValues(att);
                    coll.Add(GetPrimaryValue(att));

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;
                    element.Freeze();

                    AssertThrowsException<LockedException>(() => coll.Add(GetOtherValue(att)));
                }
                AssertIsFalse(changing);
                AssertIsFalse(changed);
                AssertIsFalse(unlockRequested);
            });
        }
        
        public virtual void ReferenceChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            CheckForAllReferences(reference =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                var other = GetOtherValue(reference) as IModelElement;
                if (reference.UpperBound == 1)
                {
                    element.SetReferencedElement(reference, GetPrimaryValue(reference) as IModelElement);

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;

                    element.SetReferencedElement(reference, other);
                }
                else
                {
                    var coll = element.GetReferencedElements(reference);
                    coll.Add(GetPrimaryValue(reference));

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;

                    coll.Add(other);
                }

                AssertIsTrue(changing);
                AssertIsTrue(changed);

                if (other != null)
                {
                    PerformChecksForModelElement(reference, element, other);
                }
            });
        }
        
        public virtual void ReferenceChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            CheckForAllReferences(reference =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                var model = new Model();
                var primary = GetPrimaryValue(reference) as IModelElement;
                if (reference.Opposite == null || !reference.Opposite.IsContainment)
                {
                    model.RootElements.Add(element);
                }
                else
                {
                    model.RootElements.Add(primary);
                }
                var unlockRequested = false;
                model.UnlockRequested += (o, e) =>
                {
                    e.MayUnlock = true;
                    unlockRequested = true;
                };
                var other = GetOtherValue(reference) as IModelElement;
                if (reference.UpperBound == 1)
                {
                    element.SetReferencedElement(reference, primary);
                    element.Lock();

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;

                    element.SetReferencedElement(reference, other);

                }
                else
                {
                    var coll = element.GetReferencedElements(reference);
                    coll.Add(primary);
                    element.Lock();

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;

                    coll.Add(other);
                }
                AssertIsTrue(changing);
                AssertIsTrue(changed);
                AssertIsTrue(unlockRequested);

                if (other != null)
                {
                    PerformChecksForModelElement(reference, element, other);
                }
            });
        }
        
        public virtual void ReferenceChange_StateLockedNoUnlock_ThrowsException()
        {
            CheckForAllReferences(reference =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                var model = new Model();
                var primary = GetPrimaryValue(reference) as IModelElement;
                if (reference.Opposite == null || !reference.Opposite.IsContainment)
                {
                    model.RootElements.Add(element);
                }
                else
                {
                    model.RootElements.Add(primary);
                }
                model.UnlockRequested += (o, e) =>
                {
                    e.MayUnlock = false;
                };

                if (reference.UpperBound == 1)
                {

                    element.SetReferencedElement(reference, primary);

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;
                    model.Lock();

                    AssertThrowsException<LockedException>(() => element.SetReferencedElement(reference, GetOtherValue(reference) as IModelElement));

                }
                else
                {
                    var coll = element.GetReferencedElements(reference);
                    coll.Add(primary);

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;
                    model.Lock();

                    AssertThrowsException<LockedException>(() => coll.Add(GetOtherValue(reference)));
                }
                AssertIsFalse(changing);
                AssertIsFalse(changed);
            });
        }
        
        public virtual void ReferenceChange_StateFixed_ThrowsException()
        {
            CheckForAllReferences(reference =>
            {
                var element = new T();
                var changing = false;
                var changed = false;
                var model = new Model();
                var primary = GetPrimaryValue(reference) as IModelElement;
                if (reference.Opposite == null || !reference.Opposite.IsContainment)
                {
                    model.RootElements.Add(element);
                }
                else
                {
                    model.RootElements.Add(primary);
                }
                var unlockRequested = false;
                model.UnlockRequested += (o, e) =>
                {
                    e.MayUnlock = true;
                    unlockRequested = true;
                };

                if (reference.UpperBound == 1)
                {

                    element.SetReferencedElement(reference, primary);

                    element.PropertyChanging += (o, e) => changing = true;
                    element.PropertyChanged += (o, e) => changed = true;
                    model.Freeze();

                    AssertThrowsException<LockedException>(() => element.SetReferencedElement(reference, GetOtherValue(reference) as IModelElement));

                }
                else
                {
                    var coll = element.GetReferencedElements(reference);
                    coll.Add(primary);

                    (coll as INotifyCollectionChanging).CollectionChanging += (o, e) => changing = true;
                    (coll as INotifyCollectionChanged).CollectionChanged += (o, e) => changed = true;
                    model.Freeze();

                    AssertThrowsException<LockedException>(() => coll.Add(GetOtherValue(reference)));
                }
                AssertIsFalse(changing);
                AssertIsFalse(changed);
                AssertIsFalse(unlockRequested);
            });
        }

        private void PerformChecksForModelElement(NMF.Models.Meta.IReference reference, IModelElement element, IModelElement other)
        {
            if (reference.IsContainment)
            {
                AssertAreEqual(element, other.Parent);
            }
            if (reference.Opposite != null)
            {
                if (reference.Opposite.UpperBound == 1)
                {
                    AssertAreEqual(element, other.GetReferencedElement(reference.Opposite));
                }
                else
                {
                    AssertIsTrue(other.GetReferencedElements(reference.Opposite).Cast<IModelElement>().Contains(element));
                }
            }
        }


        #endregion
    }
}
