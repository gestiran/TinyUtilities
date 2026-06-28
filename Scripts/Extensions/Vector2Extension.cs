// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace TinyUtilities.Extensions {
    public static class Vector2Extension {
        [Pure]
        public static bool TryCalculateAverage(this List<Vector2> positions, out Vector2 result) {
            result = new Vector2(0, 0);
            
            if (positions.Count == 0) {
                return false;
            }
            
            for (int pointId = 0; pointId < positions.Count; pointId++) {
                result += positions[pointId];
            }
            
            result /= positions.Count;
            return true;
        }
        
        [Pure]
        public static bool TryCalculateAverage(this Vector2[] positions, out Vector2 result) {
            result = new Vector2(0, 0);
            
            if (positions.Length == 0) {
                return false;
            }
            
            for (int pointId = 0; pointId < positions.Length; pointId++) {
                result += positions[pointId];
            }
            
            result /= positions.Length;
            return true;
        }
    }
}