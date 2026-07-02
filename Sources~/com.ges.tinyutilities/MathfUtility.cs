// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if EXTERNAL_DEPENDENCIES
using System;

namespace TinyUtilities {
    public struct MathfUtility {
        public const float PI = 3.1415927f;
        public const float INFINITY = float.PositiveInfinity;
        public const float NEGATIVE_INFINITY = float.NegativeInfinity;
        public const float DEG2_RAD = 0.017453292f;
        public const float RAD2_DEG = 57.29578f;
        public const float EPSILON = 2.7182818284590452354f;
        
        public static float Sin(float f) => (float)Math.Sin(f);
        
        public static float Cos(float f) => (float)Math.Cos(f);
        
        public static float Tan(float f) => (float)Math.Tan(f);
        
        public static float Asin(float f) => (float)Math.Asin(f);
        
        public static float Acos(float f) => (float)Math.Acos(f);
        
        public static float Atan(float f) => (float)Math.Atan(f);
        
        public static float Atan2(float y, float x) => (float)Math.Atan2(y, x);
        
        public static float Sqrt(float f) => (float)Math.Sqrt(f);
        
        public static float Abs(float f) => Math.Abs(f);
        
        public static int Abs(int value) => Math.Abs(value);
        
        public static float Min(float a, float b) => a < b ? a : b;
        
        public static float Min(params float[] values) {
            int length = values.Length;
            
            if (length == 0) {
                return 0f;
            }
            
            float min = values[0];
            
            for (int i = 1; i < length; i++) {
                if (values[i] < min) {
                    min = values[i];
                }
            }
            
            return min;
        }
        
        public static int Min(int a, int b) => a < b ? a : b;
        
        public static int Min(params int[] values) {
            int length = values.Length;
            
            if (length == 0) {
                return 0;
            }
            
            int min = values[0];
            
            for (int i = 1; i < length; i++) {
                if (values[i] < min) {
                    min = values[i];
                }
            }
            
            return min;
        }
        
        public static float Max(float a, float b) => a > b ? a : b;
        
        public static float Max(params float[] values) {
            int length = values.Length;
            
            if (length == 0) {
                return 0f;
            }
            
            float max = values[0];
            
            for (int i = 1; i < length; i++) {
                if (values[i] > max) {
                    max = values[i];
                }
            }
            
            return max;
        }
        
        public static int Max(int a, int b) => a > b ? a : b;
        
        public static int Max(params int[] values) {
            int length = values.Length;
            
            if (length == 0) {
                return 0;
            }
            
            int max = values[0];
            
            for (int i = 1; i < length; i++) {
                if (values[i] > max) {
                    max = values[i];
                }
            }
            
            return max;
        }
        
        public static float Pow(float f, float p) => (float)Math.Pow(f, p);
        
        public static float Exp(float power) => (float)Math.Exp(power);
        
        public static float Log(float f, float p) => (float)Math.Log(f, p);
        
        public static float Log(float f) => (float)Math.Log(f);
        
        public static float Log10(float f) => (float)Math.Log10(f);
        
        public static float Ceil(float f) => (float)Math.Ceiling(f);
        
        public static float Floor(float f) => (float)Math.Floor(f);
        
        public static float Round(float f) => (float)Math.Round(f);
        
        public static int CeilToInt(float f) => (int)Math.Ceiling(f);
        
        public static int FloorToInt(float f) => (int)Math.Floor(f);
        
        public static float Sign(float f) => f >= 0f ? 1f : -1f;
        
        public static float Clamp(float value, float min, float max) {
            if (value < min) {
                value = min;
            } else if (value > max) {
                value = max;
            }
            
            return value;
        }
        
        public static int Clamp(int value, int min, int max) {
            if (value < min) {
                value = min;
            } else if (value > max) {
                value = max;
            }
            
            return value;
        }
        
        public static float Clamp01(float value) {
            if (value < 0f) {
                return 0f;
            }
            
            return value > 1f ? 1f : value;
        }
        
        public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t);
        
        public static float LerpUnclamped(float a, float b, float t) => a + (b - a) * t;
        
        public static float LerpAngle(float a, float b, float t) {
            float num = Repeat(b - a, 360f);
            
            if (num > 180f) {
                num -= 360f;
            }
            
            return a + num * Clamp01(t);
        }
        
