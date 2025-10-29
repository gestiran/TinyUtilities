// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;
using UnityObject = UnityEngine.Object;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Validation {
#if UNITY_EDITOR
    public static class ResourcesValidate {
        public static bool TryLoad<T>(string path, SelfValidationResult result) where T : UnityObject => TryLoad(path, result, out T _);
        
        public static bool TryLoad<T>(string path, SelfValidationResult result, out T parameters) where T : UnityObject {
            parameters = Resources.Load<T>(path);
            
            if (parameters == null) {
                result.AddError($"{typeof(T).Name} not found in Resources/{path}!");
                return false;
            }
            
            return true;
        }
        
        public static T Create<T>(string path, string name) where T : ScriptableObject {
            T instance = ScriptableObject.CreateInstance<T>();
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(instance, $"Assets/Resources/{path}/{name}.asset");
        #endif
            return instance;
        }
    }
#endif
}