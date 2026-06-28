// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections;
using TinyUtilities.Extensions;
using UnityEngine;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public sealed class ScaleFixed : MonoBehaviour {
        private void OnEnable() => StartCoroutine(RescaleProcess(transform));
        
        private void OnDisable() => StopAllCoroutines();
        
        private IEnumerator RescaleProcess(Transform current) {
            Vector3 lossyScale = current.lossyScale;
            
            while (Application.isPlaying) {
                current.SetLossyScale(lossyScale);
                yield return null;
            }
        }
    }
}