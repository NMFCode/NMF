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

        void SerializeFragment(IModelElement element, Stream target);

        Model Deserialize(Stream source, Uri modelUri, IModelRepository repository, bool addToRepository);
    }
}
