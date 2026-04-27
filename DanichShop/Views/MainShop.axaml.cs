using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DanichShop;

public partial class MainShop : Window
{
    public MainShop()
    {
        InitializeComponent();
        ContentPage.Content = new CatalogePage();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentPage.Content = new MainPage();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentPage.Content = new CatalogePage();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentPage.Content = new AccountPage();
    }
}