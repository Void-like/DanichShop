using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DanichShop.ViewModels;

namespace DanichShop;

public partial class CatalogePage : UserControl
{
    public CatalogePage()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        
    }
}