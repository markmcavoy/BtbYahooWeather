using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Entities.Modules;
using System.Collections;
using BiteTheBullet.DNN.Modules.BTBYahooWeather.Extensions;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Components
{
    public class BTBYahooWeatherSettings
    {
        int tabModuleId;
        Hashtable settings = null;

        private const string USER_PERSONIALISATION = "BTBYahooWeather_UserPersist";
        private const string RENDER_ENGINE = "BTBYahooWeather_RenderEngine";
        private const string TEMPLATE_NAME = "BTBYahooWeather_Template_Name";

        /// <summary>
        /// defines the different rendering engines we 
        /// have to transform the xml data into HTML
        /// </summary>
        public enum TemplateEngine
        {
            Xlst,
            Razor
        }

        public BTBYahooWeatherSettings(int tabModuleId)
        {
            this.tabModuleId = tabModuleId;

            ModuleController mc = new ModuleController();
            settings = mc.GetTabModuleSettings(this.tabModuleId);
        }

        public Hashtable Settings
        {
            get
            {
                if (settings == null)
                {
                    ModuleController mc = new ModuleController();
                    settings = mc.GetTabModuleSettings(this.tabModuleId);
                }

                return settings;
            }
        }

        public TemplateEngine RenderEngine 
        {
            get
            {
                object setting = Settings[RENDER_ENGINE];
                var result = TemplateEngine.Xlst;

                if (setting != null)
                    result = setting.ToString().ToEnum<TemplateEngine>();

                return result;
            }
            set
            {
                new ModuleController().UpdateTabModuleSetting(tabModuleId, RENDER_ENGINE, value.ToString());
                settings = null;
            } 
        }

        public string TemplateName 
        {
            get 
            {
                object setting = Settings[TEMPLATE_NAME];
                
                if (setting != null)
                    return setting.ToString();

                //if we get here is means we don't have a template
                //selected so we should just grab a default one
                switch (RenderEngine)
                {
                    case TemplateEngine.Razor:
                        return "weatherReport.cshtml";

                    case TemplateEngine.Xlst:
                    default:
                        return "weatherReport.xsl";
                }
            }

            set
            {
                new ModuleController().UpdateTabModuleSetting(tabModuleId, TEMPLATE_NAME, value.ToString());
                settings = null;
            }
        }

        /// <summary>
        /// defines if the user can selected their preferred weather location
        /// </summary>
        public bool AllowUserPersonialisation
        {
            get 
            {
                object setting = Settings[USER_PERSONIALISATION];
                bool result = false;

                if (setting != null)
                    bool.TryParse(setting.ToString(), out result);

                return result;
            }
            
            set 
            {
                new ModuleController().UpdateTabModuleSetting(tabModuleId, USER_PERSONIALISATION, value.ToString());
                settings = null;
 
            }
        }
        
    }
}
