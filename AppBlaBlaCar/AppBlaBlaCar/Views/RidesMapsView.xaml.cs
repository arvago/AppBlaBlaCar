using AppBlaBlaCar.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppBlaBlaCar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RidesMapsView : ContentPage
    {
        public RidesMapsView(RideModel rideSelected)
        {
            InitializeComponent();
            MapRides.Ride = rideSelected;

            //CENTRA EL MAPA EN EL DESTINO GUARDADO DEL VIAJE
            MapRides.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(
                        rideSelected.DestinationLat,
                        rideSelected.DestinationAlt
                        ),
                        Distance.FromMiles(.5)
                    )
                );

            //AGREGA EL PIN EN LAS UBICACIONES GUARDADAS DEL VIAJE
            MapRides.Pins.Add(
                new Pin
                {
                    Type = PinType.Place,
                    Label = rideSelected.OriginStr,
                    Position = new Position(
                        rideSelected.OriginLat,
                         rideSelected.OriginAlt
                        )
                }
            );

            MapRides.Pins.Add(
                new Pin
                {
                    Type = PinType.Place,
                    Label = rideSelected.DestinationStr,
                    Position = new Position(
                        rideSelected.DestinationLat,
                         rideSelected.DestinationAlt
                        )
                }
            );
        }
    }
}