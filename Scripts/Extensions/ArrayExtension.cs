// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Random = UnityEngine.Random;

namespace TinyUtilities.Extensions {
    public static class ArrayExtension {
        [Pure]
        public static T Any<T>(this T[] array, T defaultValue = default) {
            if (array.Length > 0) {
                return array[Random.Range(0, array.Length - 1)];
            }
            
            return defaultValue;
        }
        
        [Pure]
        public static T Any<T>(this T[] array, out int index, T defaultValue = default) {
            if (array.Length > 0) {
                index = Random.Range(0, array.Length - 1);
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
        
        public static T[] Shuffle<T>(this T[] array) {
            for (int i = 0; i < array.Length; i++) {
                int index = Random.Range(0, array.Length);
                
                if (index == i) {
                    continue;
                }
                
                (array[index], array[i]) = (array[i], array[index]);
            }
            
            return array;
        }
    }
}