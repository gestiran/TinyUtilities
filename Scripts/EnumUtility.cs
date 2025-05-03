using System;

namespace TinyUtilities {
    public static class EnumUtility {
        public static void RunAll<T>(Action<T> action) where T : struct {
            string[] names = Enum.GetNames(typeof(T));
            
            for (int nameId = 0; nameId < names.Length; nameId++) {
                action(Enum.Parse<T>(names[nameId]));
            }
        }
        
        public static (T, int)[] ToArray<T>() where T : struct {
            Type enumType = typeof(T);
            int[] values = (int[])Enum.GetValues(enumType);
            (T, int)[] result = new (T, int)[values.Length];
            
            for (int valueId = 0; valueId < values.Length; valueId++) {
                result[valueId] = ((T)Enum.ToObject(enumType, values[valueId]), values[valueId]);
            }
            
            return result;
        }
        
        public static int Count<T>() where T : struct => Enum.GetNames(typeof(T)).Length;
    }
}