// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;
using UnityEngine;

namespace TinyUtilities.Extensions {
    public static class AnimatorExtension {
        [Pure]
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