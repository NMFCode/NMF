using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents a node in a graph which can notify its successors
    /// and gets notified by its dependencies.
    /// </summary>
    public interface INotifiable
    {
        /// <summary>
        /// The nodes that will get notified by this node.
        /// </summary>
        IList<INotifiable> Successors { get; }

        /// <summary>
        /// Nodes that notify this node.
        /// </summary>
        IEnumerable<INotifiable> Dependencies { get; }

        /// <summary>
        /// Gets called when one of the dependencies signal a notification.
        /// </summary>
        /// <param name="sources">The nodes that triggered the notification.</param>
        /// <returns>Whether the successors of this node should be notified.r</returns>
        bool Notify(IEnumerable<INotifiable> sources);

        int TotalVisits { get; set; }

        int RemainingVisits { get; set; }
    }
}
