// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Xml.Linq;

namespace TinyUtilities.Extensions.Global {
    public static class TimeSpanExtension {
        private const string _ROOT_KEY = "TimeSpan";
        private const string _DAY_KEY = "D";
        private const string _HOUR_KEY = "h";
        private const string _MINUTE_KEY = "m";
        private const string _SECOND_KEY = "sc";
        private const string _MILLISECOND_KEY = "ms";
        
        public static int TotalHours(this TimeSpan timeSpan) => timeSpan.Days * 24 + timeSpan.Hours;
        
        public static int TotalMinutes(this TimeSpan timeSpan) => timeSpan.Hours * 60 + timeSpan.Minutes;
        
        public static string ToXMLString(this TimeSpan timeSpan) {
            XElement root = new XElement(_ROOT_KEY);
            
            root.Add(new XAttribute(_DAY_KEY, timeSpan.Days.ToString()));
            root.Add(new XAttribute(_HOUR_KEY, timeSpan.Hours.ToString()));
            root.Add(new XAttribute(_MINUTE_KEY, timeSpan.Minutes.ToString()));
            root.Add(new XAttribute(_SECOND_KEY, timeSpan.Seconds.ToString()));
            root.Add(new XAttribute(_MILLISECOND_KEY, timeSpan.Milliseconds.ToString()));
            
            return root.ToString();
        }
        
        public static TimeSpan SubtractSecond(this TimeSpan timeSpan, int count) => timeSpan.Subtract(new TimeSpan(0, 0, count));
        
        public static bool TryConvertToTimeSpan(this string value, out TimeSpan timeSpan) => TryConvertToTimeSpan(value, out timeSpan, TimeSpan.Zero);
        
        public static bool TryConvertToTimeSpan(this string value, out TimeSpan timeSpan, TimeSpan defaultValue) {
            if (String.IsNullOrEmpty(value)) return ReturnDefaultTimeSpan(out timeSpan, defaultValue);
            
            XElement root = XElement.Parse(value);
            
            if (!TryExtractValue(root, _DAY_KEY, out int day)) return ReturnDefaultTimeSpan(out timeSpan, defaultValue);
            if (!TryExtractValue(root, _HOUR_KEY, out int hour)) return ReturnDefaultTimeSpan(out timeSpan, defaultValue);
            if (!TryExtractValue(root, _MINUTE_KEY, out int minute)) return ReturnDefaultTimeSpan(out timeSpan, defaultValue);
            if (!TryExtractValue(root, _SECOND_KEY, out int second)) return ReturnDefaultTimeSpan(out timeSpan, defaultValue);
            if (!TryExtractValue(root, _MILLISECOND_KEY, out int millisecond)) return ReturnDefaultTimeSpan(out timeSpan, defaultValue);
            
            timeSpan = new TimeSpan(day, hour, minute, second, millisecond);
            
            return true;
        }
        
        private static bool TryExtractValue(XElement root, string key, out int resultValue) {
            XAttribute attribute = root.Attribute(key);
            
            if (attribute != null && int.TryParse(attribute.Value, out resultValue)) return true;
            
            resultValue = default;
            
            return false;
        }
        
        private static bool ReturnDefaultTimeSpan(out TimeSpan timeSpan, TimeSpan defaultValue) {
            timeSpan = defaultValue;
            
            return false;
        }
    }
}