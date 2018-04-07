using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NMF.Controls
{
    public partial class ModelTemplates
    {
        public static readonly ModelTemplates Instance;

        static ModelTemplates()
        {
            Instance = new ModelTemplates();
            Instance.InitializeComponent();
        }

        public static DataTemplate SmallItemTemplate
        {
            get { return Instance["SmallItemTemplate"] as DataTemplate; }
        }
    }
}
