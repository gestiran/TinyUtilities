// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics.Contracts;

namespace TinyUtilities {
    public static class EnumUtility {
        public static void RunAll<T>(Action<T> action) where T : struct {
            foreach (object value in Enum.GetValues(typeof(T))) {
                action((T)value);
            }
        }
        
        public static bool RunFirst<T>(Func<T, bool> func, bool targetResult = true) where T : struct {
            foreach (object value in Enum.GetValues(typeof(T))) {
                if (func((T)value) == targetResult) {
                    return true;
                }
            }
            
            return false;
        }
        
        [Pure]
        public static T[] ToArray<T>() where T : struct {
            Array values = Enum.GetValues(typeof(T));
            T[] result = new T[values.Length];
            int i = 0;
            
            foreach (object value in values) {
                result[i++] = (T)value;
            }
            
            return result;
        }
        
        [Pure]
        public static (T, int)[] ToArrayTuple<T>() where T : struct {
            Array values = Enum.GetValues(typeof(T));
            (T, int)[] result = new (T, int)[values.Length];
            int i = 0;
            
            foreach (object value in values) {
                result[i] = ((T)value, i);
                i++;
            }
            
            return result;
        }
        
        [Pure]
        public static int Count<T>() where T : struct => Enum.GetValues(typeof(T)).Length;
    }
}