        public static float MoveTowards(float current, float target, float maxDelta) {
            return Abs(target - current) <= maxDelta ? target : current + Sign(target - current) * maxDelta;
        }
        
        public static float MoveTowardsAngle(float current, float target, float maxDelta) {
            float num = DeltaAngle(current, target);
            
            if (-maxDelta < num && num < maxDelta) {
                return target;
            }
            
            target = current + num;
            return MoveTowards(current, target, maxDelta);
        }
        
        public static float SmoothStep(float from, float to, float t) {
            t = Clamp01(t);
            t = -2f * t * t * t + 3f * t * t;
            return to * t + from * (1f - t);
        }
        
        public static float Gamma(float value, float absmax, float gamma) {
            bool flag = value < 0f;
            float num1 = Abs(value);
            
            if (num1 > absmax) {
                return flag ? -num1 : num1;
            }
            
            float num2 = Pow(num1 / absmax, gamma) * absmax;
            return flag ? -num2 : num2;
        }
        
        public static bool Approximately(float a, float b) {
            return Abs(b - a) < Max(1E-06f * Max(Abs(a), Abs(b)), EPSILON * 8f);
        }
        
        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime) {
            smoothTime = Max(0.0001f, smoothTime);
            float num1 = 2f / smoothTime;
            float num2 = num1 * deltaTime;
            
            float num3 = 1f / (1f + num2 + 0.47999998927116394f * num2 * num2 + 0.23499999940395355f * num2 * num2 * num2);
            
            float num4 = current - target;
            float num5 = target;
            float max = maxSpeed * smoothTime;
            float num6 = Clamp(num4, -max, max);
            target = current - num6;
            float num7 = (currentVelocity + num1 * num6) * deltaTime;
            float num8 = currentVelocity;
            currentVelocity = (currentVelocity - num1 * num7) * num3;
            float num9 = target + (num6 + num7) * num3;
            
            if (num5 - current > 0f == num9 > num5) {
                num9 = num5;
                currentVelocity = deltaTime != 0f ? (num9 - num5) / deltaTime : num8;
            }
            
            return num9;
        }
        
        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime) {
            target = current + DeltaAngle(current, target);
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }
        
        public static float Repeat(float t, float length) {
            return Clamp(t - Floor(t / length) * length, 0f, length);
        }
        
        public static float PingPong(float t, float length) {
            t = Repeat(t, length * 2f);
            return length - Abs(t - length);
        }
        
        public static float InverseLerp(float a, float b, float value) {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return a != b ? Clamp01((value - a) / (b - a)) : 0f;
        }
        
        public static float DeltaAngle(float current, float target) {
            float num = Repeat(target - current, 360f);
            
            if (num > 180f) {
                num -= 360f;
            }
            
            return num;
        }
        
        public static int NextPowerOfTwo(int value) {
            --value;
            value |= value >> 16 /*0x10*/;
            value |= value >> 8;
            value |= value >> 4;
            value |= value >> 2;
            value |= value >> 1;
            return value + 1;
        }
        
        public static int ClosestPowerOfTwo(int value) {
            int num1 = NextPowerOfTwo(value);
            int num2 = num1 >> 1;
            return value - num2 < num1 - value ? num2 : num1;
        }
        
        public static bool IsPowerOfTwo(int value) => (value & value - 1) == 0;
        
        public static bool Approximately(float value, params float[] values) {
            for (int i = 0; i < values.Length; i++) {
                if (MathfUtility.Approximately(value, values[i])) {
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
        
        public static int CalculatePercentSafe(int current, int max) {
            if (max == 0) {
                return 0;
            }
            
            if (current >= max) {
                return 100;
            }
            
            int result = current * 100 / max;
            
            if (result == 100 && current < max) {
                result = 99;
            }
            
            return result;
        }
        
        public static int CalculateRemainPercent(int current, int max) => 100 - CalculatePercent(current, max);
        
        public static int ApplyPercent(int value, int percent) => value * percent / 100;
        
        public static int ApplyPercent(int value, int current, int max) => ApplyPercent(value, CalculatePercent(current, max));
        
        public static int RoundToInt(float value) {
            int roundedValue = (int)Round(value);
            
            if (roundedValue % 2 == 0 && Abs(value - roundedValue + 1) < Abs(value - roundedValue)) {
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
#endif