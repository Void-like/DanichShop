using Avalonia.Controls;
using DanichShop.ViewModels;

namespace DanichShop.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Opened += (sender, args) =>
            {

                (DataContext as MainWindowViewModel).SetCloseAction(Close);
            };
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Registration Window = new Registration();
            Window.Show();
            this.Close();
        }

    
    }
}