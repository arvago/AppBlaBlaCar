using AppBlaBlaCar.ViewModels;
using AppBlaBlaCar.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppBlaBlaCar
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(RidesView), typeof(RidesView));
            Routing.RegisterRoute(nameof(UserDetailView), typeof(UserDetailView));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginView");
        }

    }
}
