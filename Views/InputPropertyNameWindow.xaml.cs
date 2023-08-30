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
    public partial class InputPropertyNameWindow : Window
    {
        public InputPropertyNameWindow()
        {
            InitializeComponent();
        }



        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(InputPropertyNameWindow), new PropertyMetadata(""));


        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(InputPropertyNameWindow), new PropertyMetadata(""));


        public event EventHandler<string> NameSubmit;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NameSubmit?.Invoke(this, PropertyName);
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
