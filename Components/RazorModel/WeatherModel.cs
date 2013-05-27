using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Components.RazorModel
{
    public class WeatherModel
    {
        string data = null;

        public WeatherModel(string xmlData)
        {
            data = xmlData;

            //load all the data
            using (var reader = new StringReader(data))
            {
                var xPath = new XPathDocument(reader); 
                var nav = xPath.CreateNavigator();
                Title = nav.SelectSingleNode("//item/title").Value;
            }
        }

        public string Title { get; set; }

        public string PublishDate { get; set; }

        public LocationData Location { get; set; }

        public UnitData Units { get; set; }

        public WindData Wind { get; set; }

        public AtmosphereData Atmosphere { get; set; }

        public AstronomyData Astronomy { get; set; }

        public ConditionData CurrentCondition { get; set; }

        public IList<ForecastData> Forecast { get; set; }

        public class LocationData
        {
            public string City { get; set; }

            public string Region { get; set; }

            public string Country { get; set; }

            public decimal Lat { get; set; }

            public decimal Long { get; set; }
        }

        public class UnitData
        {
            public string Temperature { get; set; }

            public string Distance { get; set; }

            public string Pressure { get; set; }

            public string Speed { get; set; }
        }

        public class WindData
        {
            public decimal Chill { get; set; }

            public int Direction { get; set; }

            public decimal Speed { get; set; }
        }

        public class AtmosphereData
        {
            public decimal Humidity { get; set; }

            public decimal Visibility { get; set; }

            public decimal Pressure { get; set; }

            public decimal Rising { get; set; }
        }

        public class AstronomyData
        {
            public DateTime SunRise { get; set; }

            public DateTime SunSet { get; set; }
        }


        public class ConditionData
        {
            public string Caption { get; set; }

            public int Code { get; set; }

            public int Temperture { get; set; }

            public DateTime Date { get; set; }
        }

        public class ForecastData
        {
            public string Day { get; set; }

            public DateTime Date { get; set; }

            public int TempLow { get; set; }

            public int TempHigh { get; set; }

            public int Code { get; set; }

            public string Caption { get; set; }
        }
    }
}
