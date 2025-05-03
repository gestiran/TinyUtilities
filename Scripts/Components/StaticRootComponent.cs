using UnityEngine;

namespace TinyUtilities.Components {
    public sealed class StaticRootComponent : MonoBehaviour {
        private void Start() {
            StaticBatchingUtility.Combine(GetChildObjects(),gameObject);
        }
        
        private GameObject[] GetChildObjects() {
            MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
            GameObject[] objects = new GameObject[filters.Length];
            
            for (int obj = 0; obj < objects.Length; obj++) {
                objects[obj] = filters[obj].gameObject;
            }
            
            return objects;
        }
    }
}