using NMF.AnyText;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AnyText.Tests.ListExpressions
{
    public partial class ListExpressionsGrammar
    {
        public partial class DoubleRule
        {
            public override string ConvertToString(double semanticElement, ParseContext context)
            {
                return semanticElement.ToString("0.0", CultureInfo.InvariantCulture);
            }
        }
    }
}
