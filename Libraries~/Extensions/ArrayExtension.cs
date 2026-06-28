// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TinyUtilities.Extensions {
    public static class ArrayExtension {
        [Obsolete("Do nothing", true)]
        public static T[] AddToArray<T>(this T[] array) => array;
        
        [Pure]
        public static T[] AddToArray<T>([In, Out] this T[] array, T obj) {
            if (array == null) {
                array = Array.Empty<T>();
            }
            
            T[] result = new T[array.Length + 1];
            
            Array.Copy(array, 0, result, 0, array.Length);
            result[array.Length] = obj;
            
            array = result;
            
            return array;
        }
        
        [Pure]
        public static T[] AddToArray<T>(this T[] array, params T[] objects) {
            if (array == null) {
                array = Array.Empty<T>();
            }
            
            if (objects == null) {
                objects = Array.Empty<T>();
            }
            
            T[] result = new T[array.Length + objects.Length];
            
            Array.Copy(array, 0, result, 0, array.Length);
            Array.Copy(objects, 0, result, array.Length, objects.Length);
            
            return result;
        }
        
        [Pure]
        public static T[] RemoveFromArray<T>(this T[] array, int id) {
            if (array == null) {
                array = Array.Empty<T>();
            }
            
            T[] result = new T[array.Length - 1];
            
            for (int i = 0, removeId = 0; i < array.Length; i++) {
                if (removeId == id) {
                    continue;
                }
                
                result[removeId] = array[i];
                removeId++;
            }
            
            return result;
        }
        
        [Pure]
        public static int[] GetNumbersArray(this int length) {
            int[] result = new int[length];
            
            for (int i = 0; i < length; i++) {
                result[i] = i;
            }
            
            return result;
        }
        
        [Pure]
        public static bool IsContainObjectOfType<T>(this T[] array, T element) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i].GetType() == element.GetType()) {
                    return true;
                }
            }
            
            return false;
        }
        
        [Pure]
        public static bool IsContain<T>(this T[] array, T element) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i].Equals(element)) {
                    return true;
                }
            }
            
            return false;
        }
        
        [Pure]
        public static int GetMiddle(this int[] array) {
            int result = array.Sum();
            result /= array.Length;
            return result;
        }
        
        [Pure]
        public static string ToStringArray<T>(this T[] array) {
            StringBuilder builder = new StringBuilder(array.Length);
            
            for (int i = 0; i < array.Length; i++) {
                builder.AppendFormat("{0} = {1}\n", i, array[i]);
            }
            
            return builder.ToString();
        }
        
        [Pure]
        public static T Any<T>(this T[] array, T defaultValue = default) {
            if (array.Length > 0) {
                return array[RandomUtility.Range(0, array.Length - 1)];
            }
            
            return defaultValue;
        }
        
        [Pure]
        public static T Any<T>(this T[] array, out int index, T defaultValue = default) {
            if (array.Length > 0) {
                index = RandomUtility.Range(0, array.Length - 1);
                return array[index];
            }
            
            index = 0;
            return defaultValue;
        }
        
        [Pure]
        public static T[] Any<T>(this T[] origin, int count) {
            if (origin.Length < count) {
                return origin;
            }
            
            T[] shuffled = new T[origin.Length];
            Array.Copy(origin, shuffled, origin.Length);
            shuffled.Shuffle();
            
            T[] result = new T[count];
            
            for (int i = 0; i < count; i++) {
                result[i] = shuffled[i];
            }
            
            return result;
        }
        
        [Pure]
        public static T[] Any<T>(this T[] array, int[] indexes) {
            if (indexes.Length == 0) {
                return Array.Empty<T>();
            }
            
            int count = indexes.Length;
            
            if (count > array.Length) {
                count = array.Length;
            }
            
            List<int> ids = new List<int>(array.Length);
            
            for (int i = 0; i < ids.Count; i++) {
                ids[i] = i;
            }
            
            T[] result = new T[count];
            
            for (int i = 0; i < count; i++) {
                int index = ids.Any();
                
                result[i] = array[index];
                indexes[i] = index;
                
                ids.Remove(index);
            }
            
            return result;
        }
        
        [Pure]
        public static T[] RemoveRange<T>(this T[] array, T[] objects) {
            List<T> result = new List<T>(array.Length);
            
            for (int arrayId = 0; arrayId < array.Length; arrayId++) {
                if (objects.IsContain(array[arrayId])) {
                    continue;
                }
                
                result.Add(array[arrayId]);
            }
            
            return result.ToArray();
        }
        
        public static bool TryIndexOf<T>(this T[] array, T obj, out int index) {
            for (index = 0; index < array.Length; index++) {
                if (array[index].Equals(obj)) {
                    return true;
                }
            }
            
            return false;
        }
        
        [Pure]
        public static int FindClosestIndex(this float[] array, float target) {
            if (array == null || array.Length == 0) {
                return 0;
            }
            
            int closestIndex = 0;
            float minDiff = Math.Abs(array[0] - target);
            
            for (int i = 1; i < array.Length; i++) {
                float diff = Math.Abs(array[i] - target);
                
                if (diff >= minDiff) {
                    continue;
                }
                
                minDiff = diff;
                closestIndex = i;
            }
            
            return closestIndex;
        }
        
        public static T[] Shuffle<T>(this T[] array) {
            for (int i = 0; i < array.Length; i++) {
                int index = RandomUtility.Range(0, array.Length);
                
                if (index == i) {
                    continue;
                }
                
                (array[index], array[i]) = (array[i], array[index]);
            }
            
            return array;
        }
        
        public static T[] Reverse<T>(this T[] array) {
            int length = array.Length / 2;
            
            for (int i = 0; i < length; i++) {
                (array[i], array[array.Length - 1 - i]) = (array[array.Length - 1 - i], array[i]);
            }
            
            return array;
        }
    }
}