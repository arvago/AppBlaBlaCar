﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBlaBlaCar.ViewModels
{
    public class RideDetailViewModel : BaseViewModel
    {
        //INICIALIZAMOS NUESTROS COMANDOS QUE SE UTILIZARAN AL MANEJAR EL DETAIL VIEW
        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));
        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));
        Command _MapCommand;
        public Command MapCommand => _MapCommand ?? (_MapCommand = new Command(MapAction));

        //SE CREAN LOS CONSTRUCTORES PARA PODER SETEAR LOS VALORES
        RideModel rideSelected;
        public RideModel RideSelected
        {
            get => rideSelected;
            set => SetProperty(ref rideSelected, value);
        }

        int _RideID;
        int _DriverID;

        string _OriginName;
        public string OriginName
        {
            get => _OriginName;
            set => SetProperty(ref _OriginName, value);
        }

        double _LatitudeOrg;
        public double LatitudeOrg
        {
            get => _LatitudeOrg;
            set => SetProperty(ref _LatitudeOrg, value);
        }

        double _LongitudeOrg;
        public double LongitudeOrg
        {
            get => _LongitudeOrg;
            set => SetProperty(ref _LongitudeOrg, value);
        }

        string _DestinationName;
        public string DestinationName
        {
            get => _DestinationName;
            set => SetProperty(ref _DestinationName, value);
        }

        double _LatitudeDes;
        public double LatitudeDes
        {
            get => _LatitudeDes;
            set => SetProperty(ref _LatitudeDes, value);
        }

        double _LongitudeDes;
        public double LongitudeDes
        {
            get => _LongitudeDes;
            set => SetProperty(ref _LongitudeDes, value);
        }

        string _Date;
        public string Date
        {
            get => _Date;
            set => SetProperty(ref _Date, value);
        }

        string _Time;
        public string Time
        {
            get => _Time;
            set => SetProperty(ref _Time, value);
        }

        int _Passengers;
        public int Passengers
        {
            get => _Passengers;
            set => SetProperty(ref _Passengers, value);
        }

        double _Price;
        public double Price
        {
            get => _Price;
            set => SetProperty(ref _Price, value);
        }

        //CONSTRUCTOR QUE SE INVOCA AL QUERER CREAR UNA NUEVA GASOLINERA
        public RideDetailViewModel(/*int IDDriver*/)
        {
            RideSelected = new RideModel();
            //_DriverID = IDDriver;
        }

        //CONSTRUCTOR QUE SE INVOCA AL QUERER EDITAR/ACTUALIZAR LA INFO DE UNA GASOLINERA
        public RideDetailViewModel(RideModel rideSelected)
        {
            RideSelected = rideSelected;

            _RideID = rideSelected.IDRide;
            _DriverID = rideSelected.IDDriver;            
            OriginName = rideSelected.OriginStr;
            LatitudeOrg = rideSelected.OriginLat;
            LongitudeOrg = rideSelected.OriginAlt;
            DestinationName = rideSelected.DestinationStr;
            LatitudeDes = rideSelected.DestinationLat;
            LongitudeDes = rideSelected.DestinationAlt;
            //Date =
            //Time =
            Passengers = rideSelected.Passengers;
            Price = rideSelected.Price;
        }

        private async void SaveAction()
        {
            ResponseModel response;
            try
            {
                DateTime fecha = Convert.ToDateTime(Date);
                DateTime hora = Convert.ToDateTime(Time);
                RideModel ride = new RideModel
                {
                    IDRide = _RideID,
                    IDDriver = _DriverID,
                    OriginStr = OriginName,
                    DestinationStr = DestinationName,
                    OriginLat = LatitudeOrg,
                    OriginAlt = LongitudeOrg,
                    DestinationLat = LatitudeDes,
                    DestinationAlt = LongitudeDes,
                    Passengers = Passengers,
                    Date = fecha.AddHours(hora.Hour).AddMinutes(hora.Minute).AddSeconds(hora.Second),
                    Price = Price
                };
                if (ride.IDRide > 0)
                {
                    //Actualizar
                    response = await new ApiService().PutDataAsync("Ride", ride);
                }
                else
                {
                    //Insertar
                    response = await new ApiService().PostDataAsync("Ride", ride);
                }
                if (response == null || !response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AppPets", $"Error al cargar los viajes {response.Message}", "Ok");
                    return;
                }

                RidesViewModel.GetInstance().LoadRides();
                await Application.Current.MainPage.Navigation.PopAsync();//LINEA PARA RECARGAR LOS DATOS GUARDADOS 
            }
            catch (Exception)
            {

                throw;
            }            
        }

        private async void DeleteAction()
        {
            ResponseModel response;
            try
            {
                if (_RideID > 0)
                {
                    response = await new ApiService().DeleteDataAsync("Ride", _RideID);
                }
                RidesViewModel.GetInstance().LoadRides();
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MapAction()
        {
            DateTime fecha = Convert.ToDateTime(Date);
            DateTime hora = Convert.ToDateTime(Time);
            Application.Current.MainPage.Navigation.PushAsync(
                new RidesMapsView(new RideModel
                {
                    IDRide = _RideID,
                    IDDriver = _DriverID,
                    OriginStr = OriginName,
                    DestinationStr = DestinationName,
                    OriginLat = LatitudeOrg,
                    OriginAlt = LongitudeOrg,
                    DestinationLat = LatitudeDes,
                    DestinationAlt = LongitudeDes,
                    Passengers = Passengers,
                    Date = fecha.AddHours(hora.Hour).AddMinutes(hora.Minute).AddSeconds(hora.Second),
                    Price = Price
                })
            );
        }
    }
}
