using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;
using System.CodeDom;

namespace NMF.CodeGen
{
    /// <summary>
    /// Represents a transformation rule that generates enumerations
    /// </summary>
    /// <typeparam name="T">The model element type from which to generate enumerations</typeparam>
    public abstract class EnumGenerator<T> : TransformationRule<T, CodeTypeDeclaration>
        where T : class
    {
        /// <summary>
        /// Represents an enumeration member
        /// </summary>
        protected struct EnumMember
        {
            /// <summary>
            /// The value of the enumeration literal
            /// </summary>
            public int? Value { get; set; }
            
            /// <summary>
            /// The name of the enumeration literal
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The documentation summary of the literal
            /// </summary>
            public string Summary { get; set; }

            /// <summary>
            /// The documentation remarks of the literal
            /// </summary>
            public string Remarks { get; set; }
        }

        /// <summary>
        /// Gets the enumeration literals that should be generated for the given input model element
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <returns>A collection of enumeration members</returns>
        protected abstract IEnumerable<EnumMember> GetMembers(T input);

        /// <summary>
        /// Gets the name of the enumeration class
        /// </summary>
        /// <param name="input">The input model element from which to generate the enumeration class</param>
        /// <returns>The name of the enumeration</returns>
        protected abstract string GetName(T input);

        /// <summary>
        /// Gets a value indicating whether the enumeration is flagged
        /// </summary>
        /// <param name="input">The input model element from which to generate the model</param>
        /// <returns>True, if the enumeration is flagged, otherwise false</returns>
        protected virtual bool GetIsFlagged(T input) { return false; }

        /// <summary>
        /// Transform the input model element to an enumeration
        /// </summary>
        /// <param name="input">The input model element that is transformed to an enumeration</param>
        /// <param name="output">The output code type declaration that represents an enumeration</param>
        /// <param name="context">The transformation context</param>
        public override void Transform(T input, CodeTypeDeclaration output, Transformations.Core.ITransformationContext context)
        {
            var flagged = GetIsFlagged(input);

            if (flagged)
            {
                output.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(FlagsAttribute).Name));
            }

            int nextValue = flagged ? 1 : 0;
            foreach (var item in GetMembers(input))
            {
                if (item.Value.HasValue) nextValue = item.Value.Value;
                var literal = new CodeMemberField()
                {
                    Name = item.Name,
                    InitExpression = new CodePrimitiveExpression(nextValue)
                };
                if (item.Summary != null || item.Remarks != null)
                {
                    string comment = string.Empty;
                    if (!string.IsNullOrEmpty(item.Summary))
                    {
                        comment += "<summary>\r\n" + item.Summary + "\r\n</summary>";
                    }
                    if (!string.IsNullOrEmpty(item.Remarks))
                    {
                        comment += "\r\n<remarks>" + item.Remarks + "</remarks>";
                    }
                    literal.Comments.Add(new CodeCommentStatement(comment, true));
                }
                if (flagged)
                {
                    nextValue <<= 1;
                }
                else
                {
                    nextValue++;
                }
                output.Members.Add(literal);
            }
        }

        /// <summary>
        /// Creates the code type declaration for the given input model element
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="context">The transformation context</param>
        /// <returns>The code type declaration that will be the transformation result for the given enumeration</returns>
        public override CodeTypeDeclaration CreateOutput(T input, Transformations.Core.ITransformationContext context)
        {
            var declaration = new CodeTypeDeclaration()
            {
                Name = GetName(input),
                IsEnum = true
            };
            var reference = new CodeTypeReference(declaration.Name);
            CodeDomHelper.SetUserItem(declaration, CodeDomHelper.TypeReferenceKey, reference);
            CodeDomHelper.SetUserItem(reference, CodeDomHelper.ClassKey, declaration);
            return declaration;
        }
    }
}
