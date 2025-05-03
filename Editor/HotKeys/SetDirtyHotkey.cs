using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.HotKeys {
    public static class SetDirtyHotkey {
        [MenuItem("Assets/Set Dirty", false, 18)]
        public static void ForceReloading() {
            Object[] objects = Selection.objects;
            
            for (int objId = 0; objId < objects.Length; objId++) {
                EditorUtility.SetDirty(objects[objId]);
                AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(objects[objId])));
            }
        }
    }
}