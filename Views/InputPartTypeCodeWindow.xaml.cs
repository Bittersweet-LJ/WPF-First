using System;
using System.Windows;

namespace testTreeView.Views
{
    /// <summary>
    /// InputPartTypeCodeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputPartTypeCodeWindow : Window
    {
        public InputPartTypeCodeWindow()
        {
            InitializeComponent();
        }

        public string TypeCode
        {
            get { return (string)GetValue(TypeCodeProperty); }
            set { SetValue(TypeCodeProperty, value); }
        }

        public static readonly DependencyProperty TypeCodeProperty =
            DependencyProperty.Register("TypeCode", typeof(string), typeof(InputPartTypeCodeWindow), new PropertyMetadata(""));



        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }
     
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(InputPartTypeCodeWindow), new PropertyMetadata(""));





        public event EventHandler<string> CodeSubmit;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CodeSubmit?.Invoke(this, TypeCode);
        }
    }
}
