using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.CustomTypes {
    public sealed class Triangle3 {
        public readonly Vector3 a;
        public readonly Vector3 b;
        public readonly Vector3 c;
        
        public Triangle3(Vector3 a, Vector3 b, Vector3 c) {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        
        public List<Vector3Int> GetPointsInside() {
            List<Vector3Int> result = new List<Vector3Int>();
            
            int minX = Mathf.FloorToInt(Mathf.Min(a.x, Mathf.Min(b.x, c.x))) - 1;
            int minZ = Mathf.FloorToInt(Mathf.Min(a.z, Mathf.Min(b.z, c.z))) - 1;
            int maxX = Mathf.CeilToInt(Mathf.Max(a.x, Mathf.Max(b.x, c.x))) + 1;
            int maxZ = Mathf.CeilToInt(Mathf.Max(a.z, Mathf.Max(b.z, c.z))) + 1;
            
            Vector3 center = (a + b + c) / 3f;
            
            for (int x = minX; x <= maxX; x++) {
                for (int z = minZ; z <= maxZ; z++) {
                    Vector3Int currentPoint = new Vector3Int(x, 0, z);
                    
                    if (IsPointInside(currentPoint + (center - currentPoint).normalized)) {
                        result.Add(currentPoint);
                    }
                }
            }
            
            return result;
        }
        
        private bool IsPointInside(Vector3 p) {
            float alpha = ((b.z - c.z) * (p.x - c.x) + (c.x - b.x) * (p.z - c.z)) / ((b.z - c.z) * (a.x - c.x) + (c.x - b.x) * (a.z - c.z));
            float beta = ((c.z - a.z) * (p.x - c.x) + (a.x - c.x) * (p.z - c.z)) / ((b.z - c.z) * (a.x - c.x) + (c.x - b.x) * (a.z - c.z));
            
            float gamma = 1 - alpha - beta;
            
            float epsilon = 0.0001f;
            return alpha > -epsilon && beta > -epsilon && gamma > -epsilon;
        }
    }
}