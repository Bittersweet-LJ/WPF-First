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
using System.Windows.Shapes;
using testTreeView.Models;
using testTreeView.Services;
using testTreeView.ViewModels;

namespace testTreeView.Views
{
    /// <summary>
    /// SelectLocationBasePartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectLocationBasePartWindow : Window
    {
        public SelectLocationBasePartWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new PartTypeService(), new PropertyDefineService(), new LocationModeDefineService());
        }



        public PartType BasePartType
        {
            get { return (PartType)GetValue(BasePartTypeProperty); }
            set { SetValue(BasePartTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BasePartType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BasePartTypeProperty =
            DependencyProperty.Register("BasePartType", typeof(PartType), typeof(SelectLocationBasePartWindow), new PropertyMetadata(null));


        public event EventHandler BasePartSelectSubmit;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BasePartSelectSubmit?.Invoke(this, EventArgs.Empty);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            BasePartType = e.NewValue as PartType;
        }
    }
}
