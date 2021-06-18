using AppBlaBlaCar.Models;
using AppBlaBlaCar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBlaBlaCar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDetailView : ContentPage
    {
        public UserDetailView()
        {
            InitializeComponent();
            BindingContext = new UserDetailViewModel();
        }

        //CONSTRUCTOR QUE SE INVOCA PARA ACTUALIZAR EL USUARIO
        public UserDetailView(UserModel actualUser)
        {
            InitializeComponent();
            //ENVIA Y OBTIENE LOS BINDINGS DEL VIEW MODEL
            BindingContext = new UserDetailViewModel(actualUser);
        }
    }
}