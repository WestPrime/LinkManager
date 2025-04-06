// MyWpfWindow.xaml.cs
using System.Windows;

namespace LinkManager
{
    public partial class MyWpfWindow : Window
    {
        public MyWpfWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Привет! WPF работает исправно! :)");
        }
    }
}