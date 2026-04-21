using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DanichShop.Models;
using DanichShop.Utils;
using DanichShop.Views;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace DanichShop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private User thisUser = new User();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AuthCommand))]
        private string login;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AuthCommand))]
        private string password;

        [ObservableProperty]
        private string windowCaption;

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
            ThisUser.Role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ThisUser.Fname = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
            ThisUser.Sname = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;
            ThisUser.Telephone = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone).Value;
            ThisUser.Email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            WindowCaption = "Успешный вход";
            if (ThisUser.Role == "User") 
            {
                MainShop Window1 = new MainShop();
                Window1.Show();
            }
            if (ThisUser.Role == "Admin")
            {
                AdminPanel Window1 = new AdminPanel();
                Window1.Show();
            }

            close();
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
