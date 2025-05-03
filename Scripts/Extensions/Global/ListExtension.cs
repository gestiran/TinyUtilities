using System;
using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;

namespace TinyUtilities.Extensions.Global {
    public static class ListExtension {
        public static void Mix<T>(this List<T> list) {
            for (int i = 0; i < list.Count; i++) {
                int newIndex = UnityRandom.Range(0, list.Count);
                (list[newIndex], list[i]) = (list[i], list[newIndex]);
            }
        }
        
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
        
        public static bool IsContainValue<T>(this List<T> list, T value) {
            for (int elementId = 0; elementId < list.Count; elementId++) {
                if (!list[elementId].Equals(value)) {
                    continue;
                }
                
                return true;
            }
            
            return false;
        }
    }
}