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
using BiteTheBullet.DNN.Modules.BTBYahooWeather.Data;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Business
{
	public class BTBWeatherFeedInfo
	{
		#region "Private Members"
		int _weatherId;
		int _moduleId;
		int _ttl;
		DateTime _updatedDate;
		DateTime _cachedDate;
		string _feed;
		string _transformedFeed;
		string _url;
		string _locationName;
		bool _defaultFeed;
		#endregion
		
		#region "Constructors"
		public BTBWeatherFeedInfo()
		{
		}

		public BTBWeatherFeedInfo(int weatherId, int moduleId , int ttl , DateTime updatedDate , 
															DateTime cachedDate , string feed , string url, 
															string transformedFeed, string locationName,
															bool defaultFeed)
		{
			this.WeatherId = weatherId;
			this.ModuleId = moduleId;
			this.Ttl = ttl;
			this.UpdatedDate = updatedDate;
			this.CachedDate = cachedDate;
			this.Feed = feed;
			this.Url = url;
			this.TransformedFeed = transformedFeed;
			this.LocationName = locationName;
			this.DefaultFeed = defaultFeed;			
		}
		#endregion
		
		#region "Public Properties"
		public int WeatherId
		{
			get{return _weatherId;}
			set{_weatherId = value;}
		}
		
		public int ModuleId
		{
			get{return _moduleId;}
			set{_moduleId = value;}
		}

		public int Ttl
		{
			get{return _ttl;}
			set{_ttl = value;}
		}

		public DateTime UpdatedDate
		{
			get{return _updatedDate;}
			set{_updatedDate = value;}
		}

		public DateTime CachedDate
		{
			get{return _cachedDate;}
			set{_cachedDate = value;}
		}

		public string Feed
		{
			get{return _feed;}
			set{_feed = value;}
		}

		public string Url
		{
			get{return _url;}
			set{_url = value;}
		}

		public string TransformedFeed
		{
			get{return _transformedFeed;}
			set{_transformedFeed = value;}
		}

		public string LocationName
		{
			get{ return _locationName;}
			set{ _locationName = value;}
		}

		public string AdminDisplayName
		{
			get
			{
				if(_locationName != null && _locationName.Length > 0)
				{
					return _locationName;
				}
				else
					return "no name defined - " + _url;

			}
		}
		
		public bool DefaultFeed
		{
			get{ return _defaultFeed; }
			set{ _defaultFeed=value; }
		}

		#endregion
	}
}