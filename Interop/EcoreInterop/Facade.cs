using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using NMF.Interop.Ecore.Transformations;
using NMF.Transformations;
using System.Reflection;
using NMF.Transformations.Core;
using NMF.Models.Meta;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Utilities;

namespace NMF.Interop.Ecore
{
    public static class EcoreInterop
    {
        private static ModelRepository repository = new ModelRepository();
        private static Ecore2MetaTransformation ecore2Meta = new Ecore2MetaTransformation();

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
        }

        public static IModelRepository Repository
        {
            get
            {
                return repository;
            }
        }

        public static EPackage LoadPackageFromFile(string path)
        {
            var fileInfo = new FileInfo(path);
            return LoadPackageFromUri(new Uri(fileInfo.FullName));
        }

        public static EPackage LoadPackageFromUri(Uri uri)
        {
            var tempRepository = new ModelRepository(repository);
            var modelElement = tempRepository.Resolve(uri);

            var package = modelElement as EPackage;
            if (package != null) return package;

            return modelElement.Model.RootElements.OfType<EPackage>().FirstOrDefault();
        }

        public static INamespace Transform2Meta(EPackage package)
        {
            var model = new Model();
            var rootPackage = TransformationEngine.Transform<IEPackage, INamespace>(package, ecore2Meta);
            model.RootElements.Add(rootPackage);
            Uri modelUri;
            if (Uri.TryCreate(package.NsURI, UriKind.Absolute, out modelUri))
            {
                model.ModelUri = modelUri;
            }
            return rootPackage;
        }

        public static IEnumerable<INamespace> Transform2Meta(IEnumerable<EPackage> packages)
        {
            var model = new Model();
            var rootPackages = TransformationEngine.TransformMany<IEPackage, INamespace>(packages, ecore2Meta);
            model.RootElements.AddRange(rootPackages);
            Uri modelUri;
            if (packages.Count() > 0 && Uri.TryCreate(packages.First().NsURI, UriKind.Absolute, out modelUri))
            {
                model.ModelUri = modelUri;
            }
            return rootPackages;
        }

    }
}
