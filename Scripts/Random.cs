// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if !UNITY_ENGINE
using System.Diagnostics.Contracts;
using SystemRandom = System.Random;

namespace TinyUtilities {
    internal static class Random {
        private static readonly SystemRandom _random;
        
        static Random() => _random = new SystemRandom();
        
        [Pure]
        public static int Range(int from, int to) => _random.Next(from, to);
        
        [Pure]
        public static float Range(float from, float to) {
            if (from > to) {
                return from + (from - to * _random.Next(0, 1000));
            }
            
            return to + (to - from * _random.Next(0, 1000));
        }
    }
}
#endif