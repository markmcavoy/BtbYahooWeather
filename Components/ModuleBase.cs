using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Entities.Modules;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Components
{
    public abstract class ModuleBase : PortalModuleBase
    {
        BTBYahooWeatherSettings settings = null;

        /// <summary>
        /// gets the module settings
        /// </summary>
        protected BTBYahooWeatherSettings ModuleSettings
        {
            get
            {
                if (settings == null)
                {
                    settings = new BTBYahooWeatherSettings(TabModuleId);
                }

                return settings;
            }

        }
    }
}
