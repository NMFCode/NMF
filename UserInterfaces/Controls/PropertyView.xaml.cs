using NMF.Models;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid;
using NMF.Utilities;

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
            var collectionType = property.PropertyType
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
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
            if (!(property.Instance is IModelElement element)) return null;
            var type = property.PropertyType;
            if (!typeof(IModelElement).IsAssignableFrom(type))
            {
                var collection = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
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
