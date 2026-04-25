using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DanichShop.Models;
using DanichShop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DanichShop.ViewModels
{
    public partial class AdminViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string searchadditem;
        [ObservableProperty]
        private string searchchangeitem;
        [ObservableProperty]
        private string searchchangeuser;

        [ObservableProperty]
        private ObservableCollection<Item> itemssearch = new ObservableCollection<Item>();
        [ObservableProperty]
        private ObservableCollection<Item> itemschange = new ObservableCollection<Item>();
        [ObservableProperty]
        private ObservableCollection<User> users = new ObservableCollection<User>();

        [ObservableProperty]
        private ObservableCollection<Item> items = new ObservableCollection<Item>();

        [ObservableProperty]
        private Item selecteditem;

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
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changetitle;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changedescription;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private decimal changecost;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private int changecount;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changelogin;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changepassword;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changefname;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changesname;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changetelephone;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changeemail;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private decimal changebalance;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private bool changeban;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeItemCommand))]
        private string changerole;

        [ObservableProperty]
        private byte[] changepictureBytes;


        [ObservableProperty]
        private Bitmap changepicturePreview;



        [ObservableProperty]
        private string windowCaption;



        public AdminViewModel()
        {
            SearchItem();
        }




        [RelayCommand(CanExecute = nameof(CanExecuteAddCommand))]
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
        public async Task ChangeItem()
        {
            var client = Http.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);
            var data = new Item { Title = this.Changetitle, Cost = this.Changecost, Description = this.Changedescription, Picture = this.ChangepictureBytes, Count = this.Changecount };
            var result = await client.PostAsync("ItemControllers/changeitem", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка изменения";
                return;
            }


            WindowCaption = "Предмет изменен";
        }
        [RelayCommand]
        public async Task ChangeUser()
        {
            var client = Http.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);

            //var result = await client.PostAsync("ItemControllers/changeuser", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));





            WindowCaption = "Юзер изменен";
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
        [RelayCommand]
        public async void SearchItem()
        {
            Itemssearch.Clear();
            Itemschange.Clear();
            await GetItem();

            if (!string.IsNullOrWhiteSpace(Searchadditem))
            {
                foreach (var item in Items) 
                {
                 if(item.Title.Contains(Searchadditem)|| item.Description.Contains(Searchadditem))
                    {
                        Itemssearch.Add(item);
                    }

                }


            }
            else
            {
                foreach (var item in Items)
                {
                    Itemssearch.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(Searchchangeitem))
            {
                foreach (var item in Items)
                {
                    
                        if (item.Title.Contains(Searchchangeitem) || item.Description.Contains(Searchchangeitem))
                        {
                        Itemschange.Add(item);
                        }

                }


            }
            else
            {
                foreach (var item in Items)
                {
                    Itemschange.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(Searchchangeuser))
            {
                

            }
            else
            {


            }
            Searchadditem = string.Empty;

            Searchchangeitem = string.Empty;

            Searchchangeuser = string.Empty;
        }
        [RelayCommand]
        public async Task GetItem()
        {
            Items.Clear();
            var client = Http.GetHttpClient();
            var result = await client.GetAsync("ItemControllers/getitem");

            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка загрузки товаров";

            }
            var json = await result.Content.ReadAsStringAsync();
            Items = new(await result.Content.ReadFromJsonAsync<List<Item>>());
            WindowCaption = "Товар успешном загружен";

        }

        bool CanExecuteAddCommand()
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
