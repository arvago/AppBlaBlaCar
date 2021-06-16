using AppBlaBlaCar.Models;
using AppBlaBlaCar.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBlaBlaCar.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public UserModel userActual;

        Command _LoginCommand;
        public Command LoginCommand => _LoginCommand ?? (_LoginCommand = new Command(OnLoginClicked));

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
