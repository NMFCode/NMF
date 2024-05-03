using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Services.Forms
{
    public record struct SchemaElement(IModelElement ModelElement, SchemaWriter Writer)
    {
    }
}
