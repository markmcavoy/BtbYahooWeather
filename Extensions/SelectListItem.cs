using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Extensions
{
    /// <summary>
    /// simple class to hold the data we are going bind to the drop down list
    /// </summary>
    public class SelectListItem
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public bool Selected { get; set; }
    }
}
