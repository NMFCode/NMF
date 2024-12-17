using Avalonia.Controls;
using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;
using Avalonia.PropertyGrid.Services;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        internal class ModelElementFactory : AbstractCellEditFactory
        {
            public override int ImportPriority => 200;

            public override Control HandleNewProperty(PropertyCellContext context)
            {
                if (context.Owner is PropertyView propertyView && typeof(IModelElement).IsAssignableFrom(context.Property.PropertyType) && context.Target is IModelElement modelElement)
                {
                    var control = new ComboBox
                    {
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
    }
}
