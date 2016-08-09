using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Analysis
{
    public abstract class Analysis
    {
        public abstract void Run(IModelRepository repository);
    }
}
