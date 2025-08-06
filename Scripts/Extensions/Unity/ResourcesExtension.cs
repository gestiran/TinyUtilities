using System;
using UnityObject = UnityEngine.Object;

namespace TinyUtilities.Extensions.Unity {
    public static class ResourcesExtension {
        public static T LoadWhenNull<T>(this T current, Func<T> load) where T : UnityObject {
            if (current == null) {
                return load.Invoke();
            }
            
            return current;
        }
    }
}