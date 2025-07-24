// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Unity {
    public static class QuaternionUtility {
        public static Quaternion LookY(Vector3 direction) => LookY(direction, Vector3.up);
        
        public static Quaternion LookY(Vector3 direction, Vector3 upward) {
            Quaternion rotation = Quaternion.LookRotation(direction, upward);
            
            rotation.y = 0;
            rotation.w = 0;
            
            return rotation;
        }
        
        public static Quaternion LookYNormalized(Vector3 direction) => LookY(direction, Vector3.up).normalized;
        
        public static Quaternion LookYNormalized(Vector3 direction, Vector3 upward) => LookY(direction, upward).normalized;
    }
}