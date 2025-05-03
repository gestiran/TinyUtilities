using UnityEngine;
using UnityEngine.SceneManagement;

namespace TinyUtilities.Unity {
    public static class ScenesUtility {
        public static string GetSceneName(int buildId) {
        #if UNITY_EDITOR
            if (buildId >= 0 && buildId < UnityEditor.EditorBuildSettings.scenes.Length) {
                string path = UnityEditor.EditorBuildSettings.scenes[buildId].path;
                
                if (string.IsNullOrEmpty(path) == false) {
                    string fileName = System.IO.Path.GetFileName(path);
                    
                    if (string.IsNullOrEmpty(fileName) == false) {
                        return fileName.Substring(0, fileName.Length - 6);
                    }
                }
            }
            
        #endif
            
            return SceneManager.GetSceneByBuildIndex(buildId).name;
        }
        
        public static GameObject NewObject(string name) => NewObject(name, default);
        
        public static GameObject NewObject(string name, Transform parent) {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
        #endif
            return obj;
        }
        
        public static GameObject NewObjectFirst(string name) => NewObject(name, default);
        
        public static GameObject NewObjectFirst(string name, Transform parent) {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.transform.SetSiblingIndex(0);
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
        #endif
            return obj;
        }
    }
}