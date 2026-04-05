using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DanichShop.Views;

namespace DanichShop;

public partial class Registration : Window
{
    
    public Registration()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow Window = new MainWindow();
        Window.Show();
        this.Close();
    }
}