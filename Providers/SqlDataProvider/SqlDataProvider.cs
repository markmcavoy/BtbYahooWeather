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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Framework.Providers;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Data 
{
    
	public class SqlDataProvider : DataProvider 
	{
        
		private const string providerType = "data";
        
		#region Private Members
		private ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
		private string _connectionString;
		private string _providerPath;
		private string _objectQualifier;
		private string _databaseOwner;
		#endregion
        
		#region Constructors
		public SqlDataProvider()
		{
			Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
			if (objProvider.Attributes["connectionStringName"] != "" && System.Configuration.ConfigurationSettings.AppSettings[objProvider.Attributes["connectionStringName"]] != "") 
			{
				_connectionString = System.Configuration.ConfigurationSettings.AppSettings[objProvider.Attributes["connectionStringName"]];
			} 
			else 
			{
				_connectionString = objProvider.Attributes["connectionString"];
			}
			_providerPath = objProvider.Attributes["providerPath"];
			_objectQualifier = objProvider.Attributes["objectQualifier"];
			if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false) 
			{
				_objectQualifier += "_";
			}
			_databaseOwner = objProvider.Attributes["databaseOwner"];
			if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false) 
			{
				_databaseOwner += ".";
			}
		}
		#endregion

		#region Properties
		public string ConnectionString 
		{
			get 
			{
				return _connectionString;
			}
		}

		public string ProviderPath 
		{
			get 
			{
				return _providerPath;
			}
		}

		public string ObjectQualifier 
		{
			get 
			{
				return _objectQualifier;
			}
		}

		public string DatabaseOwner 
		{
			get 
			{
				return _databaseOwner;
			}
		}
		#endregion

		#region Private Methods
		private object GetNull(object Field)
		{
			return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value);
		}

		#endregion

		#region "BTBWeatherFeed Methods"
		public override IDataReader GetBTBWeatherFeed(int weatherId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "BTBWeatherFeedGet", weatherId);
		}

		public override IDataReader GetBTBWeatherFeedByModule(int moduleId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "BTBWeatherFeedGetByModule", moduleId);
		}

		public override int AddBTBWeatherFeed(int moduleId, int ttl, DateTime updatedDate, DateTime cachedDate, 
												string feed, string url, string transformedFeed, string locationName)
		{
			object ud = updatedDate;
			object cd = cachedDate;

			if(updatedDate.Year == 1)
				ud = null;

			if(cachedDate.Year == 1)
				cd = null;

			return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "BTBWeatherFeedAdd", moduleId, ttl, ud, cd, feed, url, transformedFeed, locationName).ToString());
		}
	
		public override void UpdateBTBWeatherFeed(int weatherId, int moduleId, int ttl, DateTime updatedDate, DateTime cachedDate, 
													string feed, string url, string transformedFeed, string locationName)
		{
			object ud = updatedDate;
			object cd = cachedDate;

			if(updatedDate.Year == 1)
				ud = null;

			if(cachedDate.Year == 1)
				cd = null;

			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "BTBWeatherFeedUpdate", weatherId, moduleId, ttl, ud, cd, feed, url, transformedFeed, locationName);
		}

		public override void DeleteBTBWeatherFeed(int weatherId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "BTBWeatherFeedDelete", weatherId);
		}
		
		public override void SetDefaultFeed(int moduleId, int weatherId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "BTBWeatherFeedSetDefault", moduleId, weatherId);
		}
		#endregion


	}
}

