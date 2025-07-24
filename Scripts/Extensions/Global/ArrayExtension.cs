// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace TinyUtilities.Extensions.Global {
    public static class ArrayExtension {
        [Obsolete("Do nothing", true)]
        public static T[] AddToArray<T>(this T[] array) => array;
        
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
        
        public static int[] GetNumbersArray(this int length) {
            int[] result = new int[length];
            
            for (int i = 0; i < length; i++) {
                result[i] = i;
            }
            
            return result;
        }
        
        public static GameObject[] ToGameObjectArray<T>(this T[] array) where T : MonoBehaviour {
            GameObject[] result = new GameObject[array.Length];
            
            for (int i = 0; i < result.Length; i++) {
                result[i] = array[i].GetComponent<GameObject>();
            }
            
            return result;
        }
        
        public static bool IsContainObjectOfType<T>(this T[] array, T element) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i].GetType() == element.GetType()) {
                    return true;
                }
            }
            
            return false;
        }
        
        public static bool IsContain<T>(this T[] array, T element) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i].Equals(element)) {
                    return true;
                }
            }
            
            return false;
        }
        
        public static int GetMiddle(this int[] array) {
            int result = array.Sum();
            result /= array.Length;
            
            return result;
        }
        
        public static string ToStringArray<T>(this T[] array) {
            StringBuilder builder = new StringBuilder(array.Length);
            
            for (int i = 0; i < array.Length; i++) {
                builder.AppendFormat("{0} = {1}\n", i, array[i]);
            }
            
            return builder.ToString();
        }
        
        public static T Random<T>(this T[] array, T defaultValue = default) {
            if (array.Length > 0) {
                return array[UnityRandom.Range(0, array.Length - 1)];
            }
            
            return defaultValue;
        }
        
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
        
        public static int FindClosestIndex(this float[] arr, float target) {
            if (arr == null || arr.Length == 0) {
                return 0;
            }
            
            int closestIndex = 0;
            float minDiff = Mathf.Abs(arr[0] - target);
            
            for (int i = 1; i < arr.Length; i++) {
                float diff = Mathf.Abs(arr[i] - target);
                
                if (diff >= minDiff) {
                    continue;
                }
                
                minDiff = diff;
                closestIndex = i;
            }
            
            return closestIndex;
        }
    }
}