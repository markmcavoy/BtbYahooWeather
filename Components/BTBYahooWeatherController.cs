/*
 * Copyright (c) 2007, www.bitethebullet.co.uk
 * All rights reserved.

 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation 
 * and/or other materials provided with the distribution.
 * 
 * Neither the name of the www.bitethebullet.co.uk nor the names of its contributors 
 * may be used to endorse or promote products derived from this software without 
 * specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
 * OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
 * 
 * */

using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;

using BiteTheBullet.DNN.Utils;
using BiteTheBullet.DNN.Modules.BTBYahooWeather.Data;
using DotNetNuke.Entities.Host;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Business
{
	public class BTBWeatherFeedController : DotNetNuke.Entities.Modules.IPortable
	{
		/// <summary>
		/// The base url that we use to create the RSS feed from, we append the parameters
		/// as required to create the corret one.
		/// </summary>
        private const string BASE_URL = "http://weather.yahooapis.com/forecastrss";

		/// <summary>
		/// tabmodule setting key used to hold the display mode for the module
		/// </summary>
		private const string TABMODULE_DISPLAYMODE= "BTBYahooWeather_DisplayMode";

		/// <summary>
		/// defines the temperature scale we are using for this feed
		/// </summary>
		public enum TemperatureScale
		{
			Celsius,
			Fahrenheit
		}

		#region "Public Methods"
		
		/// <summary>
		/// Gets the default weather feed from the list of weatherinfo
		/// </summary>
		public BTBWeatherFeedInfo GetDefaultFeed(ArrayList weatherList)
		{
			if(weatherList.Count == 0)
				return null;
			
			foreach(BTBWeatherFeedInfo item in weatherList)
			{
				if(item.DefaultFeed)
					return item;
			}
			
			return (BTBWeatherFeedInfo)weatherList[0];
		}
		
		/// <summary>
		/// Sets the default weather feed for a module
		/// </summary>
		public void SetDefaultFeed(int moduleId, int weatherId)
		{
			DataProvider.Instance().SetDefaultFeed(moduleId, weatherId);
		}	

		/// <summary>
		/// Creates the url we need to ping in order to get the weather information for the
		/// given location
		/// </summary>
		/// <param name="weatherStationCode">Yahoo weather code for the location</param>
		/// <param name="tempScale">TemperatureScale we wish to receive the data in</param>
		/// <returns></returns>
		public string CreateFeedUrl(string weatherStationCode, TemperatureScale tempScale)
		{
			return BASE_URL + "?p=" + weatherStationCode + "&u=" + 
							(tempScale == TemperatureScale.Celsius ? "c" : "f");	
		}

		/// <summary>
		/// Returns the weather code from a feed URL
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public string ExtractWeatherCodeFromUrl(string url)
		{
			int startPos = url.IndexOf("=");
			int endPos = url.IndexOf("&");

            return url.Substring(++startPos, endPos - startPos);
		}

		/// <summary>
		/// Returns the TemperatureScale for the URL
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public TemperatureScale ExtractTemperatureScaleFromUrl(string url)
		{
			string temp = url.Substring(url.Length - 1);
			if(temp == "c")
				return TemperatureScale.Celsius;
			else
				return TemperatureScale.Fahrenheit;
		}


		public void UpdateWeatherFeed(BTBWeatherFeedInfo info, PortalSettings portalSettings, ModuleInfo moduleConfiguration)
		{
			string proxyHost	 = Host.GetHostSettingsDictionary()["ProxyServer"];
			string proxyPort     = Host.GetHostSettingsDictionary()["ProxyPort"];
			string proxyUsername = Host.GetHostSettingsDictionary()["ProxyUsername"];
			string proxyPassword = Host.GetHostSettingsDictionary()["ProxyPassword"];

			//version 1.6
			//26/8/2008
			//fix to check if we have a proxy port, in which case we should append to the proxyhost
			//Thanks to Corrie Meyer for finding this bug.
			string proxyUrl = proxyPort.Trim() == string.Empty ? proxyHost : proxyHost + ":" + proxyPort;
			
			RssReader reader = new RssReader(info.Url, proxyUrl, proxyUsername, proxyPassword);
			
			//get the raw feed
			info.Feed = reader.Response();

			XmlUrlResolver resolver = new XmlUrlResolver();
			resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

			XPathDocument doc = new XPathDocument(new XmlTextReader(info.Feed, XmlNodeType.Document, null));			
			MemoryStream ms = new MemoryStream();
			XslTransform transform = new XslTransform();

			//if we don't have a custom XSLT defined then we should just use the 
			//default one which we install with the module
			string xslFile = null;

			ModuleController mc = new ModuleController();
			Hashtable settings = mc.GetTabModuleSettings(moduleConfiguration.TabModuleID);
			if(settings[TABMODULE_DISPLAYMODE] != null)
			{
				string displayMode = settings[TABMODULE_DISPLAYMODE].ToString();

				/*
				 * Version 1.4 change 8/5/2008
				 * 
				 * The five day forecast is no longer available, due the not having the
				 * data in the RSS feed. Any option which used this should
				 * just use the correct 2 day forecast instead
				 * */
				switch(displayMode)
				{
					case "full":
					case "full5":
						xslFile = HttpContext.Current.Server.MapPath("~/DesktopModules/BTBYahooWeather/weatherReport.xsl");
						break;

					case "medium":
					case "medium5":
						xslFile = HttpContext.Current.Server.MapPath("~/DesktopModules/BTBYahooWeather/weatherReportMedium.xsl");
						break;

					case "summary":
						xslFile = HttpContext.Current.Server.MapPath("~/DesktopModules/BTBYahooWeather/weatherReportSmall.xsl");
						break;
				}

			}
			else
				xslFile = HttpContext.Current.Server.MapPath("~/DesktopModules/BTBYahooWeather/weatherReport.xsl"); 

			transform.Load(xslFile);
			transform.Transform(doc, null, ms, resolver);

			ms.Position = 0;
			info.TransformedFeed = new StreamReader(ms, System.Text.Encoding.UTF8).ReadToEnd();

			//clean up
			ms.Close();
			
			//get the ttl
			int ttl = reader.Ttl();
			if(ttl > -1)
				info.Ttl = reader.Ttl();
			else
				//no value of ttl in feed
				//default to 20 minutes
				info.Ttl = 20;

			//set the date stamps
			info.UpdatedDate = DateTime.Now;
			info.CachedDate = DateTime.Now.AddMinutes(info.Ttl);
			info.LocationName = reader.LocationName();

			//cache the feed in the database
			BTBWeatherFeedController controller = new BTBWeatherFeedController();
			controller.Update(info);
		}


		#region DAL methods

		public BTBWeatherFeedInfo Get(int weatherId)
		{
			return (BTBWeatherFeedInfo)DotNetNuke.Common.Utilities.CBO.FillObject(DataProvider.Instance().GetBTBWeatherFeed(weatherId), typeof(BTBWeatherFeedInfo));
		}

		/// <summary>
		/// Returns a list of WeatherInfo objects for the given module id
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		public ArrayList GetByModuleId(int moduleId)
		{
			return DotNetNuke.Common.Utilities.CBO.FillCollection(DataProvider.Instance().GetBTBWeatherFeedByModule(moduleId), typeof(BTBWeatherFeedInfo));
		}

	
		public int Add(BTBWeatherFeedInfo objBTBWeatherFeed)
		{
			return (int)DataProvider.Instance().AddBTBWeatherFeed(objBTBWeatherFeed.ModuleId, 
													objBTBWeatherFeed.Ttl, 
													objBTBWeatherFeed.UpdatedDate, 
													objBTBWeatherFeed.CachedDate, 
													objBTBWeatherFeed.Feed, 
													objBTBWeatherFeed.Url, 
													objBTBWeatherFeed.TransformedFeed,
													objBTBWeatherFeed.LocationName);
		}
	
		public void Update(BTBWeatherFeedInfo objBTBWeatherFeed)
		{
			DataProvider.Instance().UpdateBTBWeatherFeed(objBTBWeatherFeed.WeatherId, 
												objBTBWeatherFeed.ModuleId, 
												objBTBWeatherFeed.Ttl, 
												objBTBWeatherFeed.UpdatedDate, 
												objBTBWeatherFeed.CachedDate, 
												objBTBWeatherFeed.Feed, 
												objBTBWeatherFeed.Url, 
												objBTBWeatherFeed.TransformedFeed,
												objBTBWeatherFeed.LocationName);
		}
		
		public void Delete(int weatherId)
		{
			DataProvider.Instance().DeleteBTBWeatherFeed(weatherId);
		}

		#endregion

		#endregion

		#region "Optional Interfaces"		
	
		public string ExportModule(int ModuleID)
		{
			string xmlExport = "";
			ArrayList feeds = this.GetByModuleId(ModuleID);

			if(feeds != null & feeds.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("<BTBYahooWeather>");

				foreach(BTBWeatherFeedInfo info in feeds)
				{
					sb.Append("<url>");
					sb.Append(Globals.XMLEncode(info.Url));
					sb.Append("</url>");
				}

				sb.Append("</BTBYahooWeather>");

				xmlExport = sb.ToString();
			}
			return xmlExport;
		}

		public void ImportModule(int ModuleID, string Content, string Version, int UserId)
		{
			
			XmlNode weatherNode = Globals.GetContent(Content, "BTBYahooWeather");

			XmlNodeList nodeList = weatherNode.SelectNodes("url");

			if(nodeList != null)
			{
				for(int i = 0; i < nodeList.Count; i++)
				{
					BTBWeatherFeedInfo info = new BTBWeatherFeedInfo();
					info.ModuleId = ModuleID;
					info.Url = nodeList[i].InnerText;

					Add(info);
				}
			}
		}
		#endregion
	}
}
