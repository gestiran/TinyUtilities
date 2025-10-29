// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public sealed class ParticleSystemRoot : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly, Required]
        private ParticleSystem[] _particles;
        
        public void Play() {
            for (int i = 0; i < _particles.Length; i++) {
                _particles[i].Play();
            }
        }
        
        public void PlayOnAwake(bool isPlay) {
            for (int i = 0; i < _particles.Length; i++) {
                ParticleSystem.MainModule main = _particles[i].main;
                main.playOnAwake = isPlay;
            }
        }
        
    #if UNITY_EDITOR
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        private void Reset() {
            _particles = GetComponentsInChildren<ParticleSystem>(true);
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}