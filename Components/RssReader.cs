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
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.XPath;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Components
{
	/// <summary>
	/// Readers a RSS 2.0 feed, this is a lightweight reader since we only
	/// want to parse the TTL element.
	/// </summary>
	public class RssReader
	{
		private string _response;
		private int _timeout = 3000;
		private bool _connected = false;
		private HttpWebRequest _webRequest;

		/// <summary>
		/// Load a RSS feed from a url, with a direct connection
		/// to the internet
		/// </summary>
		/// <param name="url"></param>
		public RssReader(string url):this(url, "", "", "")
		{

		}

		/// <summary>
		/// Loads a RSS feed from a url, with the defined proxy server used
		/// to connect to the internet
		/// </summary>
		/// <param name="url"></param>
		/// <param name="proxyHost"></param>
		/// <param name="proxyUsername"></param>
		/// <param name="proxyPassword"></param>
		public RssReader(string url, string proxyHost, string proxyUsername, string proxyPassword)
		{
			_webRequest = (HttpWebRequest)WebRequest.Create(url);

			//if we have proxy setup the webrequest
			if(proxyHost != null && proxyHost != string.Empty)
			{
        try
        {
          _webRequest.Proxy = new WebProxy(proxyHost);
				}
				catch(Exception)
				{
				}
      }
			//check if we need creditials for the proxy
			if(proxyUsername != null && proxyUsername != string.Empty)
			{
				_webRequest.Proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
			}
		}

		/// <summary>
		/// Property to set the timeout in milliseconds
		/// we will wait before throwing a webexception
		/// </summary>
		public int TimeOut
		{
			get{ return _timeout;}
			set{ _timeout = value;}
		}

		/// <summary>
		/// Retrurns the repsonse from the Url
		/// </summary>
		/// <returns></returns>
		public string Response()
		{
			if(!_connected)
				this.Connect();

			return _response;
		}

		/// <summary>
		/// Returns the Time To Live value for the feed, if a feed doesn't have
		/// this value -1 will be returned
		/// </summary>
		/// <returns></returns>
		public int Ttl()
		{
			string ttl = this.GetElement("/rss/channel/ttl");
			if(ttl != null)
				return Int32.Parse(ttl);
			else
				return -1;			
		}


		/// <summary>
		/// Returns the geographic location for the weather feed as string.
		/// i.e London, UK 
		/// </summary>
		/// <returns></returns>
		public string LocationName()
		{
			string location = this.GetElement("/rss/channel/title");

			//filter off the yahoo 
			return location.Substring(location.IndexOf("-") + 2);
		}

		public string GetElement(string XpathQuery)
		{
			ArrayList list = this.GetElements(XpathQuery);
			if(list.Count > 0)
			{
				//just peel off the first item
				return list[0].ToString();
			}else
				return null;
		}

		public ArrayList GetElements(string XpathQuery)
		{
			if(!_connected)
				this.Connect();

			
			XmlTextReader reader = new XmlTextReader(_response, XmlNodeType.Document, null);
			XPathDocument doc = new XPathDocument(reader);
			XPathNavigator query = doc.CreateNavigator();
			XPathNodeIterator iterator = query.Select(XpathQuery);

			ArrayList elements = new ArrayList();

			while(iterator.MoveNext())
			{
				elements.Add(iterator.Current.Value);
			}

			reader.Close();
			return elements;
			
		}

		/// <summary>
		/// Connects to the remote Url and stores the response
		/// </summary>
		private void Connect()
		{
			_webRequest.Timeout = _timeout;
			WebResponse response = _webRequest.GetResponse();
			
			Stream input = response.GetResponseStream();
			StreamReader reader = new StreamReader(input);
			_response = reader.ReadToEnd();
			_connected = true;
		}
	}
}
