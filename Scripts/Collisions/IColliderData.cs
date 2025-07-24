// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Collisions {
    public interface IColliderData {
        public Vector3 getPosition { get; }
        public Vector3 getVelocity { get; }
        public float getRadius { get; }
    }
}