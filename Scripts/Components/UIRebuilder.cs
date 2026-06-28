// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIRebuilder : MonoBehaviour {
        public void ForceRebuild() => LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        
        public void MarkRebuild() => LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }
}