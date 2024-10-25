using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Transformation
{
    public class CodeGeneratorSettings
    {
        public string Namespace { get; set; } = "Generated";

        public List<string> ImportedNamespaces { get; } = new List<string>
        {
            "NMF.AnyText.Model",
            "NMF.AnyText.Rules",
            "NMF.AnyText.PrettyPrinting",
            "System",
            "System.Text.RegularExpressions"
        };
    }
}
