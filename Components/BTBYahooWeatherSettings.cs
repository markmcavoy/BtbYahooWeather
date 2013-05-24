using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Entities.Modules;
using System.Collections;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Components
{
    public class BTBYahooWeatherSettings
    {
        int tabModuleId;
        Hashtable settings;

        public BTBYahooWeatherSettings(int tabModuleId)
        {
            this.tabModuleId = tabModuleId;

            ModuleController mc = new ModuleController();
            settings = mc.GetTabModuleSettings(this.tabModuleId);
        }
    }
}
