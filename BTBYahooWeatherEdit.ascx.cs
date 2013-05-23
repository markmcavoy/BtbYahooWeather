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
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Profile;
using BiteTheBullet.DNN.Modules.BTBYahooWeather.Components;
using System.Linq;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather
{
	public abstract class BTBYahooWeatherEdit : PortalModuleBase
	{
        //const used to access the settings key/value pairs
		private const string VIEWSTATE_TEMPERATURE_SCALE = "BTBYahooWeather_TempScale";
		private const string TABMODULE_DISPLAYMODE= "BTBYahooWeather_DisplayMode";
        private const string USER_PERSONIALISATION = "BTBYahooWeather_UserPersist";

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdDeleteImage.Click += new System.Web.UI.ImageClickEventHandler(this.cmdDeleteImage_Click);
			this.lbnAddFeed.Click += new System.EventHandler(this.lbnAddFeed_Click);
			this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region Controls
		protected System.Web.UI.WebControls.LinkButton cmdUpdate;
		protected System.Web.UI.WebControls.LinkButton cmdCancel;
		protected System.Web.UI.WebControls.LinkButton cmdDelete;
		#endregion

		protected System.Web.UI.WebControls.Label lblFeedUpdate;
		protected System.Web.UI.WebControls.Label lblFeedCacheExpired;
		protected System.Web.UI.WebControls.TextBox txtFeedCode;
		protected System.Web.UI.WebControls.RadioButton rbTempC;
		protected System.Web.UI.WebControls.RadioButton rbTempF;
		protected System.Web.UI.WebControls.ListBox lstLocations;
		protected System.Web.UI.WebControls.ImageButton cmdDeleteImage;
		protected System.Web.UI.WebControls.LinkButton lbnAddFeed;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvWeatherCode;
		protected System.Web.UI.WebControls.RadioButton rbNormalDisplay;
		protected System.Web.UI.WebControls.RadioButton rbSmallDisplay;
		protected System.Web.UI.WebControls.RadioButton rbMediumDisplay;
		protected System.Web.UI.WebControls.DropDownList ddlDefaultFeed;
        protected System.Web.UI.WebControls.CheckBox chkUserPersist;


		public int ItemId
		{
			get
			{
				object i = ViewState["itemId"];
				if(i != null)
				{
					return Int32.Parse(i.ToString());
				}else
					return -1;
			}
			set
			{
				ViewState["itemId"] = value;
			}
		}

		private void LoadExistingData()
		{
			var list = new DataProvider().GetBTBWeatherFeedByModule(this.ModuleId);
            var controller = new BTBWeatherFeedController();


			//sort the list by location name
			lstLocations.DataSource = list.OrderBy(w => w.LocationName);
			lstLocations.DataBind();


			//if we have one or more feed get the current temperature unit
			//we need this to set the correct radio button checked
			if(list.Count() > 0)
			{
				BTBWeatherFeedInfo info = (BTBWeatherFeedInfo)list.First();
                BTBWeatherFeedController.TemperatureScale tempScale = controller.ExtractTemperatureScaleFromUrl(info.Url);
				
				if(tempScale == BTBWeatherFeedController.TemperatureScale.Celsius)
					rbTempC.Checked = true;
				else
					rbTempF.Checked = true;

				ViewState[VIEWSTATE_TEMPERATURE_SCALE] = tempScale;
				
				//setup the default weather feed
				ddlDefaultFeed.DataValueField = "WeatherId";
				ddlDefaultFeed.DataTextField = "LocationName";
				ddlDefaultFeed.DataSource = list;
				ddlDefaultFeed.DataBind();
				
				//set the default feed
                BTBWeatherFeedInfo defaultWeatherFeed = controller.GetDefaultFeed(list);
				if(defaultWeatherFeed != null)
				{
					ddlDefaultFeed.Enabled = true;
					ddlDefaultFeed.SelectedValue = defaultWeatherFeed.WeatherId.ToString();
				}				
			}
			else
			{
					ddlDefaultFeed.Enabled = false;
			}

			//load the current display defined for this module
			ModuleController mc = new ModuleController();
			Hashtable settings = mc.GetTabModuleSettings(this.TabModuleId);
						
			object displayMode = settings[TABMODULE_DISPLAYMODE] ?? "full";
			if(displayMode != null)
			{
				/*
				 * Version 1.4 change 8/5/2008
				 * 
				 * The five day forecast is no longer available, due the not having the
				 * data in the RSS feed. Any option which used this should
				 * just use the correct 2 day forecast instead
				 * */
				switch(displayMode.ToString())
				{
					case "full":
					case "full5":
						rbNormalDisplay.Checked = true;
						break;

					case "medium":
					case "medium5":
						rbMediumDisplay.Checked = true;
						break;

					case "summary":
						rbSmallDisplay.Checked = true;
						break;
				}				
			}

            //load the flag to determine if we allow the user to persist the selected
            //weather location
            object userPersist = settings[USER_PERSONIALISATION];
            if (userPersist != null)
            {
                bool userStoreSelection = false;
                bool.TryParse(userPersist.ToString(), out userStoreSelection);

                chkUserPersist.Checked = userStoreSelection;
            }

		}


		private void AddFeed()
		{
			BTBWeatherFeedInfo objBTBYahooWeather = new BTBWeatherFeedInfo();
			objBTBYahooWeather = ((BTBWeatherFeedInfo)CBO.InitializeObject(objBTBYahooWeather, typeof(BTBWeatherFeedInfo)));
					
			BTBWeatherFeedController objCtlBTBYahooWeather = new BTBWeatherFeedController();

			//load info object with relavent data
			objBTBYahooWeather.WeatherId = ItemId;
			objBTBYahooWeather.ModuleId = ModuleId;
					
			BTBWeatherFeedController.TemperatureScale tempScale;

			if(rbTempC.Checked)
				tempScale = BTBWeatherFeedController.TemperatureScale.Celsius;
			else
				tempScale = BTBWeatherFeedController.TemperatureScale.Fahrenheit;

			objBTBYahooWeather.Url = objCtlBTBYahooWeather.CreateFeedUrl(txtFeedCode.Text, tempScale);

			objBTBYahooWeather.WeatherId = new DataProvider().AddBTBWeatherFeed(objBTBYahooWeather);
			objCtlBTBYahooWeather.UpdateWeatherFeed(objBTBYahooWeather, this.PortalSettings, this.ModuleConfiguration);

		}

		private void ChangeDefaultItem()
		{
			int weatherId;
			
			string ddValue = ddlDefaultFeed.SelectedValue;
			if(ddValue != null)
			{
				weatherId = int.Parse(ddValue);
				
				BTBWeatherFeedController objCtlBTBYahooWeather = new BTBWeatherFeedController();
				objCtlBTBYahooWeather.SetDefaultFeed(ModuleId, weatherId);
			}
		}

		private void ChangeTemperatureScale()
		{
			//remove the cached transformation and set the url to
			//the correct one for the new unit of temperature

			BTBWeatherFeedController.TemperatureScale tempScale;
            var controller = new BTBWeatherFeedController();

			if(rbTempC.Checked)
				tempScale = BTBWeatherFeedController.TemperatureScale.Celsius;
			else
				tempScale = BTBWeatherFeedController.TemperatureScale.Fahrenheit;


			var feeds = new DataProvider().GetBTBWeatherFeedByModule(ModuleId);

			foreach(BTBWeatherFeedInfo info in feeds)
			{
				info.TransformedFeed = null;
				info.Url = controller.CreateFeedUrl(controller.ExtractWeatherCodeFromUrl(info.Url),
					tempScale);

				new DataProvider().UpdateBTBWeatherFeed(info);
			}
		}

		private void ChangeDisplayMode()
		{
			//remove the cached transformation and set the url to
			//the correct one for the new unit of temperature
			BTBWeatherFeedController controller = new BTBWeatherFeedController();

			ModuleController mc = new ModuleController();			

			if(rbNormalDisplay.Checked)
				mc.UpdateTabModuleSetting(TabModuleId, TABMODULE_DISPLAYMODE, "full");
//			else if(rbNormalDisplayFiveDays.Checked)
//				mc.UpdateTabModuleSetting(TabModuleId, TABMODULE_DISPLAYMODE, "full5");
			else if(rbSmallDisplay.Checked)
				mc.UpdateTabModuleSetting(TabModuleId, TABMODULE_DISPLAYMODE, "summary");
			else if(rbMediumDisplay.Checked)
				mc.UpdateTabModuleSetting(TabModuleId, TABMODULE_DISPLAYMODE, "medium");
//			else if(rbMediumDisplayFiveDays.Checked)
//				mc.UpdateTabModuleSetting(TabModuleId, TABMODULE_DISPLAYMODE, "medium5");

			var feeds = new DataProvider().GetBTBWeatherFeedByModule(ModuleId);

			foreach(BTBWeatherFeedInfo info in feeds)
			{
				info.TransformedFeed = null;
				new DataProvider().UpdateBTBWeatherFeed(info);
			}
		}

		#region Event Handlers
		private void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				if(!Page.IsPostBack)
					LoadExistingData();
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{

				//check if we have changed the selection
				object temperature = ViewState[VIEWSTATE_TEMPERATURE_SCALE];
				if(temperature != null)
				{
					BTBWeatherFeedController.TemperatureScale currentSelection;
					BTBWeatherFeedController.TemperatureScale previousSelection;

					previousSelection = (BTBWeatherFeedController.TemperatureScale)Enum.Parse(typeof(BTBWeatherFeedController.TemperatureScale),
												   temperature.ToString());

					if(rbTempC.Checked)
						currentSelection = BTBWeatherFeedController.TemperatureScale.Celsius;
					else
						currentSelection = BTBWeatherFeedController.TemperatureScale.Fahrenheit;

					if(previousSelection != currentSelection)
						ChangeTemperatureScale();
				}

				//check if we changed the display mode
				ModuleController mc = new ModuleController();
				Hashtable settings = mc.GetTabModuleSettings(this.TabModuleId);
						
				object displayMode = settings[TABMODULE_DISPLAYMODE];
				
				string previousDisplayMode;

				if(displayMode == null)
					previousDisplayMode = "full";
				else
					previousDisplayMode = displayMode.ToString();

				string currentDisplayMode = null;
				
				if(rbNormalDisplay.Checked)
					currentDisplayMode = "full";
				else if(rbSmallDisplay.Checked)
					currentDisplayMode = "summary";
				else if(rbMediumDisplay.Checked)
					currentDisplayMode = "medium";

				if(previousDisplayMode != currentDisplayMode)
				{
					//update the database and clear the cache transformation
					ChangeDisplayMode();
				}
				
				//set the default weather field
				ChangeDefaultItem();

                mc.UpdateTabModuleSetting(TabModuleId, USER_PERSONIALISATION, chkUserPersist.Checked.ToString());
                if (chkUserPersist.Checked)
                    CreatePropertyDefinitions();
				
				Response.Redirect(Globals.NavigateURL(), true);				
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

        /// <summary>
        /// check to make sure the currrent portal defines the property definition we
        /// are going to store against the userinfo object
        /// </summary>
        private void CreatePropertyDefinitions()
        {
            ProfileController.ClearProfileDefinitionCache(-1);
            ProfileController.ClearProfileDefinitionCache(PortalId);

            ProfilePropertyDefinition definition = new ProfilePropertyDefinition();
            definition.PortalId = -1;
            definition.PropertyCategory = "BtbYWeather";
            definition.PropertyName = USER_PERSONIALISATION;
            definition.Length = 10;
            definition.DataType = 349;

            if (ProfileController.GetPropertyDefinitionByName(-1, USER_PERSONIALISATION) == null)
            {
                //create for the host
                ProfileController.AddPropertyDefinition(definition);
            }

            if (ProfileController.GetPropertyDefinitionByName(this.PortalId, USER_PERSONIALISATION) == null)
            {
                //create for the users of the portal
                definition.PortalId = this.PortalId;
                ProfileController.AddPropertyDefinition(definition);
            }

            ProfileController.ClearProfileDefinitionCache(PortalId);
            
        }

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			try 
			{
				Response.Redirect(Globals.NavigateURL(), true);
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}



		/// <summary>
		/// handles removing an existing feed from the collection of weather feeds
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdDeleteImage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int weatherId = Int32.Parse(lstLocations.SelectedValue);
			new DataProvider().DeleteBTBWeatherFeed(weatherId);

			LoadExistingData();
		}


		/// <summary>
		/// handles the user adding a new weather feed to collection of
		/// existing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lbnAddFeed_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				AddFeed();
				LoadExistingData();

				//clear the feed since we have added it
				txtFeedCode.Text = string.Empty;
			}
		}

		#endregion
	}
}