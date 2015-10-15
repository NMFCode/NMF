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

namespace NMF.Interop.Ecore
{
    public static class EcoreInterop
    {
        private static ModelRepository repository = new ModelRepository();
        private static ResourceMapLocator resourceLocator;
        private static Ecore2MetaTransformation ecore2Meta = new Ecore2MetaTransformation();

        static EcoreInterop()
        {
            resourceLocator = new ResourceMapLocator(typeof(EcoreInterop).Assembly);
            resourceLocator.Mappings.Add(new Uri("http://www.eclipse.org/emf/2002/Ecore", UriKind.Absolute), "NMF.Interop.Ecore.Ecore.ecore");
            resourceLocator.Mappings.Add(new Uri("platform:/plugin/org.emftext.commons.layout/metamodel/layout.ecore", UriKind.Absolute), "NMF.Interop.Ecore.layout.ecore");
            repository.Locators.Add(resourceLocator);
        }

        public static EPackage LoadPackageFromFile(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Environment.CurrentDirectory, path);
            }
            return LoadPackageFromUri(new Uri(path));
        }

        public static EPackage LoadPackageFromUri(Uri uri)
        {
            var modelElement = repository.Resolve(uri);

            var package = modelElement as EPackage;
            if (package != null) return package;

            return modelElement.Model.RootElements.OfType<EPackage>().FirstOrDefault();
        }

        public static IModelLocator EcoreLocator
        {
            get
            {
                return resourceLocator;
            }
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

    }
}
