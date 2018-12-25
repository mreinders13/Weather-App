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
using Android.Locations;

using Android.Net;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections;


using Java.Net;
using Java.IO;
using Android.Provider;
using Android.Database;

namespace ReindersMichaelAssn4
{
    [Activity(Label = "ReindersMichaelAssn4", MainLauncher = true)]
    public class MainActivity : Activity, ILocationListener
    {
        // public List<WeatherObservation> NewWeatherObservation = new List<WeatherObservation>();
        private string locationProvider;
        private LocationManager locationManager;
        public string url = Global.Url;
        HttpWebResponse httpResponse;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Set text views
            TextView tvCurrentLong = (TextView)FindViewById(Resource.Id.tvCurrentLong);
            TextView tvCurrentLat = (TextView)FindViewById(Resource.Id.tvCurrentLat);

            // Set Button 
            Button btnGetWeatherForcast = (Button)FindViewById(Resource.Id.btnGetWeatherForcast);
            btnGetWeatherForcast.Click += btnGetWeatherForcast_Click;

            // Get LocationManager
            locationManager = (LocationManager)GetSystemService(Context.LocationService);

            // Get Android.Locations.Criteria to set accuracy and power requirements
            Android.Locations.Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Android.Locations.Accuracy.Coarse
            };

            // Get Location Providers 
            IList<string> acceptableLocationProviders = 
                locationManager.GetProviders(criteriaForLocationService, true);

            // Test if list is empty
            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
                Toast.MakeText(this, "EXCEPTION THROWN LINE 88", ToastLength.Long).Show();
            }

            Location locationCurrent = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);

            // Display the current Long and Lat
            Global.Longitude = locationCurrent.Longitude;
            Global.Latitude = locationCurrent.Latitude;
            tvCurrentLong.Text = "Long: " + Math.Round(Global.Longitude, 3);
            tvCurrentLat.Text = "Lat: " + Math.Round(Global.Latitude, 3);
        }
        // Register the location updates when the activity is active.
        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(locationProvider, 1, 1, this);
        }
        public void OnLocationChanged(Location location)
        {
            // Set text views
            TextView tvCurrentLong = (TextView)FindViewById(Resource.Id.tvCurrentLong);
            TextView tvCurrentLat = (TextView)FindViewById(Resource.Id.tvCurrentLat);
            Global.Longitude = location.Longitude;
            Global.Latitude = location.Latitude;
            tvCurrentLong.Text = "Long: " + Math.Round(Global.Longitude, 3);
            tvCurrentLat.Text = "Lat: " + Math.Round(Global.Latitude, 3);
        }

        public void OnProviderDisabled(string provider)
        {
            locationManager.RemoveUpdates(this);
        }

        public void OnProviderEnabled(string provider)
        {
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            // nothing
        }

        private void btnGetWeatherForcast_Click(object sender, EventArgs e)
        {
            TextView tvUrl = (TextView)FindViewById(Resource.Id.tvUrl);
            string Long = Math.Round(Global.Longitude, 3).ToString();
            string Lat = Math.Round(Global.Latitude, 3).ToString();
            Global.Url = "https://" + "forecast.weather.gov/MapClick.php?lat=" + Lat + "&lon=" + Long + "&unit=0&lg=english&FcstType=dwml";
            tvUrl.Text = Global.Url.ToString();


            XmlDocument d;
            d = GetXmlFromURL(url);

            System.Xml.XmlNodeList nlData = d.GetElementsByTagName("data");

            foreach (System.Xml.XmlNode nData in nlData)
            {
                if (nData.Attributes["type"].Value == "forecast")
                {
                    for (int i = 1; i < 14; i++)
                    {
                        string s1, s2, s3, s4;
                        s1 = nData.ChildNodes[2].ChildNodes[i].Attributes["period-name"].Value;
                        s2 = nData.ChildNodes[2].ChildNodes[i].ChildNodes[0].Value;
                        s3 = nData.ChildNodes[5].ChildNodes[3].ChildNodes[i].Attributes["weather-summary"].Value;
                        s4 = nData.ChildNodes[5].ChildNodes[5].ChildNodes[i].ChildNodes[0].Value;

                        //Store WeatherObservation in array
                        Global.NewWeatherObservation.Add(new WeatherObservation(s1, s2, s3, s4));
                    }
                }
            }

            

           

            // Create Array Adapter
            ListView lvList = (ListView)FindViewById(Resource.Id.lvList);
            MyListViewAdapter adapter = new MyListViewAdapter(this, Global.NewWeatherObservation);

            lvList.Adapter = adapter;

        }
        protected XmlDocument GetXmlFromURL(string argURL)
        {
            TextView tvUrl = (TextView)FindViewById(Resource.Id.tvUrl);
            // Create the XmlDocument object.
            XmlDocument d = new XmlDocument();
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(Global.Url);

                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/html";
                request.UserAgent = @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";

                // Here we make the request and prepare the response object.
                httpResponse = (HttpWebResponse)request.GetResponse();

                // Connect the HTTP response stream to our .NET StreamReader; 
                Stream s = httpResponse.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                string resultString = sr.ReadToEnd();
                d.LoadXml(resultString);
            }

            catch (Exception)
            {
                HttpStatusCode i = httpResponse.StatusCode;
                tvUrl.Text = "HTTP response code: " + i.ToString();
            }
            return d;
        }

    }
}

