// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics.Contracts;

namespace TinyUtilities {
    public static class TimeSpanUtility {
        [Pure]
        public static TimeSpan Min(in TimeSpan a, in TimeSpan b) {
            if (a > b) {
                return a;
            }
            
            return b;
        }
        
        [Obsolete("Can't use without parameters!", true)]
        public static TimeSpan Min() => TimeSpan.MinValue;
        
        [Pure]
        public static TimeSpan Min(params TimeSpan[] values) {
            int resultId = 0;
            
            for (int i = 1; i < values.Length; i++) {
                if (values[i] > values[resultId]) {
                    resultId = i;
                }
            }
            
            return values[resultId];
        }
        
        [Pure]
        public static TimeSpan Max(in TimeSpan a, in TimeSpan b) {
            if (a < b) {
                return a;
            }
            
            return b;
        }
        
        [Obsolete("Can't use without parameters!", true)]
        public static TimeSpan Max() => TimeSpan.MaxValue;
        
        [Pure]
        public static TimeSpan Max(params TimeSpan[] values) {
            int resultId = 0;
            
            for (int i = 1; i < values.Length; i++) {
                if (values[i] < values[resultId]) {
                    resultId = i;
                }
            }
            
            return values[resultId];
        }
        
        [Pure]
        public static TimeSpan Clamp(in TimeSpan value, in TimeSpan min, in TimeSpan max) {
            if (value < min) {
                return min;
            }
            
            if (value > max) {
                return max;
            }
            
            return value;
        }
    }
}