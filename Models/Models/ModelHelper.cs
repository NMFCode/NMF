using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NMF.Models
{
    public static class ModelHelper
    {
        /// <summary>
        /// Creates a Uri for an ordered reference where the child element has the given index
        /// </summary>
        /// <param name="reference">The reference of the child element</param>
        /// <param name="index">The index inside the child element</param>
        /// <returns>A relative Uri for the reference with the given index</returns>
        public static string CreatePath(string reference, int index)
        {
            if (reference == null) return index.ToString();
            return "@" + reference + "." + index.ToString();
        }

        /// <summary>
        /// Creates a relative Uri for the given reference name
        /// </summary>
        /// <param name="reference">The name of the reference</param>
        /// <returns>A relative Uri for the given reference</returns>
        public static string CreatePath(string reference)
        {
            return "@" + reference;
        }

        /// <summary>
        /// Gets the index of the given element inside the given reference or -1 if the element is not contained in the reference
        /// </summary>
        /// <typeparam name="T">The element type for the reference</typeparam>
        /// <param name="reference">The reference list</param>
        /// <param name="element">The element that should be looked for</param>
        /// <returns>The index of the element or -1, if the reference does not contain the given element</returns>
        public static int IndexOfReference<T>(IList<T> reference, IModelElement element) where T : class, IModelElement
        {
            if (reference == null) return -1;
            if (element is T casted)
            {
                return reference.IndexOf(casted);
            }
            else
            {
                return -1;
            }
        }

        private static readonly Regex uriSegmentRegex = new Regex(@"^(.+[\._])?(?<Number>\d+)$", RegexOptions.Compiled);
        private static readonly Regex identifierRegex = new Regex(@"^(?<Reference>\w+)\[(?<IdReference>\w+)\s*=\s*'(?<Identifier>[^']+)'\]$", RegexOptions.Compiled);

        /// <summary>
        /// Parses the given relative Uri
        /// </summary>
        /// <param name="segment">the relative Uri</param>
        /// <param name="reference">The reference that corresponds to the Uri. This is never null.</param>
        /// <param name="index">The element index of the Uri or 0 if no index is given</param>
        /// <returns>True, if the segment was parsed successfully, otherwise false</returns>
        public static bool ParseSegment(string segment, out string reference, out int index)
        {
            reference = string.Empty;
            index = 0;
            if( segment == null ) return true;
            if (segment.StartsWith("@")) segment = segment.Substring(1);
            var match = uriSegmentRegex.Match(segment);
            if (match.Success)
            {
                var number = match.Groups["Number"];
                if (number.Index > 0)
                {
                    reference = segment.Substring(0, number.Index - 1);
                    index = int.Parse(number.Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    reference = "#";
                    index = int.Parse(number.Value, CultureInfo.InvariantCulture);
                }
            }
            else
            {
                reference = segment;
            }
            reference = reference.ToUpperInvariant();
            return !segment.Contains( '[' );
        }

        /// <summary>
        /// Parses the given relative Uri for a identifier reference
        /// </summary>
        /// <param name="segment">The relative Uri</param>
        /// <param name="reference">The reference that corresponds to the Uri. This is never null.</param>
        /// <param name="identifierReference">The identifier reference. This is never null.</param>
        /// <param name="identifier">The identifier. This is never null.</param>
        /// <returns>True, if the segment was parsed successfully, otherwise false</returns>
        public static bool ParseIdentifierSegment( string segment, out string reference, out string identifierReference, out string identifier )
        {
            reference = string.Empty;
            identifierReference = string.Empty;
            identifier = string.Empty;
            if(segment == null) return false;
            if(segment.StartsWith("@")) segment = segment.Substring(1);
            var match = identifierRegex.Match(segment);
            if( match.Success )
            {
                reference = match.Groups["Reference"].Value;
                identifierReference = match.Groups["IdReference"].Value;
                identifier = match.Groups["Identifier"].Value;
            }
            reference = reference.ToUpperInvariant();
            identifierReference = identifierReference.ToUpperInvariant();
            return match.Success;
        }

        public static T Parse<T>(string text)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter == null) return default(T);
            return (T)converter.ConvertFromInvariantString(text);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T CastAs<T>(object item) where T : class
        {
            return item as T;
        }
    }
}
