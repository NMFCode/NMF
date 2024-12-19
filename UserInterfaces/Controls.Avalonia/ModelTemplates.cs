using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using NMF.Controls.Converters;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Controls
{
    /// <summary>
    /// Gives access to standard templates
    /// </summary>
    public static class ModelTemplates
    {
        /// <summary>
        /// Initializes the type
        /// </summary>
        static ModelTemplates()
        {
            classColorConverter = new ClassColorConverter();
            itemTemplate = CreateSmallItemTemplate(classColorConverter);
        }

        private static ClassColorConverter classColorConverter;
        private static IDataTemplate itemTemplate;

        /// <summary>
        /// Gets the converter used to convert collections to string representations
        /// </summary>
        public static CollectionConverter CollectionConverter { get; } = new CollectionConverter();

        /// <summary>
        /// Gets the converter used to convert classes to colors
        /// </summary>
        public static ClassColorConverter ClassColorConverter => classColorConverter;

        /// <summary>
        /// Gets the default small item template
        /// </summary>
        public static IDataTemplate ItemTemplate => itemTemplate;

        private static IDataTemplate CreateSmallItemTemplate(ClassColorConverter classColorConverter)
        {
            return new FuncDataTemplate<IModelElement>((element, nameScope) =>
            {

                var textClass = new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };
                textClass.Bind(TextBlock.TextProperty, new Binding(nameof(IModelElement.ClassName)));
                var textIdentifier = new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };
                textIdentifier.Bind(TextBlock.TextProperty, new Binding(nameof(IModelElement.IdentifierString)));
                var rect = new Rectangle
                {
                    Width = 5,
                    Height = 5,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                };
                rect.Bind(Shape.FillProperty, new Binding { Converter = classColorConverter });
                return new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        rect,
                        textClass,
                        textIdentifier,
                    }
                };
            });
        }
    }
}
