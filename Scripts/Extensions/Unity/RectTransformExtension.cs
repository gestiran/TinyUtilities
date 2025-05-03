using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class RectTransformExtension {
        public static float CalculateHeight(this RectTransform transform) {
            if (transform == null) {
                return 1;
            }
            
            return transform.sizeDelta.y;
        }
        
        public static float CalculateHeight<T>(this T obj) where T : MonoBehaviour => obj.GetComponent<RectTransform>().CalculateHeight();
    }
}