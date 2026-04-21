using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DanichShop.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace DanichShop;

public partial class AdminPanel : Window
{
    
    public AdminPanel()
    {
        InitializeComponent();
        DataContext = new AdminViewModel();
        Opened += (sender, args) =>
        {

            (DataContext as AdminViewModel).SetCloseAction(Close);
        };
    }
  
}