using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DanichShop.Models;
using DanichShop.Utils;
using DanichShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DanichShop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<Item> korzinalist = new ObservableCollection<Item>();



        [ObservableProperty]
        private decimal balanceChange;
        [ObservableProperty]
        private decimal balanceaccount;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string oldPassword;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string newPassword;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string rNewPassword;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeAccountCommand))]
        private string changelogin;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeAccountCommand))]
        private string changefname;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeAccountCommand))]
        private string changesname;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeAccountCommand))]
        private string changetelephone;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeAccountCommand))]
        private string changeemail;

        [ObservableProperty]
        private string searchthisitem;
        [ObservableProperty]
        private ObservableCollection<Item> listitems = new ObservableCollection<Item>();

        [ObservableProperty]
        private ObservableCollection<Item> items = new ObservableCollection<Item>();

        [ObservableProperty]
        private static User thisUser = new User();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AuthCommand))]
        private string login;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AuthCommand))]
        private string password;

        [ObservableProperty]
        private string windowCaption;




        public MainWindowViewModel()
        {
            SearchItem();
            if (thisUser.Id != 0) 
            {
                make();
                LoadCart();


            }
        }




        [RelayCommand(CanExecute = nameof(CanExecuteLoginCommand))]
        public async void Auth()
        {
            var client = Http.GetHttpClient();
            var data = new LoginData { Login = this.Login, Password = this.Password };
            var result = await client.PostAsync("Login/login", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка входа";
                return;
            }

            ActiveUser.Token = await result.Content.ReadAsStringAsync();
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(ActiveUser.Token);
            ThisUser.Login = Login;
            ThisUser.Id =int.Parse(jwt.Claims.FirstOrDefault(c => c.Type == ClaimValueTypes.Integer32).Value);
            ThisUser.Role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ThisUser.Fname = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
            ThisUser.Sname = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;
            ThisUser.Telephone = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone).Value;
            ThisUser.Email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            ThisUser.Balance =decimal.Parse(jwt.Claims.FirstOrDefault(c => c.Type == "balance").Value);

            WindowCaption = "Успешный вход";
            if (ThisUser.Role == "User") 
            {
              
                MainShop Window1 = new MainShop();
                Window1.Show();
                GetItem();
            }
            if (ThisUser.Role == "Admin")
            {
                AdminPanel Window1 = new AdminPanel();
                Window1.Show();
            }

            close();
        }
        
        private void make()
        {
            Balanceaccount = thisUser.Balance;
            Changelogin = ThisUser.Login;
            Changefname = thisUser.Fname;
            Changesname = thisUser.Sname;
            Changeemail = thisUser.Email;
            Changetelephone = thisUser.Telephone;
        }
        [RelayCommand]
        public async Task ChangeAccount()
        {

            var client = Http.GetHttpClient();
            var data = new ChangeUser {Id = this.ThisUser.Id, Login = this.Changelogin, FirstName = this.Changefname,LastName = this.Changesname, Email = Changeemail,Telephone = this.Changetelephone };
            var result = await client.PostAsync("Login/changeaccount", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

            ActiveUser.Token = await result.Content.ReadAsStringAsync();
            var handler = new JwtSecurityTokenHandler();

            ThisUser.Login = Changelogin;
            ThisUser.Fname = Changefname;
            ThisUser.Sname = Changesname;
            ThisUser.Telephone = Changetelephone;
            ThisUser.Email = Changeemail; 

            WindowCaption = "все данные успешно изменены";
        
        }
        [RelayCommand]
        public async Task AddBalance()
        {
            var client = Http.GetHttpClient();
            var data = new AddBalance {Id = this.ThisUser.Id, addbalance = this.BalanceChange  };
            var result = await client.PostAsync("Login/addbalance", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
            thisUser.Balance = thisUser.Balance + BalanceChange;
            make();

        }
        [RelayCommand]
        public async Task ChangePassword()
        {
            if (NewPassword == RNewPassword) 
            {
                var client = Http.GetHttpClient();
                var data = new ChangePassword { Id = this.ThisUser.Id,OldPassword = this.OldPassword,NewPassword =  this.NewPassword  };
                var result = await client.PostAsync("Login/changepassword", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
                if (!result.IsSuccessStatusCode)
                {
                    WindowCaption = "Неправильный пароль";

                }
                WindowCaption = "Пароль успешно изменен";
            }
            else
            {

                WindowCaption = "Пароли не одинаковые";
            }




        }
        [RelayCommand]
        public async Task SearchItem()
        {
             Listitems.Clear();  
            await GetItem();

            if (!string.IsNullOrWhiteSpace(Searchthisitem))
            {
                foreach (var item in Items)
                {
                    if (item.Title.Contains(Searchthisitem) || item.Description.Contains(Searchthisitem))
                    {
                        Listitems.Add(item);
                    }

                }
            }
            else
            {
                foreach (var item in Items)
                {
                    Listitems.Add(item);
                }
            }
        }
        [RelayCommand]
        public async Task GetItem()
        {
            var client = Http.GetHttpClient();   
            var result = await client.GetAsync("ItemControllers/getitem");

            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка загрузки товаров";
                
            }
           var json = await result.Content.ReadAsStringAsync();
          
           Items.Clear();
           Items = new (await result.Content.ReadFromJsonAsync<List<Item>>());
            WindowCaption = "Товар успешном загружен";
            
        }
        [RelayCommand]
        public async Task LoadCart()
        {
            Korzinalist.Clear();
            var client = Http.GetHttpClient();
            var result = await client.GetAsync($"korzina/get/{ThisUser.Id}");
            var items = await result.Content.ReadFromJsonAsync<List<Item>>();
            if (result.IsSuccessStatusCode)
            {
                 
                    foreach (var item in items)
                    {
                        Korzinalist.Add(item);
                    }
                WindowCaption = "Загружена";
            }
                else
                {
                    WindowCaption = "Корзина пуста";
                }
            
         
        }
        [RelayCommand]
        public async Task RemoveKorzina(int cartId)
        {
            try
            {
                var client = Http.GetHttpClient();
                var result = await client.DeleteAsync($"korzina/remove/{cartId}");

                if (result.IsSuccessStatusCode)
                {
                    WindowCaption = "Товар удалён из корзины";
                    await LoadCart(); 
                }
            }
            catch (Exception ex)
            {
                WindowCaption = $"Ошибка: {ex.Message}";
            }
        }
        [RelayCommand]
        public async Task GetKorzina(int Id)
        {
            var client = Http.GetHttpClient();
            
            var data = new Korzina { UserID = ThisUser.Id,ItemsID = Id};
            var result = await client.PostAsync("Korzina/add", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
            WindowCaption = "Товар добавлен в корзину";
            
        }
        bool CanExecuteLoginCommand()
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }

        private Action close;
        public void SetCloseAction(Action close)
        {
            this.close = close;
        }
    }
}
