// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace TinyUtilities.Extensions {
    public static class ListExtension {
        private const int _INSERTION_THRESHOLD = 16;
        
    #if EXTERNAL_DEPENDENCIES
        public static void Shuffle<T>(this List<T> list) {
            for (int i = 0; i < list.Count; i++) {
                int newIndex = RandomUtility.Range(0, list.Count);
                
                if (newIndex == i) {
                    continue;
                }
                
                (list[newIndex], list[i]) = (list[i], list[newIndex]);
            }
        }
    #endif
        
        public static void AddRange<T>(this List<T> list, T[] elements) {
            list.Capacity += elements.Length;
            
            for (int elementId = 0; elementId < elements.Length; elementId++) {
                list.Add(elements[elementId]);
            }
        }
        
        public static void AddRange<T>(this List<T> list, T[] elements, Func<T, bool> exclude) {
            list.Capacity += elements.Length;
            
            for (int elementId = 0; elementId < elements.Length; elementId++) {
                if (exclude(elements[elementId])) {
                    continue;
                }
                
                list.Add(elements[elementId]);
            }
        }
        
        public static void AddRange<T>(this List<T> list, List<T> elements) {
            list.Capacity += elements.Count;
            
            for (int elementId = 0; elementId < elements.Count; elementId++) {
                list.Add(elements[elementId]);
            }
        }
        
        public static void AddRange<T>(this List<T> list, List<T> elements, Func<T, bool> exclude) {
            list.Capacity += elements.Count;
            
            for (int elementId = 0; elementId < elements.Count; elementId++) {
                if (exclude(elements[elementId])) {
                    continue;
                }
                
                list.Add(elements[elementId]);
            }
        }
        
        [Pure]
        public static bool IsContainValue<T>(this IList<T> list, T value) {
            for (int elementId = 0; elementId < list.Count; elementId++) {
                if (!list[elementId].Equals(value)) {
                    continue;
                }
                
                return true;
            }
            
            return false;
        }
        
    #if EXTERNAL_DEPENDENCIES
        public static T Any<T>(this List<T> list, T defaultValue = default) {
            if (list.Count > 0) {
                return list[RandomUtility.Range(0, list.Count - 1)];
            }
            
            return defaultValue;
        }
    #endif
        
        [Pure]
        public static int GetUniqueCount<T>(this IList<T> list) => list.GetUniqueCount(value => value.GetHashCode());
        
        [Pure]
        public static int GetUniqueCount<T>(this IList<T> list, Func<T, int> getHashCode) {
            List<int> diff = new List<int>();
            
            for (int i = 0; i < list.Count; i++) {
                int hash = getHashCode(list[i]);
                
                if (diff.Contains(hash)) {
                    continue;
                }
                
                diff.Add(hash);
            }
            
            return diff.Count;
        }
        
        [Pure]
        public static bool IsAllUniqueElements<T>(this IList<T> list) => list.IsAllUniqueElements(value => value.GetHashCode());
        
        [Pure]
        public static bool IsAllUniqueElements<T>(this IList<T> list, Func<T, int> getHashCode) {
            List<int> diff = new List<int>();
            
            for (int i = 0; i < list.Count; i++) {
                int hash = getHashCode(list[i]);
                
                if (diff.Contains(hash)) {
                    return false;
                }
                
                diff.Add(hash);
            }
            
            return true;
        }
        
        [Pure]
        public static bool TryGetValue<T>(this IList<T> list, int hash, out T result) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i].GetHashCode() != hash) {
                    continue;
                }
                
                result = list[i];
                return true;
            }
            
            result = default;
            return false;
        }
        
        [Pure]
        public static string ToStringArray<T>(this IList<T> list) {
            StringBuilder builder = new StringBuilder(list.Count);
            
            for (int i = 0; i < list.Count; i++) {
                builder.AppendFormat("{0} = {1}\n", i, list[i]);
            }
            
            return builder.ToString();
        }
        
        public static string ToStringArrayValues<T>(this List<T> list) => list.ToStringArrayValues(value => value.ToString());
        
        public static string ToStringArrayValues<T>(this List<T> list, Func<T, string> toString) {
            StringBuilder builder = new StringBuilder(list.Count);
            
            if (list.Count > 0) {
                builder.AppendFormat("{0}", toString(list[0]));
            }
            
            for (int i = 1; i < list.Count; i++) {
                builder.AppendFormat(", {0}", toString(list[i]));
            }
            
            return builder.ToString();
        }
        
        public static List<T> Reverse<T>(this List<T> list) {
            int length = list.Count / 2;
            
            for (int i = 0; i < length; i++) {
                (list[i], list[list.Count - 1 - i]) = (list[list.Count - 1 - i], list[i]);
            }
            
            return list;
        }
        
        public static void SortStable<T>(this IList<T> list) where T : IComparable<T> {
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }
            
            int count = list.Count;
            
            if (count < 2) {
                return;
            }
            
            T[] buffer = new T[count];
            
            MergeSort(list, buffer, 0, count);
        }
        
        private static void MergeSortNR<T>(IList<T> list, T[] buffer, int left, int right) where T : IComparable<T> {
            MergeSort(list, buffer, left, right);
        }
        
        private static void MergeSort<T>(IList<T> list, T[] buffer, int left, int right) where T : IComparable<T> {
            int length = right - left;
            
            if (length < 2) {
                return;
            }
            
            if (length <= _INSERTION_THRESHOLD) {
                InsertionSort(list, left, right);
                return;
            }
            
            int middle = left + length / 2;
            
            MergeSortNR(list, buffer, left, middle);
            MergeSortNR(list, buffer, middle, right);
            
            if (list[middle - 1].CompareTo(list[middle]) <= 0) {
                return;
            }
            
            Merge(list, buffer, left, middle, right);
        }
        
        private static void InsertionSort<T>(IList<T> list, int left, int right) where T : IComparable<T> {
            for (int i = left + 1; i < right; i++) {
                T current = list[i];
                int j = i - 1;
                
                while (j >= left && list[j].CompareTo(current) > 0) {
                    list[j + 1] = list[j];
                    j--;
                }
                
                list[j + 1] = current;
            }
        }
        
        private static void Merge<T>(IList<T> list, T[] buffer, int left, int middle, int right) where T : IComparable<T> {
            int i = left;
            int j = middle;
            int k = left;
            
            while (i < middle && j < right) {
                if (list[i].CompareTo(list[j]) <= 0) {
                    buffer[k] = list[i];
                    i++;
                } else {
                    buffer[k] = list[j];
                    j++;
                }
                
                k++;
            }
            
            while (i < middle) {
                buffer[k] = list[i];
                i++;
                k++;
            }
            
            while (j < right) {
                buffer[k] = list[j];
                j++;
                k++;
            }
            
            for (int p = left; p < right; p++) {
                list[p] = buffer[p];
            }
        }
    }
}