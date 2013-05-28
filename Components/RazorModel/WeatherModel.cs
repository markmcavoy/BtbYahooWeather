using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Xml;

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

                XmlNamespaceManager ns = new XmlNamespaceManager(nav.NameTable);
                ns.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
                ns.AddNamespace("geo", "http://www.w3.org/2003/01/geo/wgs84_pos#");

                Title = nav.SelectSingleNode("//item/title").Value;
                PublishDate = nav.SelectSingleNode("//lastBuildDate").Value;

                var location = new LocationData() 
                {
                    City = nav.SelectSingleNode("//yweather:location/@city", ns).Value,
                    Region = nav.SelectSingleNode("//yweather:location/@region", ns).Value,
                    Country = nav.SelectSingleNode("//yweather:location/@country", ns).Value,
                    Lat = decimal.Parse(nav.SelectSingleNode("//geo:lat", ns).Value),
                    Long = decimal.Parse(nav.SelectSingleNode("//geo:long", ns).Value)
                };
                Location = location;


                var units = new UnitData()
                {
                    Temperature = nav.SelectSingleNode("//yweather:units/@temperature", ns).Value,
                    Distance = nav.SelectSingleNode("//yweather:units/@distance", ns).Value,
                    Pressure = nav.SelectSingleNode("//yweather:units/@pressure", ns).Value,
                    Speed = nav.SelectSingleNode("//yweather:units/@speed", ns).Value
                };
                Units = units;


                var wind = new WindData()
                {
                    Chill = decimal.Parse(nav.SelectSingleNode("//yweather:wind/@chill", ns).Value),
                    Direction = int.Parse(nav.SelectSingleNode("//yweather:wind/@direction", ns).Value),
                    Speed = decimal.Parse(nav.SelectSingleNode("//yweather:wind/@speed", ns).Value)
                };
                Wind = wind;


                var atmos = new AtmosphereData()
                {
                    Humidity = decimal.Parse(nav.SelectSingleNode("//yweather:atmosphere/@humidity", ns).Value),
                    Visibility = decimal.Parse(nav.SelectSingleNode("//yweather:atmosphere/@visibility", ns).Value),
                    Pressure = decimal.Parse(nav.SelectSingleNode("//yweather:atmosphere/@pressure", ns).Value),
                    Rising = decimal.Parse(nav.SelectSingleNode("//yweather:atmosphere/@rising", ns).Value)
                };
                Atmosphere = atmos;


                var astro = new AstronomyData()
                {
                    SunRise = DateTime.Parse(nav.SelectSingleNode("//yweather:astronomy/@sunrise", ns).Value),
                    SunSet = DateTime.Parse(nav.SelectSingleNode("//yweather:astronomy/@sunset", ns).Value)
                };
                Astronomy = astro;


                var conditions = new ConditionData()
                {
                    Caption = nav.SelectSingleNode("//yweather:condition/@text", ns).Value,
                    Code = int.Parse(nav.SelectSingleNode("//yweather:condition/@code", ns).Value),
                    Temperture = int.Parse(nav.SelectSingleNode("//yweather:condition/@temp", ns).Value),
                    Date = DateTime.Parse(nav.SelectSingleNode("//yweather:condition/@date", ns).Value.Substring(0, nav.SelectSingleNode("//yweather:condition/@date", ns).Value.Length- 3))
                };
                CurrentCondition = conditions;

                var forecasts = new List<ForecastData>();
                foreach (XPathNavigator item in nav.Select("//yweather:forecast", ns))
                {
                    forecasts.Add(new ForecastData() 
                    {
                        Day = item.SelectSingleNode("@day", ns).Value,
                        Date = DateTime.Parse(item.SelectSingleNode("@date", ns).Value),
                        TempLow = int.Parse(item.SelectSingleNode("@low", ns).Value),
                        TempHigh = int.Parse(item.SelectSingleNode("@high", ns).Value),
                        Code = int.Parse(item.SelectSingleNode("@code", ns).Value),
                        Caption = item.SelectSingleNode("@text", ns).Value
                    });
                }
                Forecast = forecasts;
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
