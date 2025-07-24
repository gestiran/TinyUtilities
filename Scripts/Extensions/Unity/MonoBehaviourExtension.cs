// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class MonoBehaviourExtension {
        public static T[] GetComponents<T>(this ICollection<MonoBehaviour> objects) where T : Object {
            List<T> result = new List<T>(objects.Count);
            
            foreach (MonoBehaviour obj in objects) {
                T component = obj.GetComponent<T>();
                
                if (component == null) {
                    continue;
                }
                
                result.Add(component);
            }
            
            return result.ToArray();
        }
    }
}