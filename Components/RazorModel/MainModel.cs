using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Components.RazorModel
{
    public class MainModel
    {
        public MainModel(int moduleId, string xmlData)
        {
            Weather = new WeatherModel(xmlData);

            //load the available locations into the dictionary
            Locations = DataProvider.GetBTBWeatherFeedByModule(moduleId)
                            .ToDictionary(w => w.WeatherId, w => w.LocationName);
        }

        public WeatherModel Weather { get; set; }

        public IDictionary<int, string> Locations { get; set; }
    }
}
