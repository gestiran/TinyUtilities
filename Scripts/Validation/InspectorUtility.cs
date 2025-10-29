// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

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