// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics.Contracts;

#if UNITY_ENGINE
using Random = UnityEngine.Random;
#endif

namespace TinyUtilities {
    public static class RandomUtility {
        [Pure]
        public static T Any<T>(params T[] values) => values[Random.Range(0, values.Length)];
        
        [Obsolete("Can't use without parameters!", true)]
        public static T Any<T>() => default;
        
        [Pure]
        public static float Offset(float value, float offset) => Random.Range(value - offset, value + offset);
        
        [Pure]
        public static float OffsetPercent(float value, float offset) => Random.Range(value * (1f - offset), value * (1f + offset));
        
        [Pure]
        public static int Offset(int value, int offset) => Random.Range(value - offset, value + offset);
    }
}