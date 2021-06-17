using AppBlaBlaCar.Models;
using AppBlaBlaCar.Services;
using AppBlaBlaCar.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBlaBlaCar.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        Command _LoginCommand;
        Command _RegisterCommand;
        public Command LoginCommand => _LoginCommand ?? (_LoginCommand = new Command(OnLoginClicked));
        public Command RegisterCommand => _RegisterCommand ?? (_RegisterCommand = new Command(Register));

        string _Email;
        public string Email
        {
            get => _Email;
            set => SetProperty(ref _Email, value);
        }

        string _Contraseña;
        public string Contraseña
        {
            get => _Contraseña;
            set => SetProperty(ref _Contraseña, value);
        }

        string _MessageLabel = "";
        public string MessageLabel
        {
            get => _MessageLabel;
            set => SetProperty(ref _MessageLabel, value);
        }

        private async void Register(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one

            await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PushAsync(new UserDetailView());
            
        }

        public LoginViewModel()
        {

        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(RidesView)}");}

            ResponseModel response;
            try
            {
                UserModel user = new UserModel
                {
                    Email = Email,
                    Password = Contraseña
                };
                response = await new ApiService().PatchDataAsync("User", user);
            }
            catch (Exception ex)
            {

                throw;
            }

            if (response.IsSuccess)
            {
                UserModel userFinal = JsonConvert.DeserializeObject<UserModel>(response.Result.ToString());                
                int id = userFinal.IDUser;
                await Application.Current.MainPage.Navigation.PopAsync();
                //await Application.Current.MainPage.Navigation.PushAsync(new RidesView(id, userFinal));
            }
            else
            {
                MessageLabel = response.Message;
            }
            
            await Application.Current.MainPage.Navigation.PopAsync();
            
        }
    }
}
