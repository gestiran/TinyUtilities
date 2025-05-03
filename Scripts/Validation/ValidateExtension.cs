using UnityEngine;

namespace TinyUtilities.Validation {
    public static class ValidateExtension {
    #if UNITY_EDITOR
        public static bool ValidateNotCurrent<T1, T2>(this T1 component, T2 current) where T1 : Component where T2 : Component {
            if (current != null) {
                T2 currentComponent = component.GetComponent<T2>();
                
                if (currentComponent != null && currentComponent.GetInstanceID() != current.GetInstanceID()) {
                    return true;
                }
            }
            
            return false;
        }
    #endif
    }
}