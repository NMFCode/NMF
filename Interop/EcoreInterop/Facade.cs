using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NMF.Interop.Ecore.Transformations;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Models.Meta;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Utilities;

namespace NMF.Interop.Ecore
{
    /// <summary>
    /// Denotes a static helper class for Ecore interoperability
    /// </summary>
    public static class EcoreInterop
    {
        private static readonly ModelRepository repository = new ModelRepository();
        private static readonly Ecore2MetaTransformation ecore2Meta = new Ecore2MetaTransformation();

        static EcoreInterop()
        {
            using (var ecoreModel = typeof(EcoreInterop).Assembly.GetManifestResourceStream("NMF.Interop.Ecore.Ecore.ecore"))
            {
                repository.Serializer.Deserialize(ecoreModel, new Uri("http://www.eclipse.org/emf/2002/Ecore", UriKind.Absolute), repository, true);
            }
            using (var layoutModel = typeof(EcoreInterop).Assembly.GetManifestResourceStream("NMF.Interop.Ecore.layout.ecore"))
            {
                repository.Serializer.Deserialize(layoutModel, new Uri("platform:/plugin/org.emftext.commons.layout/metamodel/layout.ecore", UriKind.Absolute), repository, true);
            }
            using (var typeModel = typeof(EcoreInterop).Assembly.GetManifestResourceStream("NMF.Interop.Ecore.XMLType.ecore"))
            {
                repository.Serializer.Deserialize(typeModel, new Uri("http://www.eclipse.org/emf/2003/XMLType", UriKind.Absolute), repository, true);
            }
        }

        /// <summary>
        /// Gets the repository with Ecore Interop types
        /// </summary>
        public static IModelRepository Repository
        {
            get
            {
                return repository;
            }
        }

        /// <summary>
        /// Loads an Ecore package from the given file
        /// </summary>
        /// <param name="path">the file path</param>
        /// <returns>The Ecore package</returns>
        public static EPackage LoadPackageFromFile(string path)
        {
            var fileInfo = new FileInfo(path);
            return LoadPackageFromUri(new Uri(fileInfo.FullName));
        }

        /// <summary>
        /// Loads an Ecore package from the given URI
        /// </summary>
        /// <param name="uri">the URI</param>
        /// <returns>The Ecore package</returns>
        public static EPackage LoadPackageFromUri(Uri uri)
        {
            var tempRepository = new ModelRepository(repository);
            var modelElement = tempRepository.Resolve(uri);

            if (modelElement is EPackage package) return package;

            return modelElement.Model.RootElements.OfType<EPackage>().FirstOrDefault();
        }

        /// <summary>
        /// Transforms the given Ecore package to NMeta
        /// </summary>
        /// <param name="package">the Ecore package</param>
        /// <param name="additionalPackageRegistry">a callback when new packages are found</param>
        /// <returns>An NMeta namespace</returns>
        public static INamespace Transform2Meta(EPackage package, Action<IEPackage, INamespace> additionalPackageRegistry = null)
        {
            var model = new Model();
            var context = new TransformationContext(ecore2Meta);
            var rootPackage = TransformationEngine.Transform<IEPackage, INamespace>(package, context);
            model.RootElements.Add(rootPackage);
            if (Uri.TryCreate(package.NsURI, UriKind.Absolute, out Uri modelUri))
            {
                model.ModelUri = modelUri;
            }
            if (additionalPackageRegistry != null)
            {
                foreach (var packageT in context.Trace.TraceAllIn(ecore2Meta.Rule<Ecore2MetaTransformation.EPackage2Namespace>()))
                {
                    var pack = packageT.GetInput(0) as IEPackage;
                    if (pack != package && pack.ESuperPackage == null)
                    {
                        additionalPackageRegistry(pack, (INamespace)packageT.Output);
                    }
                }
            }
            return rootPackage;
        }

        /// <summary>
        /// Transforms the given Ecore packages to NMeta
        /// </summary>
        /// <param name="packages">the Ecore packages</param>
        /// <returns>An NMeta namespace</returns>
        public static IEnumerable<INamespace> Transform2Meta(IEnumerable<EPackage> packages)
        {
            var model = new Model();
            var rootPackages = TransformationEngine.TransformMany<IEPackage, INamespace>(packages, ecore2Meta);
            model.RootElements.AddRange(rootPackages);
            if (packages.Any() && Uri.TryCreate(packages.First().NsURI, UriKind.Absolute, out Uri modelUri))
            {
                model.ModelUri = modelUri;
            }
            return rootPackages;
        }

    }
}
