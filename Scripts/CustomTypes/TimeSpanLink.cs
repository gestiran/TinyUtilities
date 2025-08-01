﻿// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities.CustomTypes {
#if ODIN_INSPECTOR && UNITY_EDITOR
    [InlineProperty]
#endif
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public sealed class TimeSpanLink : IEquatable<TimeSpanLink>, IEquatable<TimeSpan> {
        [JsonIgnore]
        public TimeSpan time => new TimeSpan(_days, _hours, _minutes, _seconds);
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("Day", true), MinValue(0)]
    #endif
        [SerializeField, JsonProperty("day")]
        private int _days;
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("Hour", true), MinValue(0)]
    #endif
        [SerializeField, JsonProperty("hour")]
        private int _hours;
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("Min", true), MinValue(0)]
    #endif
        [SerializeField, JsonProperty("min")]
        private int _minutes;
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("Sec", true), MinValue(0)]
    #endif
        [SerializeField, JsonProperty("sec")]
        private int _seconds;
        
        public TimeSpanLink() { }
        
        public TimeSpanLink(TimeSpan timeSpan) {
            _days = timeSpan.Days;
            _hours = timeSpan.Hours;
            _minutes = timeSpan.Minutes;
            _seconds = timeSpan.Seconds;
        }
        
        public TimeSpanLink(int hours, int minutes, int seconds) {
            _hours = hours;
            _minutes = minutes;
            _seconds = seconds;
        }
        
        public TimeSpanLink(int days, int hours, int minutes, int seconds) {
            _days = days;
            _hours = hours;
            _minutes = minutes;
            _seconds = seconds;
        }
        
        public static implicit operator TimeSpan(TimeSpanLink value) => value.time;
        
        public override string ToString() => $"TimeSpan(days: {_days}, hours: {_hours}, minutes: {_minutes}, seconds: {_seconds})";
        
        public bool Equals(TimeSpanLink other) {
            return other != null && other._days == _days && other._hours == _hours && other._minutes == _minutes && other._seconds == _seconds;
        }
        
        public bool Equals(TimeSpan other) {
            return other.Days == _days && other.Hours == _hours && other.Minutes == _minutes && other.Seconds == _seconds;
        }
        
        public override bool Equals(object obj) {
        #if UNITY_EDITOR
            if (obj == null) {
                return false;
            }
        #endif
            return obj is TimeSpanLink other && other.Equals(this);
        }
        
        public override int GetHashCode() => time.GetHashCode();
    }
}