using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.GridSnapping {
    internal sealed class GridSnappingPrefs {
        private const string _ENABLE = "GridSnapping_IsEnable";
        
        public void SaveEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_ENABLE}", value);
        }
        
        public bool LoadEnable() {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_ENABLE}", false);
        }
    }
}