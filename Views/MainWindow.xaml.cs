using System.Windows;
using testTreeView.Models;
using testTreeView.Services;
using testTreeView.ViewModels;

namespace testTreeView.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MainWindowViewModel mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainWindowViewModel = new MainWindowViewModel(
                new PartTypeService(),
                new PropertyDefineService(),
                new LocationModeDefineService()
                );
            
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            mainWindowViewModel.CurrentPartType = e.NewValue as PartType;
        }

        private void TreeView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainWindowViewModel.CurrentPartType = null;
        }
    }
}
