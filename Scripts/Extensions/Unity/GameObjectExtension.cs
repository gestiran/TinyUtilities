using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class GameObjectExtension {
        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component) where T : Component {
            component = gameObject.GetComponentInParent<T>();
            return component != null;
        }
        
        public static void SetActive<T>(this T objects, bool value) where T : ICollection<GameObject> {
            foreach (GameObject gameObject in objects) {
                gameObject.SetActive(value);
            }
        }
        
        public static void SetActiveTrue(this GameObject gameObject) => gameObject.SetActive(true);
        
        public static void SetActiveFalse(this GameObject gameObject) => gameObject.SetActive(false);
        
        public static bool TryGetComponent<T>(this ICollection<GameObject> gameObjects, out T component) where T : Object {
            component = null;
            
            foreach (GameObject gameObject in gameObjects) {
                component = gameObject.GetComponent<T>();
                
                if (component == null) {
                    continue;
                }
                
                return true;
            }
            
            return false;
        }
        
        public static void SetActiveSelf(this GameObject gameObject, bool value) {
            if (gameObject.activeSelf == value) {
                return;
            }
            
            gameObject.SetActive(value);
        }
        
        public static void SetActiveSelfInverted(this GameObject gameObject, bool value) => gameObject.SetActiveSelf(value == false);
        
        public static void SetLayer(this GameObject[] gameObjects, int layer) {
            for (int objId = 0; objId < gameObjects.Length; objId++) {
                if (gameObjects[objId].layer == layer) {
                    continue;
                }
                
            #if UNITY_EDITOR
                
                if (!UnityEditor.EditorApplication.isPlaying) {
                    UnityEditor.EditorUtility.SetDirty(gameObjects[objId]);
                }
                
            #endif
                
                gameObjects[objId].layer = layer;
            }
        }
        
        public static GameObject GetChildByName(this GameObject gameObject, string name) {
            Transform transform = gameObject.transform;
            int childCount = transform.childCount;
            
            for (int childId = 0; childId < childCount; childId++) {
                GameObject child = transform.GetChild(childId).gameObject;
                
                if (child.name.Contains(name)) {
                    return child;
                }
            }
            
            return null;
        }
        
    #if UNITY_EDITOR
        public static bool IsSceneAsset(this GameObject gameObject) {
            if (UnityEditor.PrefabUtility.IsPartOfAnyPrefab(gameObject)) {
                return UnityEditor.PrefabUtility.GetPrefabInstanceStatus(gameObject) == UnityEditor.PrefabInstanceStatus.Connected;
            }
            
            return UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) == null;
        }
    #endif
    }
}