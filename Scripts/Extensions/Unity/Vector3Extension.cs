using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class Vector3Extension {
        public static Vector3 Abs(this Vector3 source) {
            return new Vector3(Mathf.Abs(source.x), Mathf.Abs(source.y), Mathf.Abs(source.z));
        }
        
        public static Vector3Int ToVectorInt(this Vector3 source) {
            return new Vector3Int(MathfUtility.RoundToInt(source.x), MathfUtility.RoundToInt(source.y), MathfUtility.RoundToInt(source.z));
        }
        
        public static List<Vector3Int> ToVectorInt(this List<Vector3> source) {
            List<Vector3Int> result = new List<Vector3Int>(source.Count);
            
            for (int i = 0; i < source.Count; i++) {
                result.Add(source[i].ToVectorInt());
            }
            
            return result;
        }
        
        public static Vector2 XZ(this Vector3 vector) => new Vector2(vector.x, vector.z);
        
        public static Vector3 Upscale(this Vector3 vector, float value) => new Vector3(vector.x * value, vector.y * value, vector.z * value);
        
        public static Vector3[] Add(this Vector3[] arr, Vector3 value) {
            Vector3[] result = new Vector3[arr.Length];
            
            for (int i = 0; i < arr.Length; i++) {
                result[i] = arr[i] + value;
            }
            
            return result;
        }
        
        public static Vector3[] ToVector3(this Vector2[] vectors) {
            Vector3[] result = new Vector3[vectors.Length];
            
            for (int vectorId = 0; vectorId < vectors.Length; vectorId++) {
                result[vectorId] = vectors[vectorId];
            }
            
            return result;
        }
        
        public static Vector3 ScaleY(this Vector3 vector, float value) => new Vector3(vector.x, vector.y * value, vector.z);
        
        public static bool IsInsideXZ(this Vector3 position, Vector3[] points) {
            bool isInside = false;
            
            for (int pointId = 0, endPointId = points.Length - 1; pointId < points.Length; endPointId = pointId++) {
                if (points[pointId].z > position.z != points[endPointId].z > position.z
                 && position.x < (points[endPointId].x - points[pointId].x) * (position.z - points[pointId].z) / (points[endPointId].z - points[pointId].z) + points[pointId].x) {
                    isInside = !isInside;
                }
            }
            
            return isInside;
        }
        
        public static Vector3 RotateY(this Vector3 vector, float angle) => Quaternion.Euler(0, angle, 0) * vector;
        
        public static Vector3 MultiplyValues(this Vector3 thisVector, Vector3 other) {
            return new Vector3(thisVector.x * other.x, thisVector.y * other.y, thisVector.z * other.z);
        }
        
        public static float Median(this Vector3 other) => (other.x + other.y + other.z) / 3f;
        
        public static Vector3 Median(this Vector3[] other) {
            Vector3 result = Vector3.zero;
            
            for (int i = 0; i < other.Length; i++) {
                result += other[i];
            }
            
            return result / other.Length;
        }
        
        public static float MedianX(this Vector3[] other) {
            float result = 0;
            
            for (int i = 0; i < other.Length; i++) {
                result += other[i].x;
            }
            
            return result / other.Length;
        }
        
        public static float MedianY(this Vector3[] other) {
            float result = 0;
            
            for (int i = 0; i < other.Length; i++) {
                result += other[i].y;
            }
            
            return result / other.Length;
        }
        
        public static float MedianZ(this Vector3[] other) {
            float result = 0;
            
            for (int i = 0; i < other.Length; i++) {
                result += other[i].z;
            }
            
            return result / other.Length;
        }
        
        public static Vector3 DivideValues(this Vector3 thisVector, Vector3 other) {
            if (other.x == 0f) {
                other.x = 1f;
            }
            
            if (other.y == 0f) {
                other.y = 1f;
            }
            
            if (other.z == 0f) {
                other.z = 1f;
            }
            
            return new Vector3(thisVector.x / other.x, thisVector.y / other.y, thisVector.z / other.z);
        }
        
        public static Vector3 Rotate(this Vector3 point, Vector3 center, Vector3 rotate) {
            point = RotateAroundX(point, center, rotate.x * Mathf.Deg2Rad);
            point = RotateAroundY(point, center, rotate.y * Mathf.Deg2Rad);
            point = RotateAroundZ(point, center, rotate.z * Mathf.Deg2Rad);
            
            return point;
        }
        
        public static bool TryCalculateAverage(this List<Vector3> positions, out Vector3 result) {
            result = Vector3.zero;
            
            if (positions.Count == 0) {
                return false;
            }
            
            for (int pointId = 0; pointId < positions.Count; pointId++) {
                result += positions[pointId];
            }
            
            result /= positions.Count;
            
            return true;
        }
        
        public static List<Vector3> FilterLineCast(this List<Vector3> positions, Vector3 from, int layerMask) {
            for (int positionId = positions.Count - 1; positionId >= 0; positionId--) {
                if (Physics.Linecast(from, positions[positionId], layerMask) == false) {
                    continue;
                }
                
                positions.RemoveAt(positionId);
            }
            
            return positions;
        }
        
        public static List<Vector3> FilterNearest<T>(this List<Vector3> positions, T filter, float distance = 0.1f) where T : IEnumerable<Vector3> {
            for (int positionId = positions.Count - 1; positionId >= 0; positionId--) {
                if (IsContainNearest(filter, positions[positionId], distance) == false) {
                    continue;
                }
                
                positions.RemoveAt(positionId);
            }
            
            return positions;
        }
        
        public static bool IsContainNearest<T>(this T positions, Vector3 position, float distance = 0.1f) where T : IEnumerable<Vector3> {
            foreach (Vector3 target in positions) {
                if (Vector3.Distance(target, position) > distance) {
                    continue;
                }
                
                return true;
            }
            
            return false;
        }
        
        private static Vector3 RotateAroundX(Vector3 point, Vector3 center, float angle) {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);
            
            float XX = point.x - center.x;
            float YY = point.y - center.y;
            float ZZ = point.z - center.z;
            
            point.x = center.x + XX;
            point.y = center.y + YY * cos - ZZ * sin;
            point.z = center.z + YY * sin + ZZ * cos;
            
            return point;
        }
        
        private static Vector3 RotateAroundY(Vector3 point, Vector3 center, float angle) {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);
            
            float XX = point.x - center.x;
            float YY = point.y - center.y;
            float ZZ = point.z - center.z;
            
            point.x = center.x + XX * cos + ZZ * sin;
            point.y = center.y + YY;
            point.z = center.z - XX * sin + ZZ * cos;
            
            return point;
        }
        
        private static Vector3 RotateAroundZ(Vector3 point, Vector3 center, float angle) {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);
            
            float XX = point.x - center.x;
            float YY = point.y - center.y;
            float ZZ = point.z - center.z;
            
            point.x = center.x + XX * cos - YY * sin;
            point.y = center.y + XX * sin + YY * cos;
            point.z = center.z + ZZ;
            
            return point;
        }
    }
}