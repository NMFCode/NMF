using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Graph
{
    public class GElement
    {
        public GElement() : this(Guid.NewGuid().ToString()) { }

        public GElement(string id)
        {
            Id = id;
        }

        public string Type { get; set; }

        public string Id { get; }

        public Point? Position { get; set; }

        public Dimension? Size { get; set; }

        public GElement Parent { get; set; }

        public IListExpression<GElement> Children { get; } = new ObservableList<GElement>();

        public IListExpression<string> CssClasses { get; } = new ObservableList<string>();

        public IDictionary<string, string> Details { get; } = new Dictionary<string, string>();

        internal List<IDisposable> Collectibles { get; } = new List<IDisposable>();

        internal void UpdateClass(object sender, ValueChangedEventArgs e)
        {
            if (e.OldValue is string oldValue) { CssClasses.Remove(oldValue); }
            if (e.NewValue is string newValue) { CssClasses.Add(newValue); }
        }

        internal void UpdateType(object sender, ValueChangedEventArgs e)
        {
            Type = e.NewValue as string;
        }

        public void Delete()
        {
            Deleted?.Invoke();
        }

        public event Action Deleted;
    }
}
