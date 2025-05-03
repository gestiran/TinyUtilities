using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TinyUtilities.Editor.HotKeys {
    public static class DisableShadowsHotkey {
        [MenuItem("GameObject/Disable Shadows/Children", false, 18)]
        public static void DisableShadowsChildren() {
            Object[] objects = Selection.objects;
            
            for (int i = 0; i < objects.Length; i++) {
                if (objects[i] is not GameObject gameObject) {
                    continue;
                }
                
                Transform transform = gameObject.transform;
                int childCount = transform.childCount;
                
                if (childCount == 0) {
                    continue;
                }
                
                for (int childId = 0; childId < childCount; childId++) {
                    Transform child = transform.GetChild(childId);
                    
                    if (child.childCount == 0) {
                        continue;
                    }
                    
                    MeshRenderer meshRenderer = child.GetChild(0).GetComponent<MeshRenderer>();
                    
                    if (meshRenderer == null) {
                        continue;
                    }
                    
                    meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                    EditorUtility.SetDirty(meshRenderer);
                }
            }
        }
        
        [MenuItem("GameObject/Disable Shadows/Child", false, 18)]
        public static void DisableShadowsChild() {
            Object[] objects = Selection.objects;
            
            for (int i = 0; i < objects.Length; i++) {
                if (objects[i] is not GameObject gameObject) {
                    continue;
                }
                
                Transform transform = gameObject.transform;
                
                if (transform.childCount == 0) {
                    continue;
                }
                
                MeshRenderer meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
                
                if (meshRenderer == null) {
                    continue;
                }
                
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                EditorUtility.SetDirty(meshRenderer);
            }
        }
        
        [MenuItem("GameObject/Disable Shadows/Current", false, 18)]
        public static void DisableShadowsCurrent() {
            Object[] objects = Selection.objects;
            
            for (int i = 0; i < objects.Length; i++) {
                if (objects[i] is not GameObject gameObject) {
                    continue;
                }
                
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                
                if (meshRenderer == null) {
                    continue;
                }
                
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                EditorUtility.SetDirty(meshRenderer);
            }
        }
    }
}