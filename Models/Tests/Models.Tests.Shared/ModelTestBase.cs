using NMF.Collections.ObjectModel;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Models.Security;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Models.Tests.Shared
{
    /// <summary>
    /// Denotes an abstract base class for implicit model tests
    /// </summary>
    /// <typeparam name="T">the type that is being tested</typeparam>
    public abstract class ModelTestBase<T> where T : IModelElement, new()
    {
        /// <summary>
        /// Gets the class instance for the test
        /// </summary>
        protected readonly IClass Class;
        /// <summary>
        /// Denotes a collection if ancestors of this class
        /// </summary>
        protected readonly IEnumerable<IClass> AllAncestors;

        /// <summary>
        /// Denotes a collection of attributes with fixed values
        /// </summary>
        protected readonly HashSet<IAttribute> fixedAttributes = new HashSet<IAttribute>();

        /// <summary>
        /// Gets a lookup which reference is refined by other references
        /// </summary>
        protected readonly Dictionary<IReference, IReference> refinesLookup = new Dictionary<IReference, IReference>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected ModelTestBase()
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

        /// <summary>
        /// Asserts that the condition is true
        /// </summary>
        /// <param name="condition">The condition</param>
        /// <param name="message">The message if not true</param>
        protected abstract void AssertIsTrue(bool condition, string message = null);

        /// <summary>
        /// Asserts that the condition is false
        /// </summary>
        /// <param name="condition">The condition</param>
        /// <param name="message">The message if not false</param>
        protected abstract void AssertIsFalse(bool condition, string message = null);

        /// <summary>
        /// Asserts that executing the action will throw the given type of exception
        /// </summary>
        /// <typeparam name="TException">The expected exception type</typeparam>
        /// <param name="toPerform">The action that should be performed in the test</param>
        /// <param name="message">The error message when a different exception or no exception is thrown</param>
        protected abstract void AssertThrowsException<TException>(Action toPerform, string message = null)
            where TException : Exception;

        /// <summary>
        /// Asserts that the given actual value equals the expected value
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The error message</param>
        protected abstract void AssertAreEqual(object expected, object actual, string message = null);

        #endregion

        #region Helper Methods

        /// <summary>
        /// Checks the action for all attributes
        /// </summary>
        /// <param name="toCheck">The check that should be performed</param>
        /// <param name="task">The name of the test</param>
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

        /// <summary>
        /// Determines whether the given attribute should be included in the given test
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <param name="task">The test name</param>
        /// <returns>True, if the attribute should be considered, otherwise false</returns>
        protected virtual bool ShouldCheckAttribute(IAttribute attribute, string task)
        {
            return true;
        }

        /// <summary>
        /// Checks the given action for all references
        /// </summary>
        /// <param name="toCheck">The check that should be performed</param>
        /// <param name="task">The name of the test to be executed</param>
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

        /// <summary>
        /// Determines whether the test should include the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <param name="task">The name of the test</param>
        /// <returns>True, if the reference should be considered, otherwise false</returns>
        protected virtual bool ShouldCheckReference(IReference reference, string task)
        {
            return true;
        }

        /// <summary>
        /// Performs the given check for all containments
        /// </summary>
        /// <param name="toCheck">The check to be performed</param>
        /// <param name="task">The name of the test</param>
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

        /// <summary>
        /// Gets the primary value for the given feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>A value</returns>
        protected virtual object GetPrimaryValue(ITypedElement feature)
        {
            return GetValue(feature, false);
        }

        /// <summary>
        /// Gets a secondary value for the given feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>A value different than the one returned by GetPrimaryValue</returns>
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
                type = reference.Type ?? ModelElement.ClassInstance;
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

        /// <summary>
        /// Tests that model elements can be resolved by a relative Uri
        /// </summary>
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

        /// <summary>
        /// Tests that attributes can change when the element is in normal state. This shall lead to change events to be raised.
        /// </summary>
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
        
        /// <summary>
        /// Tests that when the model element is locked, attempting to change an attribute leads to a request to unlock the model element
        /// </summary>
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
        
        /// <summary>
        /// Tests that if an attribute is attempted to change while the element is locked and the unlock is refused, an exception occurs
        /// </summary>
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
        
        /// <summary>
        /// Tests that when the model element is fixed, any attempt to change a value throws an exception
        /// </summary>
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
        
        /// <summary>
        /// Tests that reference changes succeed when the model element is in normal state and change events are raised appropriately
        /// </summary>
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
        
        /// <summary>
        /// Tests that reference changes succeed if the model element is locked and the unlock request is granted
        /// </summary>
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
        
        /// <summary>
        /// Tests that an attempt to change a reference while a model element is locked and the unlock request is denied leads to an exception
        /// </summary>
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
        
        /// <summary>
        /// Tests that any changes to references will fail if the element is fixed
        /// </summary>
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

        /// <summary>
        /// Tests that if the model element is serialized and deserialized again, the hash of the model does not change
        /// </summary>
        public virtual void SerializationRoundtrip_KeepsModelHash()
        {
            var model = new Model();
            model.ModelUri = new Uri("foo:bar");
            var element = new T();
            model.RootElements.Add(element);
            var serializer = new ModelSerializer();
            CheckForAllAttributes(att =>
            {
                if (att.UpperBound == 1)
                {
                    element.SetAttributeValue(att, GetPrimaryValue(att));
                }
                else
                {
                    element.GetAttributeValues(att).Add(GetPrimaryValue(att));
                }
            });
            CheckForAllReferences(reference =>
            {
                if (reference.Opposite != null && reference.Opposite.IsContainment)
                {
                    return;
                }
                var referencedValue = GetPrimaryValue(reference) as IModelElement;
                if (reference.UpperBound == 1)
                {
                    element.SetReferencedElement(reference, referencedValue);
                }
                else
                {
                    element.GetReferencedElements(reference).Add(referencedValue);
                }
                if (!reference.IsContainment)
                {
                    model.RootElements.Add(referencedValue);
                }
            });

            var hash = Convert.ToBase64String(ModelHasher.CreateHash(element));

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(model, ms);
                ms.Position = 0;
                var repo = new ModelRepository();
                var deserializedModel = serializer.Deserialize(ms, model.ModelUri, repo, false);

                var deserializedHash = Convert.ToBase64String(ModelHasher.CreateHash(element));
                AssertAreEqual(hash, deserializedHash, "The hashes of the original model element and the one after serialization roundtrip do not coincide.");
            }
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
