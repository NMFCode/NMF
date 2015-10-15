using NMF.Serialization.Xmi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NMF.Models.Repository.Serialization
{
    public class ModelSerializationContext : XmiSerializationContext
    {
        public ModelSerializationContext(IModelRepository repository, Model root) : base(root)
        {
            Repository = repository;
        }

        public IModelRepository Repository { get; private set; }

        public Model Model { get { return Root as Model; } }

        private static Regex colonRegex = new Regex(@"^\w+:\w+ ", RegexOptions.Compiled);

        public override object Resolve(string id, Type type)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var match = colonRegex.Match(id);
            if (match.Success)
            {
                id = id.Substring(match.Length);
            }

            Uri uri;
            IModelElement resolved = null;
            int hashIndex = id.IndexOf('#');
            if (hashIndex != -1)
            {
                if (hashIndex == 0)
                {
                    resolved = Model.Resolve(id);
                }
                else if (Uri.TryCreate(id, UriKind.Absolute, out uri))
                {
                    resolved = Repository.Resolve(uri);
                }
                else
                {
                    if (Model.ModelUri != null)
                    {
                        var newUri = new Uri(Model.ModelUri, id);
                        resolved = Repository.Resolve(newUri);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else
            {
                resolved = Model.Resolve(id);
            }
            if (resolved != null)
            {
                return resolved;
            }
            return base.Resolve(id, type);
        }
    }
}
