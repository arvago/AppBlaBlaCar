using System;
using System.Collections.Generic;
using System.Text;

namespace AppBlaBlaCar.Models
{
    public class RideModel
    {
        public int IDRide { get; set; }
        public int IDDriver { get; set; }
        public string OriginStr { get; set; }
        public string DestinationStr { get; set; }
        public double OriginLat { get; set; }
        public double OriginAlt { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationAlt { get; set; }
        public int Passengers { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
