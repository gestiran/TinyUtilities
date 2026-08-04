// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if EXTERNAL_DEPENDENCIES
using System.Diagnostics.Contracts;
using System;

namespace TinyUtilities {
    public static class RandomUtility {
        private static readonly Random _random;
        
        static RandomUtility() => _random = new Random();
        
        [Pure]
        public static int Range(int from, int to) => _random.Next(from, to);
        
        [Pure]
        public static float Range(float from, float to) {
            if (from > to) {
                return from + (from - to * _random.Next(0, 1000));
            }
            
            return to + (to - from * _random.Next(0, 1000));
        }
        
        [Pure]
        public static T Any<T>(params T[] values) => values[_random.Next(0, values.Length)];
        
        [Obsolete("Can't use without parameters!", true)]
        public static T Any<T>() => default;
        
        [Pure]
        public static float Offset(float value, float offset) => Range(value - offset, value + offset);
        
        [Pure]
        public static float OffsetPercent(float value, float offset) => Range(value * (1f - offset), value * (1f + offset));
        
        [Pure]
        public static int Offset(int value, int offset) => _random.Next(value - offset, value + offset);
    }
}
#endif