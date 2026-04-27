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
        private ObservableCollection<User> userschange = new ObservableCollection<User>();

        [ObservableProperty]
        private ObservableCollection<Item> items = new ObservableCollection<Item>();

        [ObservableProperty]
        private ObservableCollection<User> users = new ObservableCollection<User>();

        [ObservableProperty]
        private Item selecteditem;

        [ObservableProperty]
        private User selecteduser;

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


            SearchItem();
        }
        [RelayCommand]
        public async Task ChangeItem()
        {
            var client = Http.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);
            var data = new Item {Id=this.Selecteditem.Id, Title = this.Selecteditem.Title, Cost = this.Selecteditem.Cost, Description = this.Selecteditem.Description, Picture = this.Selecteditem.Picture, Count = this.Selecteditem.Count };
            var result = await client.PostAsync("ItemControllers/changeitem", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка изменения";
                return;
            }


            WindowCaption = "Предмет изменен";
            SearchItem();
        }
        [RelayCommand]
        public async Task ChangeUser()
        {
            var client = Http.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);
            var data = new User {Id = this.Selecteduser.Id,Sname = this.Selecteduser.Sname, Login = this.Selecteduser.Login, Password = this.Selecteduser.Password, Ban = this.Selecteduser.Ban,Balance = this.Selecteduser.Balance,Email = this.Selecteduser.Email,Telephone = this.Selecteduser.Telephone, Fname = this.Selecteduser.Fname,Role = this.Selecteduser.Role };
            var result = await client.PostAsync("Login/adminchangeuser", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка изменения";
                return;
            }




            WindowCaption = "Юзер изменен";
            SearchItem();
        }
        [RelayCommand]
        public async Task DelItem()
        {
            var client = Http.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);

            var result = await client.DeleteAsync($"ItemControllers/Delete/{Selecteditem.Id}");
            if (!result.IsSuccessStatusCode)
            {
                
                WindowCaption = "Ошибка удаление";
                return;
            }




            WindowCaption = "Товар удален";
            SearchItem();
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
            Userschange.Clear();
            await GetItem();
            await GetUser();
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
                foreach (var item in Users)
                {

                    if (item.Login.Contains(Searchchangeuser) || item.Fname.Contains(Searchchangeuser) || item.Sname.Contains(Searchchangeuser))
                    {
                        Userschange.Add(item);
                    }

                }

            }
            else
            {
                foreach (var item in Users)
                {
                    Userschange.Add(item);
                }

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
        [RelayCommand]
        public async Task GetUser()
        {
            var client = Http.GetHttpClient();
            var result = await client.GetAsync("Login/getuser");

            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка загрузки товаров";

            }
            var json = await result.Content.ReadAsStringAsync();

            Users.Clear();
            Users = new(await result.Content.ReadFromJsonAsync<List<User>>());
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
