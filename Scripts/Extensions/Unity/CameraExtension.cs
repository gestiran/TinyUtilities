// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;
using TinyUtilities.CustomTypes;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class CameraExtension {
        [Pure]
        public static float CalculateObjectResize(this Camera camera, in Vector3 position, in ResizeData data) {
            return camera.CalculateObjectResize(position, data.size, data.distance, data.fov);
        }
        
        [Pure]
        public static float CalculateObjectResize(this Camera camera, in Vector3 position, in float size, in float distance, in float fov) {
            float currentDistance = Vector3.Distance(camera.transform.position, position);
            float currentFovRad = camera.fieldOfView * Mathf.Deg2Rad;
            float referenceFovRad = Mathf.Max(fov, 1f) * Mathf.Deg2Rad;
            
            return Mathf.Max(size, 0.01f) * (currentDistance / Mathf.Max(distance, 0.01f)) * (Mathf.Tan(currentFovRad / 2f) / Mathf.Tan(referenceFovRad / 2f));
        }
    }
}