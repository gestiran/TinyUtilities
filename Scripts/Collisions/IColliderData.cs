using UnityEngine;

namespace TinyUtilities.Collisions {
    public interface IColliderData {
        public Vector3 getPosition { get; }
        public Vector3 getVelocity { get; }
        public float getRadius { get; }
    }
}