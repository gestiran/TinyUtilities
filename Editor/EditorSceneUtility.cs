// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TinyUtilities.Editor {
    public static class EditorSceneUtility {
        public static void InvokeOnAllActionScenes(Action action) {
            string folderPath = "Assets/Scenes/Action";
            
            InvokeOnAllScenes(folderPath, action);
        }
        
        public static void InvokeOnAllScenes(string scenesFolderPath, Action action) {
            bool confirm = EditorUtility.DisplayDialog("Confirm action", "Are you sure you want to perform this on all scenes? it could take quite a while", "Yes", "No");
            
            if (!confirm) {
                return;
            }
            
            string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene", new[] { scenesFolderPath });
            
            if (sceneGUIDs.Length == 0) {
                Debug.LogWarning($"No scenes found in {scenesFolderPath}");
                return;
            }
            
            string[] scenePaths = sceneGUIDs.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            
            for (int i = 0; i < scenePaths.Length; i++) {
                EditorSceneManager.OpenScene(scenePaths[i]);
                Scene currentScene = EditorSceneManager.GetActiveScene();
                
                action?.Invoke();
                
                EditorSceneManager.MarkSceneDirty(currentScene);
                EditorSceneManager.SaveScene(currentScene);
            }
        }
    }
}