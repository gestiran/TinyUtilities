// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Threading.Tasks;
using UnityEngine;

namespace TinyUtilities.Components {
    public sealed class ADSTimeScaleFix : MonoBehaviour {
        private float _timeScale;
        
        private void Awake() => _timeScale = Time.timeScale;
        
        private void OnDestroy() => ReturnTimeScaleProcess();
        
        private async void ReturnTimeScaleProcess() {
            await Task.Yield();
            Time.timeScale = _timeScale;
        }
    }
}