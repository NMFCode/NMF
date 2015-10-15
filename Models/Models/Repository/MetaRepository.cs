using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NMF.Models.Repository.Serialization;
using NMF.Models.Meta;

namespace NMF.Models.Repository
{
    public class MetaRepository : IModelRepository
    {
        private static MetaRepository instance = new MetaRepository();
        private Dictionary<Uri, Model> entries = new Dictionary<Uri, Model>();
        private ModelSerializer serializer = new ModelSerializer();
        private HashSet<Assembly> modelAssemblies = new HashSet<Assembly>();

        public static MetaRepository Instance
        {
            get
            {
                return instance;
            }
        }

        public ModelSerializer Serializer
        {
            get
            {
                return serializer;
            }
        }

        private MetaRepository()
        {
            serializer.KnownTypes.Add(typeof(INamespace));

            var domain = AppDomain.CurrentDomain;
            domain.AssemblyLoad += domain_AssemblyLoad;
            var assemblies = domain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                RegisterAssembly(assemblies[i]);
            }
        }

        public IClass ResolveClass(string uriString)
        {
            return Resolve(new Uri(uriString, UriKind.Absolute)) as IClass;
        }

        public IClass ResolveClass(System.Type systemType)
        {
            if (systemType == null) throw new ArgumentNullException("systemType");
            var modelAtt = systemType.GetCustomAttributes(typeof(ModelRepresentationClassAttribute), false);
            if (modelAtt != null && modelAtt.Length > 0)
            {
                var representation = (ModelRepresentationClassAttribute)modelAtt[0];
                return ResolveClass(representation.UriString);
            }
            return null;
        }

        private void RegisterAssembly(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(ModelMetadataAttribute), false);
            if (attributes != null && attributes.Length > 0 && modelAssemblies.Add(assembly))
            {
                var references = assembly.GetReferencedAssemblies();
                if (references != null)
                {
                    for (int i = 0; i < references.Length; i++)
                    {
                        RegisterAssembly(Assembly.Load(references[i]));
                    }
                }
                var types = assembly.GetTypes();
                var saveMapping = new List<KeyValuePair<string, System.Type>>();
                if (types != null)
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        var t = types[i];
                        var modelRepresentation = t.GetCustomAttributes(typeof(ModelRepresentationClassAttribute), false);
                        if (modelRepresentation != null && modelRepresentation.Length > 0)
                        {
                            serializer.KnownTypes.Add(t);
                            var attr = (ModelRepresentationClassAttribute)modelRepresentation[0];
                            saveMapping.Add(new KeyValuePair<string, System.Type>(attr.UriString, t));
                        }
                    }
                }
                var names = assembly.GetManifestResourceNames();
                for (int i = 0; i < attributes.Length; i++)
                {
                    var metadata = attributes[i] as ModelMetadataAttribute;
                    Uri modelUri;
                    if (metadata != null && names.Contains(metadata.ResourceName) && Uri.TryCreate(metadata.ModelUri, UriKind.Absolute, out modelUri))
                    {
                        try
                        {
                            serializer.Deserialize(assembly.GetManifestResourceStream(metadata.ResourceName), modelUri, this, true);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex.Message);
                        }
                    }
                }
                for (int i = 0; i < saveMapping.Count; i++)
                {
                    var cls = ResolveClass(saveMapping[i].Key);
                    if (cls != null)
                    {
                        var typeExtension = MappedType.FromType(cls);
                        typeExtension.SystemType = saveMapping[i].Value;
                    }
                }
            }
        }

        void domain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            RegisterAssembly(args.LoadedAssembly);
        }

        public IModelElement Resolve(Uri uri)
        {
            Model model;
            if (entries.TryGetValue(uri, out model))
            {
                return model.Resolve(uri.Fragment);
            }
            return null;
        }

        public IModelElement Resolve(string uriString)
        {
            return Resolve(new Uri(uriString, UriKind.Absolute));
        }

        public IDictionary<Uri, Model> Models
        {
            get { return entries; }
        }

    }
}
