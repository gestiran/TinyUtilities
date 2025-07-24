// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Validation {
    public static class ResourcesDebug {
        public static T Load<T>(string path) where T : Object {
            T parameters = Resources.Load<T>(path);
            
        #if UNITY_EDITOR || WITH_DEBUG
            if (parameters == null) {
                Debug.LogError($"{typeof(T).Name} not found in Resources/{path}!");
            }
        #endif
            
            return parameters;
        }
    }
}