#if !ODIN_INSPECTOR
    using UnityEngine;
    
    namespace TinyUtilities.Editor {
        public abstract class OdinEditorWindow {
            protected static T GetWindow<T>(string title) where T : OdinEditorWindow, new() {
                return new T();
            }
            
            protected void Show() { }
            
            protected void Repaint() { }
            
            protected void DestroyImmediate(GameObject gameObject) { }
        }
        
        public abstract class OdinValueDrawer<T> {
            public Entry ValueEntry;
            
            public sealed class Entry {
                public T SmartValue;
            }
            
            protected virtual void Initialize() { }
            
            protected virtual void DrawPropertyLayout(GUIContent label) { }
        }
    }
#endif