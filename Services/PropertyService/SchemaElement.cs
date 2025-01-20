using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// Internal struct that is used to dynamically render the schema 
    /// </summary>
    /// <param name="ModelElement">The model element for which the schema </param>
    /// <param name="Writer">The component used to write the schema</param>
    public record struct SchemaElement(IModelElement ModelElement, SchemaWriter Writer)
    {
    }
}
