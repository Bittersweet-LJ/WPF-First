using System.Windows.Controls;

namespace testTreeView.Views
{
    /// <summary>
    /// PropertyDefineView.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyDefineView : UserControl
    {
        public PropertyDefineView()
        {
            InitializeComponent();
        }
    
        private void CurrentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            propertyGrid.IsEnabled = true;
        }

        private void ParentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            propertyGrid.IsEnabled = false;
        }

        private void ListBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            ParentList.SelectedItem = null;
            //CurrentList.SelectedItem = null;
        }
    }
}
