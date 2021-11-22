using FileApp.ViewModels;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace FileApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow(MainWindowViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
