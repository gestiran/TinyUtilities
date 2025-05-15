using System.Collections.Generic;
using TinyUtilities.Extensions.Global;
using TinyUtilities.Unity;
using UnityEngine;
using UnityEngine.AI;

namespace TinyUtilities {
    public static class NavigationUtility {
        public static readonly NavMeshBuildSettings[] settings;
        
        private static readonly int[] _agentIds;
        
        public const float SAMPLE_PATH_DISTANCE = 0.25f;
        
        private const float _SAMPLE_WAYPOINT_DISTANCE = 3f;
        
        private const float _SAMPLE_CURRENT_DISTANCE_MIN = 0.25f;
        private const float _SAMPLE_CURRENT_DISTANCE_MAX = 50f;
        
        static NavigationUtility() {
            settings = CreateSettings();
            _agentIds = ConvertSettingsToIds(settings);
        }
        
        public static NavMeshQueryFilter CreateFilter(int areaMask, int agentId = 0) {
            NavMeshQueryFilter filter = new NavMeshQueryFilter();
            filter.areaMask = areaMask;
            filter.agentTypeID = _agentIds[Mathf.Clamp(agentId, 0, _agentIds.Length)];
            return filter;
        }
        
        public static Vector3 GetRandomPositionNearTarget(Vector3 targetPosition, NavMeshQueryFilter filter, float radius, int iterations = 10) {
            Vector2 randomDirection = Random.insideUnitCircle;
            Vector3 offset = new Vector3(randomDirection.x, 0, randomDirection.y) * Random.Range(1, radius);
            Vector3 position = targetPosition;
            
            for (int i = 1; i < iterations; i++) {
                if (TrySamplePosition(targetPosition + offset + Vector3.up * 5, filter, 4 * i, out position)) {
                    break;
                }
                
                randomDirection = Random.insideUnitCircle;
                offset = new Vector3(randomDirection.x, 0, randomDirection.y) * Random.Range(1, radius);
            }
            
            return position;
        }
        
        public static Vector3 GetRandomPositionOnRange(Vector3 targetPosition, NavMeshQueryFilter filter, float radius) {
            List<Vector3> directions = Vector3Utility.GetDirectionsByAngle(16);
            List<Vector3> results = new List<Vector3>();
            
            for (int i = 0; i < directions.Count; i++) {
                if (TrySamplePosition(targetPosition + directions[i] * radius + Vector3.up, filter, radius, out Vector3 position)) {
                    results.Add(position);
                }
            }
            
            if (results.Count > 0) {
                return results[Random.Range(0, results.Count)];
            }
            
            return targetPosition;
        }
        
        public static bool TryGetNearestWaypoint(Vector3 from, Vector3 to, NavMeshQueryFilter filter, out Vector3 result) {
            if (Vector3.Distance(from, to) < _SAMPLE_WAYPOINT_DISTANCE) {
                result = to;
                return true;
            }
            
            Vector3[] path = CalculatePath(from, to, filter, out NavMeshPathStatus status);
            
            if (path.Length > 1) {
                for (int cornerId = 1; cornerId < path.Length; cornerId++) {
                    if (Vector3.Distance(from, path[cornerId]) < _SAMPLE_WAYPOINT_DISTANCE) {
                        continue;
                    }
                    
                    result = path[cornerId];
                    return true;
                }
            }
            
            result = to;
            return true;
        }
        
        public static Vector3[] CalculatePath(Vector3 position, Vector3 target, NavMeshQueryFilter filter) => CalculatePath(position, target, filter, out _);
        
