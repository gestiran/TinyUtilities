// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Xml.Linq;

namespace TinyUtilities.Extensions.Global {
    public static class DateTimeExtension {
        private const string _ROOT_KEY = "DateTime";
        private const string _YEAR_KEY = "Y";
        private const string _MOTH_KEY = "M";
        private const string _DAY_KEY = "D";
        private const string _HOUR_KEY = "h";
        private const string _MINUTE_KEY = "m";
        private const string _SECOND_KEY = "sc";
        private const string _MILLISECOND_KEY = "ms";
        
        public static string ToXMLString(this DateTime dateTime) {
            XElement root = new XElement(_ROOT_KEY);
            
            root.Add(new XAttribute(_YEAR_KEY, dateTime.Year.ToString()));
            root.Add(new XAttribute(_MOTH_KEY, dateTime.Month.ToString()));
            root.Add(new XAttribute(_DAY_KEY, dateTime.Day.ToString()));
            root.Add(new XAttribute(_HOUR_KEY, dateTime.Hour.ToString()));
            root.Add(new XAttribute(_MINUTE_KEY, dateTime.Minute.ToString()));
            root.Add(new XAttribute(_SECOND_KEY, dateTime.Second.ToString()));
            root.Add(new XAttribute(_MILLISECOND_KEY, dateTime.Millisecond.ToString()));
            
            return root.ToString();
        }
        
        public static bool TryConvertToDateTime(this string value, out DateTime dateTime) => TryConvertToDateTime(value, out dateTime, DateTime.Now);
        
        public static bool IsNextDay(this DateTime now, DateTime last) {
            if (now.Year > last.Year) {
                return true;
            }
            
            if (now.Month > last.Month) {
                return true;
            }
            
            if (now.Day > last.Day) {
                return true;
            }
            
            return false;
        }
        
        public static bool TryConvertToDateTime(this string value, out DateTime dateTime, DateTime defaultValue) {
            if (string.IsNullOrEmpty(value)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            XElement root = XElement.Parse(value);
            
            if (!TryExtractValue(root, _YEAR_KEY, out int year)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            if (!TryExtractValue(root, _MOTH_KEY, out int month)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            if (!TryExtractValue(root, _DAY_KEY, out int day)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            if (!TryExtractValue(root, _HOUR_KEY, out int hour)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            if (!TryExtractValue(root, _MINUTE_KEY, out int minute)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            if (!TryExtractValue(root, _SECOND_KEY, out int second)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            if (!TryExtractValue(root, _MILLISECOND_KEY, out int millisecond)) {
                return ReturnDefaultTime(out dateTime, defaultValue);
            }
            
            
            dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
            
            return true;
        }
        
        private static bool TryExtractValue(XElement root, string key, out int resultValue) {
            XAttribute attribute = root.Attribute(key);
            
            if (attribute != null && int.TryParse(attribute.Value, out resultValue)) {
                return true;
            }
            
            resultValue = default;
            
            return false;
        }
        
        private static bool ReturnDefaultTime(out DateTime dateTime, DateTime defaultValue) {
            dateTime = defaultValue;
            
            return false;
        }
    }
}