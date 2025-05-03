using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class AnimatorExtension {
        public static bool IsHaveParameter(this Animator animator, string parameterName) {
            #if UNITY_EDITOR
            animator.logWarnings = false;
            #endif
            
            AnimatorControllerParameter[] parameters = animator.parameters;
            
            for (int parameterId = 0; parameterId < parameters.Length; parameterId++) {
                if (parameters[parameterId].name != parameterName) {
                    continue;
                }
                
                return true;
            }
            
            return false;
        }
    }
}