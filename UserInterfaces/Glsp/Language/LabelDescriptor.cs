using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes a descriptor for elements shown as labels
    /// </summary>
    public abstract class LabelDescriptor<T> : ElementDescriptor<T>
    {
        /// <inheritdoc />
        protected internal override IEnumerable<TypeHint> CalculateTypeHints()
        {
            yield return new ShapeTypeHint
            {
                Deletable = true,
                ElementTypeId = ElementTypeId,
                Reparentable = true,
                Repositionable = false,
                Resizable = false
            };
        }

        internal override GElementSkeleton<T> CreateSkeleton()
        {
            return new GLabelSkeleton<T>(this);
        }

        /// <summary>
        /// Specifies that a GLabel element should be created for the current element
        /// </summary>
        /// <param name="labelSelector">An expression calculating the text of the label</param>
        /// <param name="canEdit">True, if the label can be added, otherwise False</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected ILabelSyntax<T> Label(Expression<Func<T, string>> labelSelector, bool canEdit = true) 
        {
            var labelSkeleton = (GLabelSkeleton<T>)CurrentSkeleton;
            labelSkeleton.LabelValue = labelSelector;
            labelSkeleton.CanEdit = canEdit;
            return new LabelSyntax(labelSkeleton);
        }

        private class LabelSyntax : ILabelSyntax<T>
        {
            private readonly GLabelSkeleton<T> _skeleton;

            public LabelSyntax(GLabelSkeleton<T> skeleton)
            {
                _skeleton = skeleton;
            }

            public ILabelSyntax<T> If(Expression<Func<T, bool>> guard)
            {
                throw new NotImplementedException();
            }

            public ILabelSyntax<T> Validate(Func<T, string, ValidationStatus> validator)
            {
                _skeleton.Validator = validator;
                return this;
            }

            public ILabelSyntax<T> WithSetter(Action<T, string> setter)
            {
                _skeleton.CustomSetter = setter;
                _skeleton.CanEdit = setter != null;
                return this;
            }

            public ILabelSyntax<T> WithType(string type)
            {
                _skeleton.Type = type;
                return this;
            }
        }
    }
}
