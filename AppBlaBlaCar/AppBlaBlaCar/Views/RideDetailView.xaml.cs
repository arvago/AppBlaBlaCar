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
    public partial class RideDetailView : ContentPage
    {
        public RideDetailView(int userID)
        {
            InitializeComponent();
            //ENVIA Y OBTIENE LOS BINDINGS DEL VIEW MODEL
            BindingContext = new RideDetailViewModel(userID);
        }

        //CONSTRUCTOR QUE SE INVOCA PARA ACTUALIZAR UNA GASOLINERA
        public RideDetailView(RideModel rideSelected, UserModel user)
        {
            InitializeComponent();
            //ENVIA Y OBTIENE LOS BINDINGS DEL VIEW MODEL
            BindingContext = new RideDetailViewModel(rideSelected, user);
        }
    }
}