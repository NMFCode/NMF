using NMF.Models;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;
using NMF.Utilities;
using NMF.Expressions;

namespace NMF.Controls
{
    /// <summary>
    /// Interaktionslogik für PropertyView.xaml
    /// </summary>
    public partial class PropertyView : PropertyGrid
    {
        /// <summary>
        /// Gets raised when all elements should be found
        /// </summary>
        public event EventHandler<FindAllElementsEventArgs> FindAllElements;

        /// <summary>
        /// Creates a new property view
        /// </summary>
        public PropertyView()
        {
            InitializeComponent();
            DataTemplate = ModelTemplates.SmallItemTemplate;
        }

        /// <summary>
        /// Gets or sets the data template to be used
        /// </summary>
        public DataTemplate DataTemplate
        {
            get { return (DataTemplate)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the DataTemplate property
        /// </summary>
        public static readonly DependencyProperty DataTemplateProperty =
            DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(PropertyView), new PropertyMetadata());


        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {
            if (!((sender as FrameworkElement).DataContext is PropertyItem property)) return;
            var collectionType = Array.Find(
                property.PropertyType.GetInterfaces(),
                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (collectionType == null) return;
            var itemType = collectionType.GetGenericArguments()[0];
            if (typeof(IModelElement).IsAssignableFrom(itemType))
            {
                var editor = new CollectionEditorDialog(property);
                editor.ItemTemplate = DataTemplate;
                editor.ShowDialog();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        internal IEnumerable<IModelElement> GetPossibleItemsFor(PropertyItem property)
        {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            if (!(property.Instance is IModelElement element)) return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            var type = property.PropertyType;
            if (!typeof(IModelElement).IsAssignableFrom(type))
            {
                var collection = Array.Find( type.GetInterfaces(),
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                type = collection?.GetGenericArguments()[0];
            }
            if (type == null) return null;
            if (FindAllElements != null)
            {
                var e = new FindAllElementsEventArgs(element, property.PropertyDescriptor, type, null);
                FindAllElements(this, e);
                if (e.AllowableElements != null)
                {
                    return e.AllowableElements;
                }
            }
            var model = element.Model;
            if (model == null) return null;
            var repository = model.Repository;
            if (repository == null)
            {
                return model.Descendants().Where(e => type.IsInstanceOfType(e));
            }
            else
            {
                IEnumerable<Model> models;
                if (!(repository is ModelRepository modelRepo))
                {
                    models = repository.Models.Values.Distinct();
                }
                else
                {
                    models = modelRepo.Models.Values.Distinct().Concat(modelRepo.Parent.Models.Values.Distinct());
                }
                return models.SelectMany(m => m.Descendants()).Where(e => type.IsInstanceOfType(e));
            }
        }

        private void PrepareProperty(object sender, PropertyItemEventArgs e)
        {
            if (e.PropertyItem is PropertyItem property && typeof(IEnumerableExpression).IsAssignableFrom(property.PropertyType))
            {
                property.IsReadOnly = false;
            }
        }
    }

    /// <summary>
    /// Denotes the event data when all elements should be obtained
    /// </summary>
    public class FindAllElementsEventArgs : EventArgs
    {
        /// <summary>
        /// The type of elements
        /// </summary>
        public Type ElementType { get; private set; }

        /// <summary>
        /// The instance for which elements are required
        /// </summary>
        public IModelElement Instance { get; private set; }

        /// <summary>
        /// The property for which the elements should be added
        /// </summary>
        public PropertyDescriptor Property { get; private set; }

        /// <summary>
        /// Gets or sets the allowable elements
        /// </summary>
        public IEnumerable<IModelElement> AllowableElements { get; set; }

        /// <summary>
        /// Creates a new event data object
        /// </summary>
        /// <param name="instance">the instance</param>
        /// <param name="property">the property</param>
        /// <param name="elementType">the element type</param>
        /// <param name="elements">the elements</param>
        public FindAllElementsEventArgs(IModelElement instance, PropertyDescriptor property, Type elementType, IEnumerable<IModelElement> elements)
        {
            ElementType = elementType;
            Instance = instance;
            Property = property;
            AllowableElements = elements;
        }
    }
}
