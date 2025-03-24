using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents programming constructs like variables, classes, interfaces etc.
    /// that appear in a document.Document symbols can be hierarchical and they
    /// have two ranges: one that encloses its definition and one that points to its
    /// most interesting range, e.g.the range of an identifier.
    /// Analogous to the LspTypes DocumentSymbol interface.
    /// </summary>
    public class DocumentSymbol
    {
        /// <summary>
        /// The name of this symbol. Will be displayed in the user interface and
        /// therefore must not be an empty string or a string only consisting of
        /// white spaces.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// More detail for this symbol, e.g the signature of a function.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// The kind of this symbol.
        /// </summary>
        public SymbolKind Kind { get; set; }

        /// <summary>
        /// Tags for this document symbol.
        /// </summary>
        public SymbolTag[] Tags { get; set; }

        /// <summary>
        /// The range enclosing this symbol not including leading/trailing whitespace
        /// but everything else like comments.This information is typically used to
        /// determine if the clients cursor is inside the symbol to reveal in the
        /// symbol in the UI.
        /// </summary>
        public ParseRange Range { get; set; }

        /// <summary>
        /// The range that should be selected and revealed when this symbol is being
        /// picked, e.g.the name of a function. Must be contained by the <see cref="Range"></see>.
        /// </summary>
        public ParseRange SelectionRange { get; set; }

        /// <summary>
        /// Children of this symbol, e.g. properties of a class.
        /// </summary>
        public IEnumerable<DocumentSymbol> Children { get; set; }
    }
}
