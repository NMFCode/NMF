using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Reactive;
using NMF.Models;
using System;

namespace NMF.Controls;

/// <summary>
/// Defines a control for a tree editor for model elements
/// </summary>
public partial class TreeEditor : UserControl
{
    static TreeEditor()
    {
        SelectedItemProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs>(OnSelectedItemChanged));
        RootElementProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs>(OnRootChanged));
    }

    private static void OnRootChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is TreeEditor treeEditor)
        {
            treeEditor.OnRootChanged(args.OldValue as IModelElement, args.NewValue as IModelElement);
        }
    }

    private static void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is TreeEditor treeEditor)
        {
            treeEditor.OnSelectedItemChanged(args.OldValue as IModelElement, args.NewValue as IModelElement);
        }
    }

    /// <summary>
    /// Gets called when the selected item changed
    /// </summary>
    /// <param name="oldValue">the old selection</param>
    /// <param name="newValue">the new selection</param>
    protected virtual void OnSelectedItemChanged(IModelElement oldValue, IModelElement newValue)
    {
        innerTree.SelectedItem = newValue;
    }

    /// <summary>
    /// Gets called when the selected element changed
    /// </summary>
    /// <param name="oldRoot">the old root element</param>
    /// <param name="newRoot">the new root element</param>
    protected virtual void OnRootChanged(IModelElement oldRoot, IModelElement newRoot)
    {
        innerTree.Items.Clear();
        innerTree.Items.Add(newRoot);
    }

    /// <summary>
    /// Creates a new tree editor control
    /// </summary>
    public TreeEditor()
    {
        ItemTemplate = ModelTemplates.ItemTemplate;

        InitializeComponent();

        innerTree.SelectionChanged += InnerTree_SelectionChanged;
    }

    private void InnerTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SetValue(SelectedItemProperty, innerTree.SelectedItem as IModelElement);
    }

    /// <summary>
    /// Gets or sets the item template for elements in the tree editor
    /// </summary>
    public IDataTemplate ItemTemplate
    {
        get { return (IDataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    /// <summary>
    /// Gets the ItemTemplate property
    /// </summary>
    public static readonly AvaloniaProperty<IDataTemplate> ItemTemplateProperty =
        AvaloniaProperty.Register<TreeEditor, IDataTemplate>("ItemTemplate");

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
    public static readonly AvaloniaProperty<IModelElement> SelectedItemProperty =
        AvaloniaProperty.Register<TreeEditor, IModelElement>("SelectedItem");


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
    public static readonly AvaloniaProperty<IModelElement> RootElementProperty =
        AvaloniaProperty.Register<TreeEditor, IModelElement>("RootElement");
}