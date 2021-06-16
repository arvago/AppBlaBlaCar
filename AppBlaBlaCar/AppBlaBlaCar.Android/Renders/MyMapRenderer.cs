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
            LatLng coorOrg = new LatLng(Ride.OriginLat, Ride.OriginAlt);
            LatLng coorDes = new LatLng(Ride.DestinationLat, Ride.DestinationAlt);
            map.AddMarker(new MarkerOptions().SetPosition(coorOrg).SetTitle($"Origen: {Ride.OriginStr}"));
            map.AddMarker(new MarkerOptions().SetPosition(coorDes).SetTitle($"Destino: {Ride.DestinationStr}"));
            base.OnMapReady(map);

            NativeMap.SetInfoWindowAdapter(this);
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
                Android.Views.View viewOrg;
                Android.Views.View viewDes;

                viewOrg = inflater.Inflate(Resource.Layout.MapWindowOrg, null);
                viewDes = inflater.Inflate(Resource.Layout.MapWindowDes, null);
                var infoNameOrg = viewOrg.FindViewById<TextView>(Resource.Id.MapWindowName);
                var infoNameDes = viewDes.FindViewById<TextView>(Resource.Id.MapWindowName);

                if ((infoNameOrg != null) || (infoNameDes != null))
                {
                    infoNameOrg.Text = Ride.OriginStr;
                    infoNameOrg.Text = Ride.OriginStr;
                }
                return (viewOrg);
                return (viewDes);
            }
            return null;
        }

        


        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }
    }
}