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
using DotNetNuke.UI;

using BiteTheBullet.DNN.Modules.BTBYahooWeather.Business;
using DotNetNuke.Entities.Users;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather
{
	public abstract class BTBYahooWeather : PortalModuleBase, IActionable, IPortable
	{
        private const string USER_PERSONIALISATION = "BTBYahooWeather_UserPersist";

		protected System.Web.UI.WebControls.Literal litOutput;
		protected System.Web.UI.WebControls.DropDownList ddlLocations;
		protected System.Web.UI.WebControls.Label lblLocation;
		protected System.Web.UI.WebControls.Panel pnlDropDown;
		protected System.Web.UI.WebControls.Label lblFeedOutput;

        private Hashtable settings = null;

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
			this.ddlLocations.SelectedIndexChanged += new System.EventHandler(this.ddlLocations_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region Event Handlers

		private void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				BTBWeatherFeedController controller = new BTBWeatherFeedController();
				BTBWeatherFeedInfo info;
				if (!Page.IsPostBack) 
				{
					ArrayList weatherFeeds = controller.GetByModuleId(ModuleId);

					if((weatherFeeds == null) | (weatherFeeds.Count == 0))
					{
						//this module does not have a configured
						//weather feed, info the user						
						DotNetNuke.UI.Skins.Skin.AddModuleMessage(this,
							Localization.GetString("FeedNotSet.Text",this.LocalResourceFile),
							DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
						return;
					}
					

					if(weatherFeeds.Count > 1)
					{
						ValidateFeedsHaveLocationName(weatherFeeds);

						pnlDropDown.Visible = true;
						ddlLocations.DataTextField = "LocationName";
						ddlLocations.DataValueField = "WeatherId";

						weatherFeeds.Sort(new BTBYahooWeatherInfoIComparer());

						ddlLocations.DataSource = weatherFeeds;
						ddlLocations.DataBind();
					}
					
                    //check if we should load the users selected weather feed
                    if (UserPersistanceEnabled && HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        //check if the user has selected otherwise just let the
                        //default location pick the selection

                        DotNetNuke.Entities.Users.UserInfo curUser = DotNetNuke.Entities.Users.UserController.GetUser(PortalId, UserId, false);
                        string location = curUser.Profile.GetPropertyValue(USER_PERSONIALISATION);
                        if (location != null)
                        {
                            BTBWeatherFeedInfo userSelectWFI = null;
                            int locationId = -1;

                            int.TryParse(location, out locationId);

                            foreach (BTBWeatherFeedInfo item in weatherFeeds)
                            {
                                if (item.WeatherId == locationId)
                                {
                                    userSelectWFI = item;
                                    break;
                                }
                            }

                            if (userSelectWFI != null)
                            {
                                ddlLocations.SelectedValue = userSelectWFI.WeatherId.ToString();
                                DisplayFeed(userSelectWFI);
                                return;
                            }
                        }

                    }


					//check if we have a default feed, overwise show the first time
					BTBWeatherFeedInfo defaultInfo = controller.GetDefaultFeed(weatherFeeds);
					if(defaultInfo != null)
					{
						info = defaultInfo;
						ddlLocations.SelectedValue = defaultInfo.WeatherId.ToString();
					}
					else
					{					
						info = (BTBWeatherFeedInfo)weatherFeeds[0];
					}
					
					DisplayFeed(info);	
				}
			}
			catch(System.Net.WebException exc)
			{
				//this will bubble up from the call to the update feed if we have a connection
				//problem to the remote RSS feed 

				//output something to the user control to tell the user we have a problem
				DotNetNuke.UI.Skins.Skin.AddModuleMessage(this,
					Localization.GetString("WebException.Text",this.LocalResourceFile),
					DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);

				//log the details in the event log for the admin to see
				Exceptions.LogException(exc);
			}
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void ddlLocations_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int weatherId = Int32.Parse(ddlLocations.SelectedValue);

			BTBWeatherFeedController controller = new BTBWeatherFeedController();
			BTBWeatherFeedInfo info = controller.Get(weatherId);

            //determine if user persistance is enabled
            //and store the selection
            if (UserPersistanceEnabled && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DotNetNuke.Entities.Users.UserInfo curUser = DotNetNuke.Entities.Users.UserController.GetUser(PortalId, UserId, false);

                curUser.Profile.SetProfileProperty(USER_PERSONIALISATION, ddlLocations.SelectedValue);
                UserController.UpdateUser(this.PortalId, curUser);
            }

			DisplayFeed(info);
		}

		#endregion

        /// <summary>
        /// get if we should store the users persisted weather location
        /// </summary>
        private bool UserPersistanceEnabled
        {
            get
            {
                if (settings == null)
                {
                    ModuleController mc = new ModuleController();
                    settings = mc.GetTabModuleSettings(this.TabModuleId);
                }

                object setting = settings[USER_PERSONIALISATION];

                bool result = false;

                if (setting != null)
                    bool.TryParse(setting.ToString(), out result);

                return result;
            }
        }

		private void ValidateFeedsHaveLocationName(ArrayList feeds)
		{
			//if we find a feeds without a name, this could becuase we
			//have just converted to 1.1 of the app or have imported
			//existing weather content, therefore the feed hasnt be parsed
			//to generate the location name
			BTBWeatherFeedController controller = new BTBWeatherFeedController();

			foreach(BTBWeatherFeedInfo feed in feeds)
			{
				if(feed.LocationName == Null.NullString)
				{
					controller.UpdateWeatherFeed(feed, PortalSettings, ModuleConfiguration);
				}
			}
		}
		
		protected void DisplayFeed(BTBWeatherFeedInfo info)
		{
			//check if we have a transformed feed or the item has expired from cache
			if(info.TransformedFeed == Null.NullString | info.CachedDate < DateTime.Now | info.LocationName == Null.NullString)
			{
				BTBWeatherFeedController controller = new BTBWeatherFeedController();
				controller.UpdateWeatherFeed(info, this.PortalSettings, this.ModuleConfiguration);
			}

			litOutput.Text = info.TransformedFeed;
		}

		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				Actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return Actions;
			}
		}

		public string ExportModule(int ModuleID)
		{
			// included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
			return null;
		}

		public void ImportModule(int ModuleID, string Content, string Version, int UserID)
		{
			// included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
		}

		#endregion

		

	}
}