// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Unity {
    public static class GameObjectUtility {
        public static T[] Instantiate<T>(T prefab, Transform parent, int count) where T : MonoBehaviour {
            T[] result = new T[count];
            
            for (int i = 0; i < count; i++) {
                result[i] = Object.Instantiate(prefab, parent);
            }
            
            return result;
        }
        
    #if UNITY_EDITOR
        
        public static bool TryFindObjectsOfTypePrefab<T>(out T[] result) where T : Object {
            UnityEditor.SceneManagement.PrefabStage stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            
            if (stage != null) {
                result = stage.prefabContentsRoot.GetComponentsInChildren<T>(true);
                return true;
            }
            
            result = null;
            return false;
        }
        
    #endif
    }
}