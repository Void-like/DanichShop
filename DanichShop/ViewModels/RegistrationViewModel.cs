using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using DanichShop.Models;
using DanichShop.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DanichShop.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegCommand))]
        private string login;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegCommand))]
        private string fName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegCommand))]
        private string sName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegCommand))]
        private string telephone;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegCommand))]
        private string password;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegCommand))]
        private string email;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegCommand))]
        private string repytpassword;

        [ObservableProperty]
        private string windowCaption;

        [RelayCommand(CanExecute = nameof(CanExecuteLoginCommand))]
        public async void Reg()
        {

            var client = Http.GetHttpClient();
            var data = new Register { Login = this.Login, Password = this.Password,FirstName = this.FName,LastName= this.SName,Telephone = this.Telephone,Email = this.Email,RPassword = this.Repytpassword };
            var result = await client.PostAsync("Registration/registration", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

            if (!result.IsSuccessStatusCode)
            {
                WindowCaption = "Ошибка регистрации";
                return;
            }
            WindowCaption = "Аккаунт создан";
     



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

