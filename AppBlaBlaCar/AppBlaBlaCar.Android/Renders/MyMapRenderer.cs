using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppBlaBlaCar.Renders;
using AppBlaBlaCar.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using AppBlaBlaCar.Droid.Renders;
using Android.Gms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Maps.Model;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace AppBlaBlaCar.Droid.Renders
{
    public class MyMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        RideModel Ride;

        public MyMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.Ride = (e.NewElement as MyMap).Ride;
            }

        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0x66FF0000);
            LatLng coorOrg = new LatLng(Ride.OriginLat, Ride.OriginAlt);
            LatLng coorDes = new LatLng(Ride.DestinationLat, Ride.DestinationAlt);
            map.AddMarker(new MarkerOptions().SetPosition(coorOrg).SetTitle($"Origen: {Ride.OriginStr}"));
            map.AddMarker(new MarkerOptions().SetPosition(coorDes).SetTitle($"Destino: {Ride.DestinationStr}"));
            polylineOptions.Add(coorOrg);
            polylineOptions.Add(coorDes);            

            NativeMap.SetInfoWindowAdapter(this);
            NativeMap.AddPolyline(polylineOptions);
        }

        /*protected override MarkerOptions CreateMarker(Pin pin)
        {
            //return base.CreateMarker(pin);
            var markerOrigin = new MarkerOptions();            

            markerOrigin.SetPosition(new LatLng(Ride.OriginLat, Ride.OriginAlt));
            markerOrigin.SetTitle($"Desde: {Ride.OriginStr}");            
            return (markerOrigin);
        }*/        

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View viewDes;
                viewDes = inflater.Inflate(Resource.Layout.MapWindowDes, null);
                var infoNameDes = viewDes.FindViewById<TextView>(Resource.Id.MapWindowName);

                if (infoNameDes != null)
                {
                    infoNameDes.Text = Ride.OriginStr;
                }
                
                return viewDes;
            }
            return null;
        }    

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }
    }
}