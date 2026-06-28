// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public sealed class DontDestroyOnLoad : MonoBehaviour {
        private void Start() => DontDestroyOnLoad(gameObject);
    }
}