using System.Windows;
using ThreadTaskHomework.ViewModels;

namespace ThreadTaskHomework
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainViewModel();
            this.DataContext = vm;
        }
    }
}
