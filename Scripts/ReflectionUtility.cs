using System;
using System.Linq;
using System.Reflection;
using UnityObject = UnityEngine.Object;

namespace TinyUtilities {
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
        #if UNITY_EDITOR
            if (obj is UnityObject unityObject) {
                UnityEditor.EditorUtility.SetDirty(unityObject);
            }
        #endif
        }
        
        public static void SetProperty<T1, T2>(this T1 obj, string fieldName, T2 value, UnityObject source) {
            PropertyInfo property = typeof(T1).GetProperty(fieldName, _flags);
            
            if (property != null) {
                property.SetValue(obj, value);
            }
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(source);
        #endif
        }
        
        public static void SetField<T1, T2>(this T1 obj, string fieldName, T2 value) {
            FieldInfo filed = typeof(T1).GetField(fieldName, _flags);
            
            if (filed != null) {
                filed.SetValue(obj, value);
            }
        #if UNITY_EDITOR
            if (obj is UnityObject unityObject) {
                UnityEditor.EditorUtility.SetDirty(unityObject);
            }
        #endif
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
        
        public static Type[] GetSubTypes<T>() {
            Type target = typeof(T);
            return Assembly.GetAssembly(target).GetTypes().Where(type => type.IsClass && type.IsAbstract == false && target.IsAssignableFrom(type)).ToArray();
        }
    }
}