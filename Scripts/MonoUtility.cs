// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities {
    public static class MonoUtility {
        public static T InstantiateAsPrefab<T>(T original, Transform parent) where T : MonoBehaviour {
        #if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) {
                Transform transform = original.transform;
                
                return InstantiateAsPrefab(original, transform.position, transform.rotation, parent);
            }
        #endif
            return Object.Instantiate(original, parent);
        }
        
        public static T InstantiateAsPrefab<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : MonoBehaviour {
        #if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) {
                Object prefab = UnityEditor.PrefabUtility.InstantiatePrefab(original.gameObject, parent);
                
                if (prefab is GameObject gameObject) {
                    T instance = gameObject.GetComponent<T>();
                    
                    if (instance != null) {
                        instance.transform.position = position;
                        instance.transform.rotation = rotation;
                        return instance;
                    }
                }
            }
        #endif
            return Object.Instantiate(original, position, rotation, parent);
        }
    }
}