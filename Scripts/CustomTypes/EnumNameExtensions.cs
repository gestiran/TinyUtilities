using System;

namespace TinyUtilities.CustomTypes {
    public static class EnumNameExtensions {
        public static bool IsContains<T>(this EnumName<T>[] enums, T value) where T : Enum {
            for (int i = 0; i < enums.Length; i++) {
                if (Equals(enums[i].value, value)) {
                    return true;
                }
            }
            
            return false;
        }
    }
}