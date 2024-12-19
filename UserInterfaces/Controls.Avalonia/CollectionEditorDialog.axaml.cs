using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;

namespace NMF.Controls;

public partial class CollectionEditorDialog : Window
{
    public CollectionEditorDialog()
    {
        InitializeComponent();
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }


    /// <summary>
    /// Gets or sets the item template
    /// </summary>
    public IDataTemplate ItemTemplate
    {
        get { return (IDataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    /// <summary>
    /// The ItemTemplate Dependency property
    /// </summary>
    public static readonly AvaloniaProperty ItemTemplateProperty =
        AvaloniaProperty.Register<CollectionEditorDialog, IDataTemplate>("ItemTemplate");

    private void AddItems(object sender, RoutedEventArgs e)
    {
        if (DataContext is CollectionViewModel viewModel)
        {
            foreach (var item in allItems.SelectedItems)
            {
                viewModel.Items.Add(item);
            }
        }
    }

    private void RemoveItems(object sender, RoutedEventArgs e)
    {
        if (DataContext is CollectionViewModel viewModel && selectedItems.SelectedItems.Count > 0)
        {
            var selectedItemsArray = new object[selectedItems.SelectedItems.Count];
            selectedItems.SelectedItems.CopyTo(selectedItemsArray, 0);
            foreach (var item in selectedItemsArray)
            {
                viewModel.Items.Remove(item);
            }
        }
    }
}