using Avalonia.Controls;
using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;
using Avalonia.PropertyGrid.Services;
using Avalonia.Layout;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Templates;
using Avalonia;

namespace NMF.Controls
{
    /// <summary>
    /// Denotes a model-centered property view for Avalonia
    /// </summary>
    public partial class PropertyView : PropertyGrid
    {
        static PropertyView()
        {
            CellEditFactoryService.Default.AddFactory(new ModelElementFactory());
            CellEditFactoryService.Default.AddFactory(new ModelElementCollectionFactory());
        }

        public PropertyView()
        {
            DataTemplate = ModelTemplates.ItemTemplate;
        }

        /// <summary>
        /// Gets or sets the data template to be used
        /// </summary>
        public IDataTemplate DataTemplate
        {
            get { return (IDataTemplate)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the DataTemplate property
        /// </summary>
        public static readonly AvaloniaProperty DataTemplateProperty =
            AvaloniaProperty.Register<PropertyView, IDataTemplate>("DataTemplate");

        internal class ModelElementFactory : AbstractCellEditFactory
        {
            public override int ImportPriority => 200;

            public override Control HandleNewProperty(PropertyCellContext context)
            {
                if (context.Owner is PropertyView propertyView && typeof(IModelElement).IsAssignableFrom(context.Property.PropertyType) && context.Target is IModelElement modelElement)
                {
                    var control = new ComboBox
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        ItemTemplate = propertyView.DataTemplate,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        ItemsSource = propertyView.GetPossibleItemsFor(context.Property, modelElement, context.Property.PropertyType),
                        SelectedItem = context.GetValue() as IModelElement
                    };

                    control.SelectionChanged += (s, e) =>
                    {
                        SetAndRaise(context, control, control.SelectedItem);
                    };

                    return control;
                }
                return null;
            }

            public override bool HandlePropertyChanged(PropertyCellContext context)
            {
                var propertyDescriptor = context.Property;
                var target = context.Target;
                var control = context.CellEdit;

                if (!typeof(IModelElement).IsAssignableFrom(propertyDescriptor.PropertyType))
                {
                    return false;
                }

                Debug.Assert(control != null);

                ValidateProperty(control, propertyDescriptor, target);

                if (control is ComboBox cb)
                {
                    cb.SelectedItem = propertyDescriptor.GetValue(target);
                    return true;
                }

                return false;
            }
        }

        internal class ModelElementCollectionFactory : AbstractCellEditFactory
        {
            public override int ImportPriority => 200;

            public override void HandleReadOnlyStateChanged(Control control, bool readOnly)
            {
                // do not set control readonly
            }

            public override Control HandleNewProperty(PropertyCellContext context)
            {
                if (context.Owner is PropertyView propertyView
                    && typeof(IEnumerable).IsAssignableFrom(context.Property.PropertyType)
                    && GetCollectionItemType(context.Property.PropertyType) is Type elementType
                    && typeof(IModelElement).IsAssignableFrom(elementType)
                    && context.Target is IModelElement modelElement
                    && context.GetValue() is IList targetCollection)
                {
                    var tb = new TextBlock();
                    var collectionModel = new CollectionViewModel(
                        targetCollection, 
                        propertyView.GetPossibleItemsFor(context.Property, modelElement, elementType),
                        propertyView.DataTemplate);
                    tb.Bind(TextBlock.TextProperty, new Binding("Summary"));
                    var button = new Button
                    {
                        Content = "...",
                        DataContext = collectionModel
                    };
                    button.Click += OpenEditor;
                    button.SetValue(DockPanel.DockProperty, Dock.Right);
                    var control = new DockPanel()
                    {
                        Children =
                        {
                            button,
                            tb
                        },
                        DataContext = collectionModel
                    };
                    control.IsEnabled = true;
                    return control;
                }
                return null;
            }

            private void OpenEditor(object sender, RoutedEventArgs e)
            {
                if (sender is Button button
                    && button.DataContext is CollectionViewModel collectionModel)
                {
                    var editor = new CollectionEditorDialog
                    {
                        DataContext = collectionModel,
                        ItemTemplate = collectionModel.ItemTemplate
                    };
                    if (TopLevel.GetTopLevel(button) is Window window)
                    {
                        editor.ShowDialog(window);
                    }
                    else
                    {
                        editor.Show();
                    }
                }
            }

            public override bool HandlePropertyChanged(PropertyCellContext context)
            {
                var control = context.CellEdit;
                return control.DataContext == context;
            }
        }
    }
}
