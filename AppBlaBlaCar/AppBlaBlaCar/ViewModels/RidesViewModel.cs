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
    public class RidesViewModel : BaseViewModel
    {
        private static RidesViewModel instance;

        UserModel actualUser;

        //CREAMOS NUESTRO COMANDO QUE SE UTILIZARA HACIENDO REFERENCIA A SU METODO
        Command _NewRideCommand;
        public Command NewRideCommand => _NewRideCommand ?? (_NewRideCommand = new Command(NewRideAction));

        List<RideModel> _RideList;
        public List<RideModel> RideList
        {
            get => _RideList;
            set => SetProperty(ref _RideList, value);
        }

        RideModel rideSelected;
        public RideModel RideSelected
        {
            get => rideSelected;
            set
            {
                if (
                SetProperty(ref rideSelected, value))
                {
                    SelectAction();
                }
            }
        }

        //CARGA LA LISTA DESDE EL PRINCIPIO DE LA APLICACION
        public RidesViewModel(/*UserModel user*/)
        {
            instance = this;
            //actualUser = user;
            LoadRides();
        }
        //METODO UNICO PARA RETORNAR NUESTRA INSTANCIA
        public static RidesViewModel GetInstance()
        {
            return instance;
        }

        //METODO PARA OBTENER TODOS LOS VIAJES
        public async void LoadRides()
        {
            ResponseModel response = await new ApiService().GetDataAsync("Ride");
            if (response == null || !response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", $"Error al cargar los viajes{response.Message}", "Ok");
                return;
            }
            RideList = JsonConvert.DeserializeObject<List<RideModel>>(response.Result.ToString());
        }

        //METODO PARA INVOCAR AL DETAILVIEW PARA AGREGAR UN VIAJE
        private void NewRideAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new RideDetailView(/*actualUser*/));

        }

        private void SelectAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new RideDetailView(rideSelected));

        }

    }
}
