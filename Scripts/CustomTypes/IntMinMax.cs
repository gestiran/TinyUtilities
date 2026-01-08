// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.CustomTypes {
#if ODIN_INSPECTOR
    [InlineProperty]
#endif
    [Serializable]
    public sealed class IntMinMax
#if UNITY_EDITOR && ODIN_VALIDATOR
        : ISelfValidator
#endif
    {
    #if ODIN_INSPECTOR
        [field: HideLabel, SuffixLabel("min", true), HorizontalGroup]
    #endif
        [field: SerializeField]
        public int min { get; private set; }
        
    #if ODIN_INSPECTOR
        [field: HideLabel, SuffixLabel("max", true), HorizontalGroup]
    #endif
        [field: SerializeField]
        public int max { get; private set; }
        
        [Pure]
        public float GetRandom() => UnityRandom.Range(min, max);
        
        [Pure]
        public float Clamp(float value) => Mathf.Clamp(value, min, max);
        
    #if UNITY_EDITOR && ODIN_VALIDATOR
        
        public void Validate(SelfValidationResult result) {
            min = Mathf.Min(min, max);
            max = Mathf.Max(min, max);
        }
        
    #endif
    }
}