// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;
using UnityEngine;

namespace TinyUtilities.NetworkTime {
    internal sealed class TimeServicePrefs {
        private const string _HOURS_OFFSET = "HoursOffset";
        
        [Pure]
        public bool HasHoursOffset() => PlayerPrefs.HasKey(_HOURS_OFFSET);
        
        [Pure]
        public int LoadHoursOffset() => PlayerPrefs.GetInt(_HOURS_OFFSET);
        
        public void SaveHoursOffset(in int hours) => PlayerPrefs.SetInt(_HOURS_OFFSET, hours);
    }
}