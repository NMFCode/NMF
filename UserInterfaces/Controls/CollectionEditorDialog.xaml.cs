using System.Collections;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace NMF.Controls
{
    /// <summary>
    /// Interaktionslogik für CollectionEditorDialog.xaml
    /// </summary>
    public partial class CollectionEditorDialog : Window
    {
        /// <summary>
        /// Creates a new collection editor dialog for the given property
        /// </summary>
        /// <param name="property"></param>
        public CollectionEditorDialog(PropertyItem property)
        {
            DataContext = property;
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Gets or sets the item template
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// The ItemTemplate Dependency property
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(CollectionEditorDialog), new PropertyMetadata());

        private void AddItems(object sender, RoutedEventArgs e)
        {
            var collection = ((PropertyItem)DataContext).Value as IList;
            foreach (var item in allItems.SelectedItems)
            {
                collection.Add(item);
            }
        }
    }
}
