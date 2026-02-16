using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Represents a migration from an old rule application to a new one
    /// </summary>
    /// <param name="Old">the old rule application</param>
    /// <param name="New">the new rule application</param>
    public record struct RuleApplicationMigration(RuleApplication Old, RuleApplication New);
}
