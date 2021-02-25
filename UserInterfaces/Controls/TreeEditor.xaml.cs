using NMF.Controls.ContextMenu;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace NMF.Controls
{
    /// <summary>
    /// Interaktionslogik für TreeEditor.xaml
    /// </summary>
    public partial class TreeEditor : UserControl
    {
        /// <summary>
        /// Creates a new tree editor control
        /// </summary>
        public TreeEditor()
        {
            ItemTemplate = ModelTemplates.SmallItemTemplate;

            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, DeleteSelectedElement, CanDelete));
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = innerTree.SelectedItem != null;
            e.ContinueRouting = false;
        }

        private void DeleteSelectedElement(object sender, ExecutedRoutedEventArgs e)
        {
            var element = innerTree.SelectedItem as IModelElement;
            if (element != null)
            {
                element.Delete();
            }
        }

        /// <summary>
        /// Gets or sets the item template for elements in the tree editor
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the ItemTemplate property
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(TreeEditor), new PropertyMetadata());

        /// <summary>
        /// Gets the selected item in the tree editor
        /// </summary>
        public IModelElement SelectedItem
        {
            get { return innerTree.SelectedItem as IModelElement; }
        }

        /// <summary>
        /// Gets the SelectedItem property
        /// </summary>
        public static readonly DependencyPropertyKey SelectedItemProperty =
            DependencyProperty.RegisterReadOnly("SelectedItem", typeof(IModelElement), typeof(TreeEditor), new PropertyMetadata());

        private void innerTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetValue(SelectedItemProperty, innerTree.SelectedItem as IModelElement);
        }


        /// <summary>
        /// Gets or sets the root element shown in the tree editor
        /// </summary>
        public IModelElement RootElement
        {
            get { return (IModelElement)GetValue(RootElementProperty); }
            set { SetValue(RootElementProperty, value); }
        }

        /// <summary>
        /// Gets the RootElement property
        /// </summary>
        public static readonly DependencyProperty RootElementProperty =
            DependencyProperty.Register("RootElement", typeof(IModelElement), typeof(TreeEditor), new PropertyMetadata(RootChanged));

        private static void RootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as TreeEditor;
            editor.innerTree.Items.Clear();
            editor.innerTree.Items.Add(editor.RootElement);
        }
    }
}
