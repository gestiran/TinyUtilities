﻿// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections;
using UnityEngine;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public sealed class RotationFixed : MonoBehaviour {
        private void OnEnable() => StartCoroutine(RotateProcess(transform));
        
        private void OnDisable() => StopAllCoroutines();
        
        private IEnumerator RotateProcess(Transform current) {
            Quaternion rotation = current.rotation;
            
            while (Application.isPlaying) {
                current.rotation = rotation;
                yield return null;
            }
        }
    }
}