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
using System.Net;
using System.IO;
using System.Xml;

namespace ReindersMichaelAssn4
{
    public class MyListViewAdapter : BaseAdapter<WeatherObservation>
    {
        public List<WeatherObservation> NewWeatherObservation;
        private Activity mContext;
        public MyListViewAdapter(Activity context, List<WeatherObservation> observation)
            :base()
        {
            this.NewWeatherObservation = observation;
            this.mContext = context;
        }
        public override int Count
        {
            get { return NewWeatherObservation.Count;  }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override WeatherObservation this[int position]
        {
            get { return NewWeatherObservation[position];  }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = NewWeatherObservation[position];

            View view = convertView;
            if(view == null)
            {
                view = mContext.LayoutInflater.Inflate(Resource.Layout.Row, null);
            }

            TextView txtPeriod = view.FindViewById<TextView>(Resource.Id.tvPeriodName);
            txtPeriod.Text = NewWeatherObservation[position].periodName.ToString();
            TextView txtDaTE = view.FindViewById<TextView>(Resource.Id.tvValidDate);
            txtDaTE.Text = NewWeatherObservation[position].validDate.ToString();
            TextView txtWeatherSummary = view.FindViewById<TextView>(Resource.Id.tvWeatherSummary);
            txtWeatherSummary.Text = NewWeatherObservation[position].weatherSummary.ToString();
            //TextView txtWordedSummary = view.FindViewById<TextView>(Resource.Id.tvWordedSummary);
            //txtWordedSummary.Text = NewWeatherObservation[position].wordedForecast.ToString();

            return view;
        }
    }
}