// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities {
    public static class MathfUtility {
        public static bool Approximately(float value, params float[] values) {
            for (int i = 0; i < values.Length; i++) {
                if (Mathf.Approximately(value, values[i])) {
                    continue;
                }
                
                return false;
            }
            
            return true;
        }
        
        public static int CalculatePercent(int current, int max) {
            if (max == 0) {
                return 0;
            }
            
            if (current == max) {
                return 100;
            }
            
            return current * 100 / max;
        }
        
        public static int CalculateRemainPercent(int current, int max) => 100 - CalculatePercent(current, max);
        
        public static int ApplyPercent(int value, int percent) => value * percent / 100;
        
        public static int ApplyPercent(int value, int current, int max) => ApplyPercent(value, CalculatePercent(current, max));
        
        public static int RoundToInt(float value) {
            int roundedValue = (int)Mathf.Round(value);
            
            if (roundedValue % 2 == 0 && Mathf.Abs(value - roundedValue + 1) < Mathf.Abs(value - roundedValue)) {
                roundedValue++;
            }
            
            return roundedValue;
        }
        
        public static bool CompareInt(float first, float second, float multiplier = 10f) {
            int firstInt = (int)(first * first * multiplier);
            int secondInt = (int)(second * second * multiplier);
            
            return firstInt == secondInt;
        }
    }
}