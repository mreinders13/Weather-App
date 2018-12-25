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
    public class WeatherObservation
    {
        public string periodName { get; set; }
        public string validDate { get; set; }
        public string weatherSummary { get; set; }
        public string wordedForecast { get; set; }

        public WeatherObservation(string s1, string s2, string s3, string s4)
            {
                this.periodName = s1;
                this.validDate = s2;
                this.weatherSummary = s3;
                this.wordedForecast = s4;
            }
    }    
}