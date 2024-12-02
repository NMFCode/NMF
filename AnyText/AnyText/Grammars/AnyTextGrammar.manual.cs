using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        /// <inheritdoc/>
        public override string[] CompletionTriggerCharacters() => new[] { ".", ":", "=" };

        public partial class AddAssignExpressionRule
        {
            public override string TokenType => "keyword";
        }

        public partial class ModelRuleRule
        {

        }
    }
}