        public static Vector3[] CalculatePath(Vector3 position, Vector3 target, NavMeshQueryFilter filter, out NavMeshPathStatus pathStatus) {
            if (TrySamplePosition(position, filter, _SAMPLE_CURRENT_DISTANCE_MIN, out Vector3 samplePosition)) {
                if (TrySamplePosition(target, filter, _SAMPLE_CURRENT_DISTANCE_MIN, out Vector3 sampleTarget)) {
                    NavMeshPath navmeshPath = new NavMeshPath();
                    
                    if (NavMesh.CalculatePath(samplePosition, sampleTarget, filter, navmeshPath)) {
                        pathStatus = navmeshPath.status;
                        return navmeshPath.corners;
                    }
                    
                    pathStatus = navmeshPath.status;
                    return new[] { samplePosition, sampleTarget };
                }
                
                if (TrySamplePosition(target, filter, _SAMPLE_CURRENT_DISTANCE_MAX, out Vector3 onMeshTarget)) {
                    NavMeshPath navmeshPath = new NavMeshPath();
                    
                    if (NavMesh.CalculatePath(samplePosition, onMeshTarget, filter, navmeshPath)) {
                        pathStatus = navmeshPath.status;
                        return navmeshPath.corners.AddToArray(target);
                    }
                    
                    pathStatus = navmeshPath.status;
                    return new[] { samplePosition, onMeshTarget, target };
                }
                
                pathStatus = NavMeshPathStatus.PathInvalid;
                return new[] { samplePosition, target };
            }
            
            if (TrySamplePosition(position, filter, _SAMPLE_CURRENT_DISTANCE_MAX, out Vector3 onMeshPosition)) {
                if (TrySamplePosition(target, filter, _SAMPLE_CURRENT_DISTANCE_MIN, out Vector3 sampleTarget)) {
                    NavMeshPath navmeshPath = new NavMeshPath();
                    
                    if (NavMesh.CalculatePath(onMeshPosition, sampleTarget, filter, navmeshPath)) {
                        pathStatus = navmeshPath.status;
                        return new Vector3[] { position, onMeshPosition }.AddToArray(navmeshPath.corners);
                    }
                    
                    pathStatus = navmeshPath.status;
                    return new[] { position, onMeshPosition, sampleTarget };
                }
                
                if (TrySamplePosition(target, filter, _SAMPLE_CURRENT_DISTANCE_MAX, out Vector3 onMeshTarget)) {
                    NavMeshPath navmeshPath = new NavMeshPath();
                    
                    if (NavMesh.CalculatePath(onMeshPosition, onMeshTarget, filter, navmeshPath)) {
                        pathStatus = navmeshPath.status;
                        return new Vector3[] { position, onMeshPosition, onMeshTarget }.AddToArray(navmeshPath.corners);
                    }
                    
                    pathStatus = navmeshPath.status;
                    return new[] { position, onMeshPosition, target };
                }
                
                pathStatus = NavMeshPathStatus.PathInvalid;
                return new[] { position, onMeshPosition, target };
            }
            
            pathStatus = NavMeshPathStatus.PathInvalid;
            return new[] { position, target };
        }
        
        public static bool TrySamplePosition(Vector3 position, float distance, out Vector3 result) {
            return TrySamplePosition(position, CreateFilter(NavMesh.AllAreas), distance, out result);
        }
        
        public static bool TrySamplePosition(Vector3 position, NavMeshQueryFilter filter) {
            return TrySamplePosition(position, filter, SAMPLE_PATH_DISTANCE, out _);
        }
        
        public static bool TrySamplePosition(Vector3 position, NavMeshQueryFilter filter, float distance, out Vector3 result) {
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, distance, filter)) {
                result = hit.position;
                
                return true;
            }
            
            result = default;
            
            return false;
        }
        
        private static NavMeshBuildSettings[] CreateSettings() {
            NavMeshBuildSettings[] result = new NavMeshBuildSettings[NavMesh.GetSettingsCount()];
            
            for (int settingsId = 0; settingsId < result.Length; settingsId++) {
                result[settingsId] = NavMesh.GetSettingsByIndex(settingsId);
            }
            
            return result;
        }
        
        private static int[] ConvertSettingsToIds(NavMeshBuildSettings[] data) {
            int[] result = new int[data.Length];
            
            for (int dataId = 0; dataId < data.Length; dataId++) {
                result[dataId] = data[dataId].agentTypeID;
            }
            
            return result;
        }
    }
}