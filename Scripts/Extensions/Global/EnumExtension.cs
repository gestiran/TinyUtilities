using System;

namespace TinyUtilities.Extensions.Global {
    public static class EnumExtension {
        public static bool Is<T>(this T value, T flag) where T : Enum {
            ushort valueData = Convert.ToUInt16(value);
            ushort flagData = Convert.ToUInt16(flag);
            return (valueData & flagData) != 0;
        }
        
        public static bool IsNot<T>(this T value, T flag) where T : Enum {
            ushort valueData = Convert.ToUInt16(value);
            ushort flagData = Convert.ToUInt16(flag);
            return (valueData & flagData) == 0;
        }
        
        [Obsolete("Can`t use without parameters!", true)]
        public static bool Any<T>(this T value) where T : Enum => false;
        
        public static bool Any<T>(this T value, params T[] flags) where T : Enum {
            ushort valueData = Convert.ToUInt16(value);
            
            foreach (T flag in flags) {
                ushort flagData = Convert.ToUInt16(flag);
                
                if ((valueData & flagData) != 0) {
                    return true;
                }
            }
            
            return false;
        }
    }
}