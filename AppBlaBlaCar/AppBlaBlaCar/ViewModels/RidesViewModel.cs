﻿using AppBlaBlaCar.Models;
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

        //CREAMOS NUESTRO COMANDO QUE SE UTILIZARA HACIENDO REFERENCIA A SU METODO
        Command _NewRideCommand;
        public Command NewRideCommand => _NewRideCommand ?? (_NewRideCommand = new Command(NewRideAction));

        List<RideModel> rides;
        public List<RideModel> Rides
        {
            get => rides;
            set => SetProperty(ref rides, value);
        }

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
        public RidesViewModel()
        {
            instance = this;
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
            ResponseModel response = await new ApiService().GetDataAsync("Rides");
            if (response == null || !response.IsSuccess)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", $"Error al cargar los viajes{response.Message}", "Ok");
                return;
            }
            RideList = JsonConvert.DeserializeObject<List<RideModel>>(response.Result.ToString());
            IsBusy = false;
        }

        //METODO PARA INVOCAR AL DETAILVIEW PARA AGREGAR UN VIAJE
        private void NewRideAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new RideDetailView());

        }

        private void SelectAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new RideDetailView(rideSelected));

        }

    }
}
