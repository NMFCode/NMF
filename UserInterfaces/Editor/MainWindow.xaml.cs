using Microsoft.Win32;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Windows;
using System.Windows.Input;

namespace Editor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ModelRepository repository = new ModelRepository();
        private int modelCounter;

        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, CreateNewModel));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenModel));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveModel));
        }

        private void SaveModel(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Metamodels (*.nmeta)|*.nmeta|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    repository.Save(Tree.RootElement, saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OpenModel(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Metamodels (*.nmeta)|*.nmeta|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    var model = repository.Resolve(openFile.FileName);
                    Tree.RootElement = model;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateNewModel(object sender, ExecutedRoutedEventArgs e)
        {
            var ns = new Namespace();
            var model = new Model();
            model.RootElements.Add(ns);
            repository.Models.Add(new Uri($"temp:newModel{modelCounter++}"), model);
            Tree.RootElement = ns;
        }
    }
}
