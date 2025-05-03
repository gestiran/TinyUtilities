using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Unity {
    public static class Vector3Utility {
        public static float DistanceXZ(Vector3 from, Vector3 to) {
            float xDiff = from.x - to.x;
            float zDiff = from.z - to.z;
            return Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff);
        }
        
        public static Vector3 Avg(Vector3[] values) {
            if (values.Length == 0) {
                return Vector3.zero;
            }
            
            Vector3 result = Vector3.zero;
            
            for (int i = 0; i < values.Length; i++) {
                result += values[i];
            }
            
            return result / values.Length;
        }
        
        public static Vector3 Create(float value) => new Vector3(value, value, value);
        
        public static Vector3[] CreateArray(float value, int count) {
            Vector3[] result = new Vector3[count];
            
            for (int i = 0; i < count; i++) {
                result[i] = Create(value);
            }
            
            return result;
        }
        
        public static Vector3 GetXZDirection(Vector3 current, Vector3 target) {
            Vector3 direction = (target - current).normalized;
            
            return new Vector3(direction.x, 0, direction.z);
        }
        
        public static Vector3 GetXZDirection(Vector3 velocity) {
            Vector3 direction = velocity.normalized;
            
            return new Vector3(direction.x, 0, direction.z);
        }
        
        public static Vector3 MoveTowardsUnclamped(Vector3 current, Vector3 target, float maxDistanceDelta) {
            Vector3 diff = (target - current).normalized * 1000f;
            
            return current + (target + diff - (current - diff)).normalized * maxDistanceDelta;
        }
        
        public static List<Vector3> GetDirectionsByAngle(int count) {
            if (count == 0) {
                return new List<Vector3>();
            }
            
            List<Vector3> result = new List<Vector3>(count);
            float angleStep = 360f / count;
            
            for (int i = 0; i < count; i++) {
                result.Add(Quaternion.Euler(0, i * angleStep, 0) * Vector3.forward);
            }
            
            return result;
        }
    }
}