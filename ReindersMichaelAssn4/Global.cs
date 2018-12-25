using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReindersMichaelAssn4
{
    public static class Global
    {
        public static double Longitude = 0;
        public static double Latitude = 0;

        public static string Url = "";
    
        public static string XmlData = "";

        public static List<WeatherObservation> NewWeatherObservation = new List<WeatherObservation>();
    }
    
}

