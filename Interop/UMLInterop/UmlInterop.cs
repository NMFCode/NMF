using NMF.Interop.Transformations;
using NMF.Models;
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
        private static LegacyCmof2NMeta _legacyCmofTransformation = new LegacyCmof2NMeta();

        public static INamespace Transform(Cmof.IPackage cmofPackage, Action<Cmof.IPackage, INamespace> additionalPackageRegistry = null)
        {
            var model = new Model();
            var context = new TransformationContext(_cmofTransformation);
            var startRule = _cmofTransformation.Rule<Cmof2NMeta.Package2Namespace>();
            var rootPackage = TransformationEngine.Transform(cmofPackage, context, startRule);
            model.RootElements.Add(rootPackage);
            Uri modelUri;
            if (Uri.TryCreate(cmofPackage.URI, UriKind.Absolute, out modelUri))
            {
                model.ModelUri = modelUri;
            }
            if (additionalPackageRegistry != null)
            {
                foreach (var packageT in context.Trace.TraceAllIn(startRule))
                {
                    var pack = packageT.GetInput(0) as Cmof.IPackage;
                    if (pack != cmofPackage && pack.Parent == null)
                    {
                        additionalPackageRegistry(pack, (INamespace)packageT.Output);
                    }
                }
            }
            return rootPackage;
        }

        public static INamespace Transform(Uml.IPackage umlPackage, Action<Uml.IPackage, INamespace> additionalPackageRegistry = null)
        {
            var model = new Model();
            var context = new TransformationContext(_umlTransformation);
            var startRule = _umlTransformation.Rule<Uml2NMeta.Package2Namespace>();
            var rootPackage = TransformationEngine.Transform(umlPackage, context, startRule);
            model.RootElements.Add(rootPackage);
            Uri modelUri;
            if (Uri.TryCreate(umlPackage.URI, UriKind.Absolute, out modelUri))
            {
                model.ModelUri = modelUri;
            }
            if (additionalPackageRegistry != null)
            {
                foreach (var packageT in context.Trace.TraceAllIn(startRule))
                {
                    var pack = packageT.GetInput(0) as Uml.IPackage;
                    if (pack != umlPackage && pack.Parent == null)
                    {
                        additionalPackageRegistry(pack, (INamespace)packageT.Output);
                    }
                }
            }
            return rootPackage;
        }

        public static INamespace Transform(Legacy.Cmof.IPackage cmofPackage, Action<Legacy.Cmof.IPackage, INamespace> additionalPackageRegistry = null)
        {
            var model = new Model();
            var context = new TransformationContext(_legacyCmofTransformation);
            var startRule = _legacyCmofTransformation.Rule<LegacyCmof2NMeta.Package2Namespace>();
            var rootPackage = TransformationEngine.Transform(cmofPackage, context, startRule);
            model.RootElements.Add(rootPackage);
            Uri modelUri;
            if (Uri.TryCreate(cmofPackage.URI, UriKind.Absolute, out modelUri))
            {
                model.ModelUri = modelUri;
            }
            if (additionalPackageRegistry != null)
            {
                foreach (var packageT in context.Trace.TraceAllIn(startRule))
                {
                    var pack = packageT.GetInput(0) as Legacy.Cmof.IPackage;
                    if (pack != cmofPackage && pack.Parent == null)
                    {
                        additionalPackageRegistry(pack, (INamespace)packageT.Output);
                    }
                }
            }
            return rootPackage;
        }
    }
}
