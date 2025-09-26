// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics.Contracts;
using UnityRandom = UnityEngine.Random;

namespace TinyUtilities {
    public static class RandomUtility {
        [Pure]
        public static T Any<T>(params T[] values) => values[UnityRandom.Range(0, values.Length)];
        
        [Obsolete("Can't use without parameters!", true)]
        public static T Any<T>() => default;
        
        [Pure]
        public static float Offset(float value, float offset) => UnityRandom.Range(value - offset, value + offset);
        
        [Pure]
        public static float OffsetPercent(float value, float offset) => UnityRandom.Range(value * (1f - offset), value * (1f + offset));
        
        [Pure]
        public static int Offset(int value, int offset) => UnityRandom.Range(value - offset, value + offset);
    }
}