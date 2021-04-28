using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NMF.Models.Repository.Serialization;
using NMF.Models.Meta;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes the repository with the metamodels contained in the loaded assemblies
    /// </summary>
    public sealed class MetaRepository : IModelRepository
    {
        private static readonly MetaRepository instance = new MetaRepository();
        private readonly ModelCollection entries;
        private readonly ModelSerializer serializer = new ModelSerializer();
        private readonly HashSet<Assembly> traversedAssemblies = new HashSet<Assembly>();

        event EventHandler<BubbledChangeEventArgs> IModelRepository.BubbledChange
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets the instance
        /// </summary>
        public static MetaRepository Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets the serializer used for the deserialization of the models
        /// </summary>
        public ModelSerializer Serializer
        {
            get
            {
                return serializer;
            }
        }

        private MetaRepository()
        {
            entries = new ModelCollection(this);
            serializer.KnownTypes.Add(typeof(INamespace));
            serializer.KnownTypes.Add(typeof(Model));

            var domain = AppDomain.CurrentDomain;
            domain.AssemblyLoad += domain_AssemblyLoad;
            var assemblies = domain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                RegisterAssembly(assemblies[i]);
            }

            var metaNamespace = entries[new Uri("http://nmf.codeplex.com/nmeta/")].RootElements[0] as INamespace;
            foreach (var type in metaNamespace.Types.OfType<PrimitiveType>())
            {
                switch (type.Name)
                {
                    case "Boolean":
                        MapType(type, typeof(bool));
                        break;
                    case "Byte":
                        MapType(type, typeof(byte));
                        break;
                    case "ByteArray":
                        MapType(type, typeof(byte[]));
                        break;
                    case "Char":
                        MapType(type, typeof(char));
                        break;
                    case "DateTime":
                        MapType(type, typeof(DateTime));
                        break;
                    case "Decimal":
                        MapType(type, typeof(decimal));
                        break;
                    case "Double":
                        MapType(type, typeof(double));
                        break;
                    case "Float":
                        MapType(type, typeof(float));
                        break;
                    case "Guid":
                        MapType(type, typeof(Guid));
                        break;
                    case "Integer":
                        MapType(type, typeof(int));
                        break;
                    case "Long":
                        MapType(type, typeof(long));
                        break;
                    case "Object":
                        MapType(type, typeof(object));
                        break;
                    case "String":
                        MapType(type, typeof(string));
                        break;
                    case "Short":
                        MapType(type, typeof(short));
                        break;
                    case "TimeSpan":
                        MapType(type, typeof(TimeSpan));
                        break;
                    case "Uri":
                        MapType(type, typeof(Uri));
                        break;
                    case "SystemType":
                        MapType(type, typeof(System.Type));
                        break;
                    default:
                        break;
                }
            }
        }

        private void MapType(IType type, System.Type systemType)
        {
            MappedType.FromType(type).SystemType = systemType;
        }

        /// <summary>
        /// Resolves the given uri into a type
        /// </summary>
        /// <param name="uriString">The absolute uri</param>
        /// <returns>The type object</returns>
        public IType ResolveType(string uriString)
        {
            return Resolve(new Uri(uriString, UriKind.Absolute)) as IType;
        }

        public IType ResolveClass(System.Type systemType)
        {
            if (systemType == null) throw new ArgumentNullException("systemType");
            var modelAtt = systemType.GetCustomAttributes(typeof(ModelRepresentationClassAttribute), false);
            if (modelAtt != null && modelAtt.Length > 0)
            {
                var representation = (ModelRepresentationClassAttribute)modelAtt[0];
                return ResolveType(representation.UriString);
            }
            return null;
        }

        private void RegisterAssembly(Assembly assembly)
        {
            if (!traversedAssemblies.Add(assembly)) return;
            var references = assembly.GetReferencedAssemblies();
            if (references != null)
            {
                for (int i = 0; i < references.Length; i++)
                {
                    try
                    {
                        RegisterAssembly(Assembly.Load(references[i]));
                    }
                    catch (System.IO.IOException)
                    {
                        // eat exception
                    }
                }
            }
            var attributes = assembly.GetCustomAttributes(typeof(ModelMetadataAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
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
                            if (!t.IsInterface)
                            {
                                var iface = t.GetInterface(t.Namespace + ".I" + t.Name);
                                if (iface != null)
                                {
                                    t = iface;
                                }
                            }
                            saveMapping.Add(new KeyValuePair<string, System.Type>(attr.UriString, t));
                        }
                    }
                }
                var names = assembly.GetManifestResourceNames();
                if (names == null || names.Length == 0)
                {
                    throw new Exception($"The assembly {assembly.FullName} declares a model but has no embedded resources. Did you forget to embed a model?");
                }
                for (int i = 0; i < attributes.Length; i++)
                {
                    var metadata = attributes[i] as ModelMetadataAttribute;
                    var resourceName = metadata.ResourceName;
                    if (!names.Contains(metadata.ResourceName))
                    {
                        var resources = names.Where(n => n.EndsWith(resourceName)).ToList();
                        if (resources.Count == 1)
                        {
                            resourceName = resources[0];
                        }
                        else if (resources.Count == 0)
                        {
                            throw new Exception($"Embedded resource {resourceName} was not found in assembly {assembly.FullName}.");
                        }
                        else
                        {
                            throw new Exception($"Multiple embedded resources with the suffix {resourceName} were found in {assembly.FullName}.");
                        }
                    }
                    if (metadata != null && metadata.ModelUri.IsAbsoluteUri)
                    {
#if DEBUG
                        LoadModel(assembly, attributes, i, resourceName, metadata.ModelUri);
#else
                        try
                        {
                            LoadModel(assembly, attributes, i, resourceName, metadata.ModelUri);
                        }
                        catch (Exception e)
                        {
                            var hint = "";
                            if (i < attributes.Length - 1)
                            {
                                hint = " Hint: Resources are loaded in the order in which they are specified. If a metamodel requires another metamodel from the same assembly, the metadata attribute must be specified afterwards. All metamodels must be in the same directory.";
                            }
                            throw new Exception($"Error loading the embedded resource {metadata.ResourceName} from assembly {assembly.FullName}: {e.Message}{hint}", e); ;
                        }
#endif
                    }
                    else
                    {
                        throw new Exception($"The declared embedded resource {metadata.ResourceName} of assembly {assembly.FullName} could not be found.");
                    }
                }
                for (int i = 0; i < saveMapping.Count; i++)
                {
                    var cls = ResolveType(saveMapping[i].Key);
                    if (cls != null)
                    {
                        var typeExtension = MappedType.FromType(cls);
                        typeExtension.SystemType = saveMapping[i].Value;
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("The class {0} could not be resolved.", saveMapping[i].Key));
                    }
                }
            }
        }

        private void LoadModel(Assembly assembly, object[] attributes, int i, string resourceName, Uri modelUri)
        {
            var model = serializer.Deserialize(assembly.GetManifestResourceStream(resourceName), modelUri, this, true);
            for (int j = i + 1; j < attributes.Length; j++)
            {
                if (attributes[j] is ModelMetadataAttribute followingAttribute)
                {
                    var followUri = new Uri(followingAttribute.ModelUri, MakeRelativePath(resourceName, followingAttribute.ResourceName));
                    if (!entries.ContainsKey(followUri))
                    {
                        entries.Add(followUri, model);
                    }
                }
            }
            model.Freeze();
        }

        private string MakeRelativePath(string resourceName, string baseResourceName)
        {
            var resourceSplit = resourceName.Split('.');
            var baseSplit = baseResourceName.Split('.');
            var relativeBuilder = new StringBuilder();
            for (int i = 0; i < resourceSplit.Length && i < baseSplit.Length; i++)
            {
                if (resourceSplit[i] != baseSplit[i])
                {
                    for (int j = i; j < resourceSplit.Length; j++)
                    {
                        relativeBuilder.Append(resourceSplit[j]);
                        if (j < resourceSplit.Length - 1) relativeBuilder.Append(".");
                    }
                    break;
                }
            }
            return relativeBuilder.ToString();
        }

        void domain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            RegisterAssembly(args.LoadedAssembly);
        }

        /// <inheritdoc />
        public IModelElement Resolve(Uri uri)
        {
            Model model;
            if (entries.TryGetValue(uri, out model))
            {
                return model.Resolve(uri.Fragment);
            }
            return null;
        }

        /// <inheritdoc />
        public IModelElement Resolve(string uriString)
        {
            return Resolve(new Uri(uriString, UriKind.Absolute));
        }

        IModelElement IModelRepository.Resolve(Uri uri, bool loadOnDemand)
        {
            return Resolve(uri);
        }

        public ModelCollection Models
        {
            get { return entries; }
        }

    }
}
