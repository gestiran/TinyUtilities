using System;

namespace TinyUtilities.Extensions.Global {
    public static class IntExtension {
        public static bool IsAny(this int value, params int[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                if (value == arr[i]) {
                    return true;
                }
            }
            
            return false;
        }
        
        [Obsolete("Can't use without parameters!", true)]
        public static bool IsAny(this int value) => false;
    }
}