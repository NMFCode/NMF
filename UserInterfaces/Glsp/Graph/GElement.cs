using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Graph
{
    /// <summary>
    /// Denotes a node in the graph model of GLSP
    /// </summary>
    public class GElement
    {
        private GElement _parent;

        internal GElementSkeletonBase Skeleton { get; set; }

        internal object CreatedFrom { get; set; }

        internal INotationElement NotationElement { get; set; }

        /// <summary>
        /// Creates a new element
        /// </summary>
        public GElement() : this(null) { }

        /// <summary>
        /// Creates a new element
        /// </summary>
        /// <param name="id">The id of the new element</param>
        /// <remarks>If the id is null, a new id is generated</remarks>
        public GElement(string id)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The graph that this element belongs to
        /// </summary>
        [JsonIgnore]
        public virtual GGraph Graph => Parent?.Graph;

        /// <summary>
        /// Sets the given field and return if any changes have been made
        /// </summary>
        /// <typeparam name="T">The field type</typeparam>
        /// <param name="field">The field</param>
        /// <param name="value">The new value</param>
        /// <returns>True, if the value is different than the field, otherwise false</returns>
        protected bool Set<T>(ref T field, T value)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                return true;
            }
            return false;
        }

        private Point? _position;
        private Dimension? _size;

        /// <summary>
        /// Gets or sets the type of the element
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets the id of the element
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets or sets the position of the element
        /// </summary>
        public Point? Position
        {
            get => _position;
            set
            {
                if (Set(ref _position, value)) PositionChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Gets or sets the size of the element
        /// </summary>
        public Dimension? Size
        {
            get => _size;
            set
            {
                if (Set(ref _size, value)) SizeChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Gets or sets the parent of the graph element
        /// </summary>
        [JsonIgnore]
        public GElement Parent
        {
            get => _parent;
            set
            {
                if (Set(ref _parent, value) && value != null)
                {
                    Register();
                }
            }
        }

        private void Register()
        {
            var graph = _parent.Graph;
            if (graph != null)
            {
                RegisterAll(graph);
            }
        }

        private void RegisterAll(GGraph graph)
        {
            graph.RegisterId(Id, this);
            foreach (var child in Children)
            {
                child.RegisterAll(graph);
            }
        }

        /// <summary>
        /// Gets a collection of child elements
        /// </summary>
        public IListExpression<GElement> Children { get; } = new ObservableList<GElement>();

        /// <summary>
        /// Gets CSS classes assigned to this element
        /// </summary>
        public IListExpression<string> CssClasses { get; } = new ObservableList<string>();

        /// <summary>
        /// Gets a dictionary of details for this element
        /// </summary>
        public IDictionary<string, object> Details { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets a dictionary of objects that should be disposed when this element is deleted
        /// </summary>
        internal Dictionary<object, IDisposable> Collectibles { get; } = new Dictionary<object, IDisposable>();

        internal void UpdateClass(object sender, ValueChangedEventArgs e)
        {
            if (e.OldValue is string oldValue) { CssClasses.Remove(oldValue); }
            if (e.NewValue is string newValue) { CssClasses.Add(newValue); }
        }

        internal void UpdateType(object sender, ValueChangedEventArgs e)
        {
            Type = e.NewValue as string;
        }

        /// <summary>
        /// Deletes this model element
        /// </summary>
        public void Delete()
        {
            Deleted?.Invoke();
        }

        internal void SilentDelete()
        {
            Deleted = null;
            foreach (var item in Children)
            {
                item.SilentDelete();
            }
            foreach (var disposable in Collectibles.Values)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Raised when the element is deleted
        /// </summary>
        public event Action Deleted;

        /// <summary>
        /// Raised when the position of this element changes
        /// </summary>
        public event Action<GElement> PositionChanged;

        /// <summary>
        /// Raised when the size of this element changes
        /// </summary>
        public event Action<GElement> SizeChanged;
    }
}
