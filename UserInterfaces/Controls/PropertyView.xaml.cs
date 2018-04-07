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
        public event EventHandler<FindAllElementsEventArgs> FindAllElements;

        public PropertyView()
        {
            InitializeComponent();
            DataTemplate = ModelTemplates.SmallItemTemplate;
        }


        public DataTemplate DataTemplate
        {
            get { return (DataTemplate)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataTemplateProperty =
            DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(PropertyView), new PropertyMetadata());


        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {
            var property = (sender as FrameworkElement).DataContext as PropertyItem;
            if (property == null) return;
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
            var element = property.Instance as IModelElement;
            if (element == null) return null;
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
                var modelRepo = repository as ModelRepository;
                IEnumerable<Model> models;
                if (modelRepo == null)
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

    public class FindAllElementsEventArgs : EventArgs
    {
        public Type ElementType { get; private set; }
        public IModelElement Instance { get; private set; }
        public PropertyDescriptor Property { get; private set; }
        public IEnumerable<IModelElement> AllowableElements { get; set; }

        public FindAllElementsEventArgs(IModelElement instance, PropertyDescriptor property, Type elementType, IEnumerable<IModelElement> elements)
        {
            ElementType = elementType;
            Instance = instance;
            Property = property;
            AllowableElements = elements;
        }
    }
}
