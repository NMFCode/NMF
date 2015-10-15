using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public interface IModelLocator
    {
        bool CanLocate(Uri uri);

        Uri GetRepositoryUri(Uri uri);

        Stream Open(Uri repositoryId);
    }
}
