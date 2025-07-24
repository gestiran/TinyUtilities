// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities.Validation {
    public static class ValidateMessage {
    #if UNITY_EDITOR
        public static ref SelfValidationResult.ResultItem AddErrorNotCurrent<T>(this SelfValidationResult result) where T : Component {
            return ref result.AddError($"{typeof(T).Name} is not current component!");
        }
    #endif
    }
}