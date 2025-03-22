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
                type = GetCollectionItemType(type);
            }
            if (type == null) return null;
            return GetPossibleItemsFor(property.PropertyDescriptor, element, type);
        }

        private void PrepareProperty(object sender, PropertyItemEventArgs e)
        {
            if (e.PropertyItem is PropertyItem property && typeof(IEnumerableExpression).IsAssignableFrom(property.PropertyType))
            {
                property.IsReadOnly = false;
            }
        }
    }
}
