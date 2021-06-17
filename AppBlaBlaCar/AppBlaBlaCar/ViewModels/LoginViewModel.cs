using AppBlaBlaCar.Views;
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

        private async void Register(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Application.Current.MainPage.Navigation.PushAsync(new UserDetailView());
        }

        public LoginViewModel()
        {

        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(RidesView)}");
        }
    }
}
