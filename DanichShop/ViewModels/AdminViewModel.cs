using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DanichShop.Models;
using DanichShop.Utils;
using DanichShop.Views;
using SkiaSharp;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DanichShop.ViewModels
{
    public partial class AdminViewModel : ViewModelBase
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateItemCommand))]
        private string title;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateItemCommand))]
        private string description;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateItemCommand))]
        private decimal cost;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateItemCommand))]
        private int count;


        [ObservableProperty]
        private byte[] pictureBytes;


        [ObservableProperty]
        private Bitmap picturePreview;

        [ObservableProperty]
        private string windowCaption;

        [RelayCommand(CanExecute = nameof(CanExecuteLoginCommand))]
        public async Task CreateItem()
        {

         
            var client = Http.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);
            var data = new Item { Title = this.Title, Cost = this.Cost,Description = this.Description,Picture = this.PictureBytes, Count = this.Count };
            var result = await client.PostAsync("ItemControllers/createitem", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка создания";
                return;
            }


            WindowCaption = "Предмет создан";
          

           
        }
        [RelayCommand]
        private async Task SelectImage()
        {
            AdminPanel adminPanel = new AdminPanel();
            var topLevel = TopLevel.GetTopLevel(adminPanel);
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions());

            if (files.Count > 0)
            {
                var path = files[0].Path.LocalPath;

                PictureBytes = File.ReadAllBytes(path);
                PicturePreview = new Bitmap(path);
            }
        }


        bool CanExecuteLoginCommand()
        {
            return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Description);
        }

        private Action close;
        public void SetCloseAction(Action close)
        {
            this.close = close;
        }
    }
}
