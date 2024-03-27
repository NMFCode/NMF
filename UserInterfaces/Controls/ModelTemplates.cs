using System.Windows;

namespace NMF.Controls
{
    /// <summary>
    /// Denotes the model templates
    /// </summary>
    public partial class ModelTemplates
    {
        /// <summary>
        /// Denotes the instance
        /// </summary>
        public static readonly ModelTemplates Instance;

        static ModelTemplates()
        {
            Instance = new ModelTemplates();
            Instance.InitializeComponent();
        }

        /// <summary>
        /// Gets the small item template
        /// </summary>
        public static DataTemplate SmallItemTemplate
        {
            get { return Instance["SmallItemTemplate"] as DataTemplate; }
        }
    }
}
