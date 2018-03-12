using NMF.Models.Repository;
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
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace NMF.Controls
{
    /// <summary>
    /// Interaktionslogik für PropertyView.xaml
    /// </summary>
    public partial class PropertyView : PropertyGrid
    {
        public PropertyView()
        {
            InitializeComponent();
        }

        public IModelRepository Repository { get; set; }

        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
