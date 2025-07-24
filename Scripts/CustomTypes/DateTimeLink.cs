// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities.CustomTypes {
    [Serializable, InlineProperty]
    public sealed class DateTimeLink : IEquatable<DateTimeLink>, IEquatable<DateTime> {
        public DateTime time {
            get {
            #if UNITY_EDITOR
                return new DateTime(Mathf.Clamp(_year, 1, 3000), Mathf.Clamp(_month, 1, 12), Mathf.Clamp(_day, 1, 31), Mathf.Clamp(_hour, 0, 23), Mathf.Clamp(_minute, 0, 59),
                                    Mathf.Clamp(_second, 0, 59));
            #endif
                
                return new DateTime(_year, _month, _day, _hour, _minute, _second);
            }
        }
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Year", true), OnValueChanged(nameof(ValidateDate))]
        private int _year = 1;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Month", true), OnValueChanged(nameof(ValidateDate))]
        private int _month = 1;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Day", true), OnValueChanged(nameof(ValidateDate))]
        private int _day = 1;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Hour", true), OnValueChanged(nameof(ValidateDate))]
        private int _hour;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Minute", true), OnValueChanged(nameof(ValidateDate))]
        private int _minute;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Second", true), OnValueChanged(nameof(ValidateDate))]
        private int _second;
        
        public static implicit operator DateTime(DateTimeLink value) => value.time;
        
        public override string ToString() => $"DateTime(year: {_year}, month: {_month}, day: {_day}, hour: {_hour}, minute: {_minute}, second: {_second})";
        
        public bool Equals(DateTimeLink other) {
            return other != null && other._year == _year && other._month == _month && other._day == _day && other._hour == _hour && other._minute == _minute
                && other._second == _second;
        }
        
        public bool Equals(DateTime other) {
            return other.Year == _year && other.Month == _month && other.Day == _day && other.Hour == _hour && other.Minute == _minute && other.Second == _second;
        }
        
        public override bool Equals(object obj) {
        #if UNITY_EDITOR
            if (obj == null) {
                return false;
            }
        #endif
            return obj is DateTimeLink other && other.Equals(this);
        }
        
        public override int GetHashCode() => time.GetHashCode();
        
        private void ValidateDate() {
            if (_year < 1) {
                _year = 1;
            }
            
            if (_month < 1) {
                _month = 1;
            } else if (_month > 12) {
                _month = 12;
            }
            
            if (_day < 1) {
                _day = 1;
            } else if (_day > 31) {
                _day = 31;
            }
            
            if (_hour < 0) {
                _hour = 0;
            } else if (_hour > 23) {
                _hour = 23;
            }
            
            if (_minute < 0) {
                _minute = 0;
            } else if (_minute > 59) {
                _minute = 59;
            }
            
            if (_second < 0) {
                _second = 0;
            } else if (_second > 59) {
                _second = 59;
            }
        }
    }
}