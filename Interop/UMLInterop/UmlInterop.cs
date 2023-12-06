using NMF.Interop.Transformations;
using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Interop
{
    public static class UmlInterop
    {
        private static Cmof2NMeta _cmofTransformation = new Cmof2NMeta();
        private static Uml2NMeta _umlTransformation = new Uml2NMeta();

        public static INamespace Transform(Cmof.IPackage cmofPackage)
        {
            return TransformationEngine.Transform(cmofPackage, new TransformationContext(_cmofTransformation), _cmofTransformation.Rule<Cmof2NMeta.Package2Namespace>());
        }

        public static INamespace Transform(Uml.IPackage umlPackage)
        {
            return TransformationEngine.Transform(umlPackage, new TransformationContext(_umlTransformation), _umlTransformation.Rule<Uml2NMeta.Package2Namespace>());
        }
    }
}
