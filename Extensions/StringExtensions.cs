using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string s)
        {
            return (T)Enum.Parse(typeof(T), s);
        }
    }
}
