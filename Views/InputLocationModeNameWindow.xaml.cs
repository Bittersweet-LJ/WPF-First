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

namespace testTreeView.Views
{
    /// <summary>
    /// InputPropertyNameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputLocationModelNameWindow : Window
    {
        public InputLocationModelNameWindow()
        {
            InitializeComponent();
        }

        public string LocationName
        {
            get { return (string)GetValue(LocationNameProperty); }
            set { SetValue(LocationNameProperty, value); }
        }

        public static readonly DependencyProperty LocationNameProperty =
            DependencyProperty.Register("LocationName", typeof(string), typeof(InputLocationModelNameWindow), new PropertyMetadata(""));

        public EventHandler NameSubmit ;

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NameSubmit?.Invoke(this, EventArgs.Empty);
        }
    }
}
