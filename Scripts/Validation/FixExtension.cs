using UnityEngine;

namespace TinyUtilities.Validation {
    public static class FixExtension {
    #if UNITY_EDITOR
        public static void FixComponent<T1, T2>(this T1 component, out T2 result) where T1 : Component where T2 : Component {
            result = component.GetComponent<T2>();
            
            if (result == null) {
                Debug.LogError($"Component {nameof(T2)} not found!");
                return;
            }
            
            UnityEditor.EditorUtility.SetDirty(component);
        }
    #endif
    }
}