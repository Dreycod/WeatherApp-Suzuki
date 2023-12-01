using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Service
{
    internal class MainViewModel
    {
        public MainViewModel(FcstDay0 forecastDay0, FcstDay1 forecastDay1, FcstDay2 forecastDay2, FcstDay3 forecastDay3, FcstDay4 forecastDay4, bool Max)
        {
            var model = new PlotModel { };

            //  Températures max et min
            if (Max)
            {
                var maxTempSeries = new LineSeries
                {
                    Title = "Max Temperature",
                    Color = OxyColors.Red
                };

                maxTempSeries.Points.Add(new DataPoint(0, forecastDay0.tmax));
                maxTempSeries.Points.Add(new DataPoint(1, forecastDay1.tmax));
                maxTempSeries.Points.Add(new DataPoint(2, forecastDay2.tmax));
                maxTempSeries.Points.Add(new DataPoint(3, forecastDay3.tmax));
                maxTempSeries.Points.Add(new DataPoint(4, forecastDay4.tmax));

                model.Series.Add(maxTempSeries);
            }
            else
            {

                var minTempSeries = new LineSeries
                {
                    Title = "Min Temperature",
                    Color = OxyColors.Blue
                };

                minTempSeries.Points.Add(new DataPoint(0, forecastDay0.tmin));
                minTempSeries.Points.Add(new DataPoint(1, forecastDay1.tmin));
                minTempSeries.Points.Add(new DataPoint(2, forecastDay2.tmin));
                minTempSeries.Points.Add(new DataPoint(3, forecastDay3.tmin));
                minTempSeries.Points.Add(new DataPoint(4, forecastDay4.tmin));

                model.Series.Add(minTempSeries);
            }

            this.Model = model;
        }

        public PlotModel Model { get; private set; }
    }
}
