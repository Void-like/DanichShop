using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using DanichShop.Models;
using DanichShop.Utils;
using DanichShop.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DanichShop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private User ThisUser;
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
            WindowCaption = "Успешный вход";
            MainShop Window1 = new MainShop();
            Window1.Show();
            


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
