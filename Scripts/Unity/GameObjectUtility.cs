using UnityEngine;

namespace TinyUtilities.Unity {
    public static class GameObjectUtility {
        public static T[] Instantiate<T>(T prefab, Transform parent, int count) where T : MonoBehaviour {
            T[] result = new T[count];
            
            for (int i = 0; i < count; i++) {
                result[i] = Object.Instantiate(prefab, parent);
            }
            
            return result;
        }
    }
}