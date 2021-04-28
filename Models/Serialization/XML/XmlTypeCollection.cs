using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace NMF.Serialization
{
    /// <summary>
    /// Defines a collection of types known to a XmlSerializer
    /// </summary>
    /// <remarks>This collection is only a wrapper for an internal collection of type serialization information</remarks>
    public sealed class XmlTypeCollection : ICollection<Type>
    {

        private readonly XmlSerializer mSerializer;

        /// <summary>
        /// Creates a new typecollection for the given XmlSerializer
        /// </summary>
        /// <param name="serializer">The XmlSerializer that will work with this set of types</param>
        public XmlTypeCollection(XmlSerializer serializer)
        {
            mSerializer = serializer;
            if (serializer == null) throw new ArgumentNullException("serializer");
        }
        
        /// <summary>
        /// The corresponding XmlSerializer
        /// </summary>
        public XmlSerializer Serializer
        {
            get
            {
                return mSerializer;
            }
        }

        /// <summary>
        /// Imports the given type to the XmlSerializer
        /// </summary>
        /// <param name="type">The type to import</param>
        /// <remarks>Note that importing a type will also import all the property types of this type, if they aren't already imported</remarks>
        public void Add(Type type)
        {
            if (!Serializer.Types.ContainsKey(type)) Serializer.GetSerializationInfo(type, true);
        }

        /// <summary>
        /// Clears the set of types known to the XmlSerializer
        /// </summary>
        public void Clear()
        {
            Serializer.Types.Clear();
        }

        /// <summary>
        /// Gets a value that indicates whether the given type is already known to the XmlSerializer
        /// </summary>
        /// <param name="item">The type to look for</param>
        /// <returns>True, if the type is already known to the XmlSerializer, otherwise False</returns>
        public bool Contains(Type item)
        {
            return Serializer.Types.ContainsKey(item);
        }

        /// <summary>
        /// Copies the known types into an array
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The destination start index</param>
        public void CopyTo(Type[] array, int arrayIndex)
        {
            foreach (XmlTypeSerializationInfo t in Serializer.Types.Values)
            {
                array[arrayIndex] = t.Type;
                arrayIndex += 1;
            }
        }

        /// <summary>
        /// Gives the amount of types known to the XmlSerializer
        /// </summary>
        public int Count
        {
            get
            {
                return Serializer.Types.Count;
            }
        }

        bool ICollection<Type>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the given type from the set
        /// </summary>
        /// <param name="item">The type to remove</param>
        /// <returns>A value indicating whether removal was successful</returns>
        public bool Remove(Type item)
        {
            return Serializer.Types.Remove(item);
        }

        /// <summary>
        /// Gets an enumerator to go through the collection
        /// </summary>
        /// <returns>An IEnumerator object</returns>
        public IEnumerator<Type> GetEnumerator()
        {
            return new Enumerator(Serializer.Types.Values.OfType<XmlTypeSerializationInfo>().GetEnumerator());
        }

        private struct Enumerator : IEnumerator<Type>
        {
            readonly IEnumerator<XmlTypeSerializationInfo> enu;

            public Enumerator(IEnumerator<XmlTypeSerializationInfo> enu)
            {
                this.enu = enu;
            }

            #region IEnumerator<Type> Members

            public Type Current
            {
                get { return enu.Current.Type; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            { enu.Dispose(); }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return enu.MoveNext();
            }

            public void Reset()
            {
                enu.Reset();
            }

            #endregion
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
