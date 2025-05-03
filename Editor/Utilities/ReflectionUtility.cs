using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.Utilities {
    public static class ReflectionUtility {
        private static readonly BindingFlags _flags;

        static ReflectionUtility() {
            _flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        }
        
        public static void SetProperty<T1, T2>(this T1 obj, string fieldName, T2 value) {
            PropertyInfo property = typeof(T1).GetProperty(fieldName, _flags);
            
            if (property != null) {
                property.SetValue(obj, value);
            }

            if (obj is Object unityObject) {
                EditorUtility.SetDirty(unityObject);
            }
        }
        
        public static void SetField<T1, T2>(this T1 obj, string fieldName, T2 value) {
            FieldInfo filed = typeof(T1).GetField(fieldName, _flags);

            if (filed != null) {
                filed.SetValue(obj, value);   
            }

            if (obj is Object unityObject) {
                EditorUtility.SetDirty(unityObject);
            }
        }
        
        public static void GetProperty<T1, T2>(this T1 obj, string fieldName, out T2 result) {
            PropertyInfo property = typeof(T1).GetProperty(fieldName, _flags);

            if (property == null) {
                result = default;
                return;
            }
            
            object value = property.GetValue(obj);

            if (value is T2 valueType) {
                result = valueType;
                return;
            }

            result = default;
        }
        
        public static void GetField<T1, T2>(this T1 obj, string fieldName, out T2 result) {
            FieldInfo filed = typeof(T1).GetField(fieldName, _flags);

            if (filed == null) {
                result = default;
                return;
            }
            
            object value = filed.GetValue(obj);

            if (value is T2 valueType) {
                result = valueType;
                return;
            }

            result = default;
        }
    }
}