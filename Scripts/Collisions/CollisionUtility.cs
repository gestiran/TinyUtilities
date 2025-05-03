using System.Collections.Generic;
using TinyUtilities.Extensions.Unity;
using UnityEngine;

namespace TinyUtilities.Collisions {
    public static class CollisionUtility {
        private const float _CIRCLE_ANGLE = 360f;
        
    #if UNITY_EDITOR
        private const bool _DEBUG = false;
    #endif
        
        public class RaycastResult {
            public Vector3 origin;
            public Vector3 point;
            public float distance;
        }
        
        public static Vector3 CorrectCollision<T>(T collider, Vector3 nextPosition, float verticalOffset, int raysCount, int mask) where T : IColliderData {
            Vector3 nextOrigin = collider.getPosition;
            nextOrigin.y += verticalOffset;
            
            Ray[] rays = CreateCircleRays(nextOrigin, collider.getVelocity.normalized, raysCount);
            
        #if UNITY_EDITOR
            if (_DEBUG) {
                DrawRaysDebug(rays, Color.red, 0.1f);
            }
        #endif
            
            List<RaycastResult> raycasts = GetHorizontalHits(rays, collider.getRadius, mask);
            Vector2[] endPoints = CalculateOffsetPosition(raycasts, collider.getRadius);
            
            if (!endPoints.TryCalculateAverage(out Vector2 endPoint)) {
                return nextPosition;
            }
            
            Vector2 lastVelocity = new Vector2(collider.getVelocity.x, collider.getVelocity.z);
            float lastMagnitude = lastVelocity.magnitude;
            
            if (lastMagnitude != 0) {
                Vector2 currentVelocity = new Vector2(endPoint.x - nextPosition.x, endPoint.y - nextPosition.z);
                float currentMagnitude = currentVelocity.magnitude;
                
                if (currentMagnitude > lastMagnitude) {
                    float downgrade = Mathf.Sqrt(lastMagnitude / currentMagnitude);
                    endPoint = new Vector2(nextPosition.x + currentVelocity.x * downgrade, nextPosition.z + currentVelocity.y * downgrade);
                }
            }
            
            nextPosition.x = endPoint.x;
            nextPosition.z = endPoint.y;
            
            return nextPosition;
        }
        
        public static float CorrectVertical<T>(T collision, float height, int mask) where T : IColliderData {
            Vector3 origin = collision.getPosition + collision.getVelocity;
            origin.y += height;
            
            return CorrectVerticalDown(origin, height, collision.getPosition.y, mask);
        }
        
        public static float CorrectVerticalDown(Vector3 origin, float height, float positionY, int mask) {
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, height, mask)) {
                return hit.point.y;
            }
            
            if (Physics.Raycast(origin, Vector3.down, out hit, height + 0.5f, mask)) {
                return Mathf.Max(positionY + Physics.gravity.y * Time.fixedDeltaTime, hit.point.y);
            }
            
            return positionY + Physics.gravity.y * Time.fixedDeltaTime;
        }
        
        public static Ray[] CreateCircleRays(Vector3 origin, Vector3 direction, int count) {
            Ray[] rays = new Ray[count];
            
            for (int rayId = 0; rayId < rays.Length; rayId++) {
                float angle = rayId * (_CIRCLE_ANGLE / count);
                rays[rayId] = new Ray(origin, Quaternion.Euler(0, angle, 0) * direction);
            }
            
            return rays;
        }
        
        public static List<RaycastResult> GetHorizontalHits(Ray[] rays, float radius, int mask) {
            List<RaycastResult> raycasts = new List<RaycastResult>(rays.Length);
            
            for (int rayId = 0; rayId < rays.Length; rayId++) {
                if (!Physics.Raycast(rays[rayId], out RaycastHit hit, radius, mask)) {
                    continue;
                }
                
                RaycastResult result = new RaycastResult();
                
                result.origin = rays[rayId].origin;
                result.point = hit.point;
                result.distance = hit.distance;
                
                raycasts.Add(result);
            }
            
            return raycasts;
        }
        
        public static Vector2[] CalculateOffsetPosition(List<RaycastResult> raycasts, float radius) {
            Vector2[] positions = new Vector2[raycasts.Count];
            
            for (int hitId = 0; hitId < raycasts.Count; hitId++) {
                positions[hitId] = CalculateOffsetPosition(raycasts[hitId], radius);
            }
            
            return positions;
        }
        
        public static Vector2 CalculateOffsetPosition(RaycastResult raycast, float radius) {
            Vector2 offset = new Vector2(raycast.origin.x - raycast.point.x, raycast.origin.z - raycast.point.z);
            Vector2 endPoint = new Vector2(raycast.origin.x + offset.x * 1.25f, raycast.origin.z + offset.y * 1.25f);
            
            var result = Vector2.MoveTowards(new Vector2(raycast.origin.x, raycast.origin.z), endPoint, radius - raycast.distance);
            
            Debug.DrawLine(raycast.origin, new Vector3(result.x, raycast.origin.y, result.y), Color.blue, 0.2f);
            
            return result;
        }
        
    #if UNITY_EDITOR
        
        private static void DrawRaysDebug(Ray[] rays, Color color, float duration) {
            for (int rayId = 0; rayId < rays.Length; rayId++) {
                Debug.DrawRay(rays[rayId].origin, rays[rayId].direction, color, duration);
            }
        }
        
    #endif
    }
}