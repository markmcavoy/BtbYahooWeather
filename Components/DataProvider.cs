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
using System.Collections;
using DotNetNuke.Data;
using System.Collections.Generic;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Components
{
	public class DataProvider
	{
        public static BTBWeatherFeedInfo GetBTBWeatherFeed(int weatherId)
        {
            using (var dataContext = DataContext.Instance())
            {
                var rep = dataContext.GetRepository<BTBWeatherFeedInfo>();
                return rep.GetById<int>(weatherId);
            }
        }

        public static IEnumerable<BTBWeatherFeedInfo> GetBTBWeatherFeedByModule(int moduleId)
        {
            using (var dataContext = DataContext.Instance())
            {
                var rep = dataContext.GetRepository<BTBWeatherFeedInfo>();
                return rep.Get<int>(moduleId);
            }
        }

        public static int AddBTBWeatherFeed(BTBWeatherFeedInfo newFeed)
        {
            using (var dataContext = DataContext.Instance())
            {
                var rep = dataContext.GetRepository<BTBWeatherFeedInfo>();
                rep.Insert(newFeed);
                return newFeed.WeatherId;
            }
        }

        public static void UpdateBTBWeatherFeed(BTBWeatherFeedInfo feed)
        {
            using (var dataContext = DataContext.Instance())
            {
                var rep = dataContext.GetRepository<BTBWeatherFeedInfo>();
                rep.Update(feed);
            }
        }

        public static void DeleteBTBWeatherFeed(int weatherId)
        {
            using (var dataContext = DataContext.Instance())
            {
                var rep = dataContext.GetRepository<BTBWeatherFeedInfo>();
                rep.Delete("WHERE weatherId=@0", weatherId);
            }
        }

        public static void SetDefaultFeed(int moduleId, int weatherId)
        {
            using (var dataContext = DataContext.Instance())
            {
                var rep = dataContext.GetRepository<BTBWeatherFeedInfo>();
                rep.Update("SET defaultFeed=0 WHERE moduleId=@0", moduleId);
                rep.Update("SET defaultFeed=1 WHERE weatherId=@0", weatherId);
            }
        }
	}
}
