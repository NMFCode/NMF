﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;
using NMF.Models.Meta;

namespace NMF.Models
{
    /// <summary>
    /// Denotes the abstract base class for an extension (aka stereotype)
    /// </summary>
    [ModelRepresentationClassAttribute("http://nmf.codeplex.com/nmeta/#//ModelElementExtension/")]
    public abstract class ModelElementExtension : ModelElement, IModelElementExtension
    {
        /// <summary>
        /// Gets the actual extension
        /// </summary>
        /// <returns></returns>
        public abstract IExtension GetExtension();

        IModelElement IModelElementExtension.ExtendedElement
        {
            get { return Parent; }
            set { }
        }

        /// <inheritdoc />
        public override Meta.IClass GetClass()
        {
            return Parent.GetClass();
        }
    }

    public abstract class ModelElementExtension<T, T2> : ModelElementExtension where T : IModelElement where T2 : ModelElementExtension<T, T2>
    {
        public static implicit operator T(ModelElementExtension<T, T2> extension)
        {
            if (extension == null) return default(T);
            return (T)extension.Parent;
        }

        public static implicit operator ModelElementExtension<T, T2>(T element)
        {
            var me = element as ModelElement;
            return me?.GetExtension<ModelElementExtension<T, T2>>();
        }
    }
}
