using NMF.Models.Changes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public interface IModelSerializer
    {
        void Serialize(Model model, Stream target);

        void SerializeFragment(ModelElement element, Stream target);

        Model Deserialize(Stream source, Uri modelUri, IModelRepository repository, bool addToRepository);

        //void SerializeChanges(ModelChangeCollection changes, Stream target);

        //ModelCollection DeserializeChanges(Stream source, IModelRepository repository);
    }
}
