using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    public interface IModelValidationLog
    {
        void LogError(IModelElement element, string message);
    }
}
