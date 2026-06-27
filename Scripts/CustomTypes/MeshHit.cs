// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if UNITY_ENGINE
using UnityEngine;

namespace TinyUtilities.CustomTypes {
    public readonly struct MeshHit {
        public readonly Vector3 position;
        public readonly Color color;
        
        public MeshHit(Vector3 position, Color color) {
            this.position = position;
            this.color = color;
        }
    }
}
#endif