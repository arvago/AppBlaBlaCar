using AppBlaBlaCar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace AppBlaBlaCar.Renders
{
    //SE CREA LA CLASE MyMap, que se utilizara para el futuro render en el mapa
    public class MyMap : Map
    {
        public RideModel Ride; //Iniciamos nuestra variable publica del tipo RideModel
    }
}
