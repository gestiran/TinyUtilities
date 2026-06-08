using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.EditorInputs {
    [InitializeOnLoad]
    public static class EditorInput {
        public static bool control { get; private set; }
        
        public static readonly bool isActive;
        
        static EditorInput() {
            FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            
            if (info != null) {
                EditorApplication.CallbackFunction globalEventHandler = (EditorApplication.CallbackFunction)info.GetValue(null);
                globalEventHandler += EditorGlobalKeyPress;
                info.SetValue(null, globalEventHandler);
                isActive = true;
            } else {
                isActive = false;
            }
        }
        
        static void EditorGlobalKeyPress() {
            Event current = Event.current;
            
            if (current.keyCode is KeyCode.LeftControl or KeyCode.RightControl) {
                control = current.control;
            }
        }
    }
}