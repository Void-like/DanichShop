using Avalonia.Controls;

namespace DanichShop.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Registration Window = new Registration();
            Window.Show();
            this.Close();
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainShop Window1 = new MainShop();
            Window1.Show();
            this.Close();
        }
    }
}