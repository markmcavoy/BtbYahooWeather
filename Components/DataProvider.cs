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
using System.Web.Caching;
using System.Reflection;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Data
{
	public abstract class DataProvider
	{
		#region Shared/Static Methods
        // singleton reference to the instantiated object 
		private static DataProvider objProvider = null;

		// constructor
		static DataProvider()
		{
			CreateProvider();
		}

		// dynamically create provider
		private static void CreateProvider()
		{
			objProvider = ((DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "BiteTheBullet.DNN.Modules.BTBYahooWeather.Data", "BiteTheBullet.DNN.Modules.BTBYahooWeather"));
		}

		// return the provider
		public static DataProvider Instance()
		{
			return objProvider;
		}

		#endregion

		#region "BTBWeatherFeed Abstract Methods"
		public abstract IDataReader GetBTBWeatherFeed(int weatherId);
		public abstract IDataReader GetBTBWeatherFeedByModule(int moduleId);
		public abstract int AddBTBWeatherFeed(int moduleId , int ttl , DateTime updatedDate , 
												DateTime cachedDate , string feed , string url, 
												string transformedFeed, string locationName);
		public abstract void UpdateBTBWeatherFeed(int weatherId, int moduleId , int ttl ,
													DateTime updatedDate , DateTime cachedDate , string feed , 
													string url, string transformedFeed, string locationName);
		public abstract void DeleteBTBWeatherFeed(int weatherId);
		public abstract void SetDefaultFeed(int moduleId, int weatherId);
		#endregion
	}
}
