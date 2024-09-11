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
    /// <summary>
    /// Facade class to help with UML interoperability
    /// </summary>
    public static class UmlInterop
    {
        private static Cmof2NMeta _cmofTransformation = new Cmof2NMeta();
        private static Uml2NMeta _umlTransformation = new Uml2NMeta();
        private static LegacyCmof2NMeta _legacyCmofTransformation = new LegacyCmof2NMeta();

        /// <summary>
        /// Transforms the given CMOF package into a NMeta namespace
        /// </summary>
        /// <param name="cmofPackage">the CMOF package</param>
        /// <param name="additionalPackageRegistry">a callback that is called for additional packages</param>
        /// <returns>an NMeta namespace</returns>
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

        /// <summary>
        /// Transforms the given UML package into a NMeta namespace
        /// </summary>
        /// <param name="umlPackage">the CMOF package</param>
        /// <param name="additionalPackageRegistry">a callback that is called for additional packages</param>
        /// <returns>an NMeta namespace</returns>
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

        /// <summary>
        /// Transforms the given legacy CMOF package into a NMeta namespace
        /// </summary>
        /// <param name="cmofPackage">the legacy CMOF package</param>
        /// <param name="additionalPackageRegistry">a callback that is called for additional packages</param>
        /// <returns>an NMeta namespace</returns>
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
