using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NMF.Utilities
{
    /// <summary>
    /// This class contains some extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Concatenates a collection with a single element
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="element">The item that should be added to the collection</param>
        /// <returns>A collection containing the source elements and the given element, provided it is not null</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T element)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
            if (element != null)
            {
                yield return element;
            }
        }

        /// <summary>
        /// The C# as operator
        /// </summary>
        /// <typeparam name="T">The type of the cast</typeparam>
        /// <param name="item">The object to cast</param>
        /// <returns>The casted result or null</returns>
        public static T As<T>(this object item) where T : class
        {
            return item as T;
        }

        public static int CombineHash(int hash, int argument, int position)
        {
            unchecked
            {
                return hash ^ (argument << position);
            }
        }

        /// <summary>
        /// Computes all the closure of the given item with the given child-function
        /// </summary>
        /// <typeparam name="T">The type of the items</typeparam>
        /// <param name="item">The root of this closure</param>
        /// <param name="children">A method that selects the child items for an item</param>
        /// <returns>A list of items that form the closure</returns>
        public static IEnumerable<T> Closure<T>(this T item, Func<T, IEnumerable<T>> children)
        {
            if (item == null) return null;
            if (children == null) throw new ArgumentNullException("children");
            var list = new HashSet<T>();
            list.Add(item);
            ComputeClosure<T>(item, children, list);
            return list;
        }

        private static void ComputeClosure<T>(T item, Func<T, IEnumerable<T>> children, HashSet<T> visited)
        {
            if (item != null)
            {
                var childs = children(item);
                if (childs != null)
                {
                    foreach (var child in childs)
                    {
                        if (!visited.Contains(child))
                        {
                            visited.Add(child);
                            ComputeClosure(child, children, visited);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a whole collection to another given collection
        /// </summary>
        /// <typeparam name="T">The type of the items</typeparam>
        /// <param name="collection">The collection where the items should be added to</param>
        /// <param name="items">A collection of items that should be added</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection != null)
            {
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        collection.Add(item);
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("collection");
            }
        }

        /// <summary>
        /// Returns a string representation of the provided uri
        /// </summary>
        /// <param name="uri">The provided uri</param>
        /// <returns>Null, if the uri was null, the absolute Uri if the provided Uri was an absolute Uri or the original string if it has been a relative Uri</returns>
        public static string ConvertToString(this Uri uri)
        {
            if (uri == null) return null;
            if (uri.IsAbsoluteUri) return uri.AbsoluteUri;
            return uri.OriginalString;
        }

        /// <summary>
        /// Removes a whole collection to another given collection
        /// </summary>
        /// <typeparam name="T">The type of the items</typeparam>
        /// <param name="collection">The collection where the items should be removed from</param>
        /// <param name="items">A collection of items that should be removed</param>
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection != null)
            {
                if (items != null)
                {
                    if (collection != items)
                    {
                        if (collection is IList<T> list)
                        {
                            for (int i = list.Count - 1; i >= 0; i--)
                            {
                                var item = list[i];
                                if (items.Contains(item)) list.RemoveAt(i);
                            }
                        }
                        else
                        {
                            foreach (var item in items)
                            {
                                collection.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        collection.Clear();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("collection");
            }
        }

        /// <summary>
        /// Computes the root element according to the given reference
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="node">The current node</param>
        /// <param name="parent">The reference to follow the parent element</param>
        /// <returns>The root of hierarchy</returns>
        public static T Root<T>(this T node, Func<T, T> parent)
        {
            if (node == null) return node;
            if (parent == null) throw new ArgumentNullException("parent");
            var test = parent(node);
            var cur = node;
            while (test != null)
            {
                cur = test;
                test = parent(test);
            }
            return cur;
        }

        /// <summary>
        /// Computes the Depth of a tree
        /// </summary>
        /// <typeparam name="T">The type of the tree elements</typeparam>
        /// <param name="root">The root of the tree</param>
        /// <param name="child">A method that selects the child item of a given parent item</param>
        /// <returns>The depth of the tree</returns>
        /// <remarks>This method will produce a StackOverflowException if the tree contains a cyclus</remarks>
        public static int DepthOfTree<T>(this T root, Func<T, T> child) 
            where T : class
        {
            if (child != null)
            {
                T item = root;
                int depth = 0;
                while (item != null)
                {
                    item = child(item);
                    depth++;
                }
                return depth;
            }
            else
            {
                throw new ArgumentNullException("child");
            }
        }

        /// <summary>
        /// Computes the Depth of a tree
        /// </summary>
        /// <typeparam name="T">The type of the tree elements</typeparam>
        /// <param name="root">The root of the tree</param>
        /// <param name="children">A method that selects the child items of a given parent item</param>
        /// <returns>The depth of the tree</returns>
        /// <remarks>This method will produce a StackOverflowException if the tree contains a cyclus</remarks>
        public static int DepthOfTree<T>(this T root, Func<T, IEnumerable<T>> children)
            where T : class
        {
            if (children != null)
            {
                if (root == null)
                {
                    return 0;
                }
                else
                {
                    IEnumerable<T> childs = children(root);
                    if (childs != null)
                    {
                        if (childs.Contains(root)) throw new InvalidOperationException("The tree contains a circle");
                        if (childs.Count() != 0)
                        {
                            return childs.Max(item => DepthOfTree(item, children)) + 1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("children");
            }
        }

        /// <summary>
        /// Creates a collection of tuples with a constant second tuple item
        /// </summary>
        /// <typeparam name="T1">The type of the tuples first items</typeparam>
        /// <typeparam name="T2">The type of the tuples second items</typeparam>
        /// <param name="collection">A collection of items typed with S</param>
        /// <param name="constant">A constant value that should be paired with the arguments of the collection</param>
        /// <returns>A collection of tuples</returns>
        public static IEnumerable<Tuple<T1, T2>> PairWithConstant<T1, T2>(this IEnumerable<T1> collection, T2 constant)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    yield return new Tuple<T1, T2>(item, constant);
                }
            }
        }

        /// <summary>
        /// Creates a pair with the given two objects
        /// </summary>
        /// <typeparam name="T1">The type of the tuples first item</typeparam>
        /// <typeparam name="T2">The type of the tuples second item</typeparam>
        /// <param name="item">The item that should be used as first argument</param>
        /// <param name="partner">The partner that should be used as second item</param>
        /// <returns>A pair of the two items</returns>
        public static Tuple<T1, T2> PairWith<T1, T2>(this T1 item, T2 partner)
        {
            return new Tuple<T1, T2>(item, partner);
        }

        /// <summary>
        /// Returns whether the given array of inputs can be an instance array of the current type signature
        /// </summary>
        /// <param name="types">The type signature</param>
        /// <param name="inputs">An array of objects that should be tested</param>
        /// <returns>True, if the array of objects can be an array with the current type signature</returns>
        public static bool IsInstanceArrayOfType(this Type[] types, object[] inputs)
        {
            if (types == null || inputs == null || types.Length != inputs.Length) return false;
            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsInstanceOfType(inputs[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether the given array of inputs can be an instance array of the current type signature
        /// </summary>
        /// <param name="types">The type signature</param>
        /// <param name="inputs">An array of objects that should be tested</param>
        /// <param name="allowNullValues">A value indicating whether null values in the array are allowed</param>
        /// <returns>True, if the array of objects can be an array with the current type signature</returns>
        public static bool IsInstanceArrayOfType(this Type[] types, object[] inputs, bool allowNullValues)
        {
            if (types == null || inputs == null || types.Length != inputs.Length) return false;
            for (int i = 0; i < types.Length; i++)
            {
                if ((!allowNullValues || inputs[i] != null) && !types[i].IsInstanceOfType(inputs[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether the instance arrays of the given type signature are also instance arrays of the current signature
        /// </summary>
        /// <param name="types">The type signature that should contain the super types</param>
        /// <param name="inputs">The type signature that contains the more concrete types</param>
        /// <returns>True, if the assertions above hold</returns>
        public static bool IsAssignableArrayFrom(this Type[] types, Type[] inputs)
        {
            if (types == null || inputs == null || types.Length != inputs.Length) return false;
            for (int i = 0; i < types.Length; i++)
            {
                var type = inputs[i];
                if (type == null) return false;
                if (!types[i].IsAssignableFrom(type) && !type.IsInterface) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether all items of the current array equal the items of the given array
        /// </summary>
        /// <typeparam name="T">The type of the items</typeparam>
        /// <param name="items1">The first array</param>
        /// <param name="items2">The second array</param>
        /// <returns>True, if the length of both arrays match and also all items match (using Equals), otherwise False</returns>
        public static bool ArrayEquals<T>(this T[] items1, T[] items2)
        {
            if (items1 == null || items2 == null || items1.Length != items2.Length) return false;
            if (items1 == items2) return true;
            for (int i = 0; i < items1.Length; i++)
            {
                if (!object.Equals(items1[i], items2[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Gets an object that is compared only by the items of the given collection
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>A collection that is treated as equal as soon as all items match</returns>
        public static IEnumerable<T> AsItemEqual<T>(this IEnumerable<T> source)
        {
            return new ItemEqualityArray<T>(source.ToArray());
        }

        /// <summary>
        /// Creates an array of the given array items type objects
        /// </summary>
        /// <param name="objects">An array of objects</param>
        /// <returns>An array of the types of these objects</returns>
        public static Type[] GetTypes(this object[] objects)
        {
            if (objects != null)
            {
                var types = new Type[objects.Length];
                for (int i = 0; i < objects.Length; i++)
                {
                    types[i] = objects[i] != null ? objects[i].GetType() : null;
                }
                return types;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets whether a collection is either null or empty
        /// </summary>
        /// <typeparam name="T">The type of the collection</typeparam>
        /// <param name="collection">The collection that should be tested</param>
        /// <returns>True, if the collection either is a null instance or is empty</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return true;
            if (collection is ICollection<T> coll) return coll.Count == 0;
            using (var enumerator = collection.GetEnumerator())
            {
                return !enumerator.MoveNext();
            }
        }

        private static string ToOtherCase(string input, bool firstUpper, bool leaveUpper, char[] allowedSpecialCharacters)
        {
            var sb = new StringBuilder();
            var nextUpper = false;
            int start = 0;
            while (start < input.Length && !char.IsLetter(input[start]))
            {
                if (input[start] == '_')
                {
                    sb.Append('_');
                }
                start++;
            }
            if (start >= input.Length) return string.Empty;
            if (firstUpper)
            {
                sb.Append(char.ToUpper(input[start]));
            }
            else
            {
                sb.Append(char.ToLower(input[start]));
            }
            for (int i = start+1; i < input.Length; i++)
            {
                var ch = input[i];
                if (char.IsLetterOrDigit(ch) || ch == '_')
                {
                    if (nextUpper)
                    {
                        sb.Append(char.ToUpper(ch, CultureInfo.CurrentCulture));
                        nextUpper = false;
                    }
                    else
                    {
                        if (leaveUpper)
                        {
                            sb.Append(ch);
                        }
                        else
                        {
                            sb.Append(char.ToLower(ch, CultureInfo.CurrentCulture));
                        }
                    }
                }
                else if (allowedSpecialCharacters != null && allowedSpecialCharacters.Contains(ch))
                {
                    sb.Append(ch);
                    nextUpper = firstUpper;
                }
                else
                {
                    nextUpper = true;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the implementation type of the given type
        /// </summary>
        /// <param name="type">The current type</param>
        /// <returns>The implementation type. For classes, the class itself will be returned. For interfaces, a possible annotated default implementation type will be returned.</returns>
        public static Type GetImplementationType(this Type type)
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

        /// <summary>
        /// Converts the given string into a string of camel case format to be used as name for e.g. parameters or local variables
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="allowedSpecialCharacters">An array of allowed special characters</param>
        /// <returns>The input string converted to camel case</returns>
        public static string ToCamelCase(this string input, params char[] allowedSpecialCharacters)
        {
            if (input == null) return null;
            return ToOtherCase(input, false, !input.Contains(' '), allowedSpecialCharacters);
        }

        /// <summary>
        /// Converts the given string into a string of Pascal case format to be used as name for e.g. methods or classes
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="allowedSpecialCharacters">An array of allowed special characters</param>
        /// <returns>The input string converted to Pascal case</returns>
        public static string ToPascalCase(this string input, params char[] allowedSpecialCharacters)
        {
            if (input == null) return null;
            return ToOtherCase(input, true, !input.Contains(' '), allowedSpecialCharacters);
        }

        /// <summary>
        /// Determines whether the given set is a superset of the given collection
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="items">The current collection</param>
        /// <param name="other">The other collection that is a prospective superset</param>
        /// <returns>True, if the given collection is a superset of the current collection, otherwise false</returns>
        public static bool IsSupersetOf<T>(this IEnumerable<T> items, IEnumerable<T> other)
        {
            return other.IsSubsetOf(items);
        }

        /// <summary>
        /// Determines whether the given set is a subset of the given collection
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="items">The current collection</param>
        /// <param name="other">The other collection that is a prospective subset</param>
        /// <returns>True, if the given collection is a subset of the current collection, otherwise false</returns>
        public static bool IsSubsetOf<T>(this IEnumerable<T> items, IEnumerable<T> other)
        {
            if (items == null) return true;
            if (other == null) return false;
            foreach (var item in items)
            {
                if (!other.Contains(item)) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns collection that contains the items of the current collection except the given element
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection</typeparam>
        /// <param name="items">The current collection that should be used as default</param>
        /// <param name="exception">The element that should not be contained in the returned collection</param>
        /// <returns>A collection that contains all elements of the current collection except the given element (including duplicates)</returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T exception)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (!object.Equals(item, exception)) yield return item;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current collection and the given collection have an intersection
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="items">The current collection</param>
        /// <param name="other">The other collection that should be checked for intersections</param>
        /// <returns>True, of both collections have a common intersection, otherwise false</returns>
        public static bool IntersectsWith<T>(this ICollection<T> items, IEnumerable<T> other)
        {
            if (other == null) return false;
            if (items == null) return false;
            foreach (var item in other)
            {
                if (items.Contains(item)) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the current collection and the given collection have an intersection
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="items">The current collection</param>
        /// <param name="other">The other collection that should be checked for intersections</param>
        /// <returns>True, of both collections have a common intersection, otherwise false</returns>
        public static bool IntersectsWith<T>(this IEnumerable<T> items, IEnumerable<T> other)
        {
            if (items is ICollection<T> coll) return IntersectsWith(coll, other);
            coll = other as ICollection<T>;
            if (coll != null) return IntersectsWith(coll, items);

            if (other == null) return false;
            if (items == null) return false;

            foreach (var item in other)
            {
                if (items.Contains(item)) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the current collection and the other collections contain the same set of elements
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="items">The current collection</param>
        /// <param name="other">The other collection that should be checked</param>
        /// <returns>True, of both collections have the same items, otherwise false</returns>
        public static bool SetEquals<T>(this ICollection<T> items, IEnumerable<T> other)
        {
            if (items == other) return true;
            if (other == null) return items.IsNullOrEmpty();
            if (items == null) return other.IsNullOrEmpty();
            var counter = 0;
            foreach (var item in other)
            {
                if (!items.Contains(item)) return false;
                counter++;
            }
            if (items.Count > counter) return false;
            return items.Distinct().Count() == counter;
        }

        /// <summary>
        /// Flattens a tree structure by recursively projecting all elements to a collection of children.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="root">The root of the tree</param>
        /// <param name="childSelector">A function which selects the children for an element</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectRecursive<T>(T root, Func<T, IEnumerable<T>> childSelector)
        {
            return SelectRecursive(childSelector(root), childSelector);
        }

        /// <summary>
        /// Flattens a tree structure by recursively projecting all elements to a collection of children.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="collection">The root elements to start with</param>
        /// <param name="childSelector">A function which selects the children for an element</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> collection, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(collection);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                foreach (var child in childSelector(current))
                    stack.Push(child);
                yield return current;
            }
        }
    }

    /// <summary>
    /// Represents a value augmented by a flag
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public struct FlaggedValue<T>
    {
        /// <summary>
        /// Gets the value represented by this struct
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Indicates whether the value is flagged
        /// </summary>
        public bool IsFlagged { get; private set; }

        public FlaggedValue(T value, bool flag) : this()
        {
            Value = value;
            IsFlagged = flag;
        }
    }
}
