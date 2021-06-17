using AppBlaBlaCar.Models;
using AppBlaBlaCar.Renders;
using AppBlaBlaCar.UWP.Renders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace AppBlaBlaCar.UWP.Renders
{
    public class MyMapRenderer : MapRenderer
    {
        MapControl NativeMap;
        RideModel Ride;
        MapWindow RideWindow;
        bool IsRideWindowVisible = false; //SI EL CUADRO ESTA VISIBLE

        //LIMPIA LA INGORMACION QUE TENIA POR DEFECTO PARA CENTRAR EL MAPA Y EL PIN PERSONALIZADO
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.MapElementClick -= OnMapElementClick;
                NativeMap.Children.Clear();
                NativeMap = null;
                RideWindow = null;
            }

            if (e.NewElement != null)
            {
                this.Ride = (e.NewElement as MyMap).Ride;

                var formsMap = (MyMap)e.NewElement;
                NativeMap = Control as MapControl;
                NativeMap.Children.Clear();
                NativeMap.MapElementClick += OnMapElementClick;

                //POSICION DEL PIN
                var positionOrg = new BasicGeoposition
                {
                    Latitude = Ride.OriginLat,
                    Longitude = Ride.OriginAlt
                };
                var positionDes = new BasicGeoposition
                {
                    Latitude = Ride.DestinationLat,
                    Longitude = Ride.DestinationAlt
                };

                var pointOrg = new Geopoint(positionOrg);
                var pointDes = new Geopoint(positionDes);

                //POLYLINES
                var coordinates = new List<BasicGeoposition>();
                coordinates.Add(new BasicGeoposition() { Latitude = Ride.OriginLat, Longitude = Ride.OriginAlt });
                coordinates.Add(new BasicGeoposition() { Latitude = Ride.DestinationLat, Longitude = Ride.DestinationAlt });

                var polyline = new MapPolyline();
                polyline.StrokeColor = Windows.UI.Color.FromArgb(128, 255, 0, 0);
                polyline.StrokeThickness = 5;
                polyline.Path = new Geopath(coordinates);

                NativeMap.MapElements.Add(polyline);

                //ATRIBUTOS DE NUESTRO MAPICON, SU FILEPATH, LOCACION, ANCHURA, ETC.
                var mapIconOrg = new MapIcon();
                mapIconOrg.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin.png"));
                mapIconOrg.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                mapIconOrg.Location = pointOrg;
                mapIconOrg.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);

                var mapIconDes = new MapIcon();
                mapIconDes.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin.png"));
                mapIconDes.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                mapIconDes.Location = pointDes;
                mapIconDes.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);

                NativeMap.MapElements.Add(mapIconOrg);
                NativeMap.MapElements.Add(mapIconDes);
            }
        }

        //METODO PARA MOSTRAR EL RECUADRO CON LA INFO EN EL MAPA, AL DAR CLICK AL PIN
        private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            var mapicon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            if (mapicon != null)
            {
                if (!IsRideWindowVisible)
                {
                    //MANDA LOS DATOS AL MAPWINDOW PARA MOSTRAR EL RECUEADRO CON LA INFO
                    if (RideWindow == null) RideWindow = new MapWindow(Ride);
                    var position = new BasicGeoposition
                    {
                        Latitude = Ride.DestinationLat,
                        Longitude = Ride.DestinationLat
                    };
                    var point = new Geopoint(position);

                    NativeMap.Children.Add(RideWindow);
                    MapControl.SetLocation(RideWindow, point);
                    MapControl.SetNormalizedAnchorPoint(RideWindow, new Windows.Foundation.Point(0.5, 1.0));

                    IsRideWindowVisible = true;
                }
                else
                {
                    NativeMap.Children.Remove(RideWindow);

                    IsRideWindowVisible = false;
                }
            }
        }
    }
}
