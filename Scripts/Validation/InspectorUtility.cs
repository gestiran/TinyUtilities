using System;
using Sirenix.OdinInspector;

namespace TinyUtilities.Validation {
#if UNITY_EDITOR
    public static class InspectorUtility {
        public static ValueDropdownList<string> AllTypes<T>() {
            ValueDropdownList<string> result = new ValueDropdownList<string>();
            Type[] allModes = ReflectionUtility.GetSubTypes<T>();
            
            for (int modeId = 0; modeId < allModes.Length; modeId++) {
                result.Add(allModes[modeId].Name, allModes[modeId].Name);
            }
            
            return result;
        }
    }
#endif
}