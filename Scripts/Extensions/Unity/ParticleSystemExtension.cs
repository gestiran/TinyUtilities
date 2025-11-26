// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class ParticleSystemExtension {
        public static ParticleSystem PlayInstance(this ParticleSystem particle, Vector3 position, Transform parent = null) {
            Transform particleTransform = particle.transform;
            ParticleSystem instance = Object.Instantiate(particle, position + particleTransform.position, particleTransform.rotation, parent);
            instance.Play();
            return instance;
        }
        
        public static ParticleSystem ScaleTo(this ParticleSystem particle, Vector3 localScale) {
            particle.transform.ScaleTo(localScale);
            return particle;
        }
        
        public static ParticleSystem DestroyAfter(this ParticleSystem particle, float seconds) {
            particle.gameObject.DestroyAfter(seconds);
            return particle;
        }
        
        public static void Play<T>(this T particles, bool withChildren = true) where T : IEnumerable<ParticleSystem> {
            foreach (ParticleSystem particle in particles) {
                particle.Play(withChildren);
            }
        }
        
        public static void Stop<T>(this T particles, bool withChildren = true) where T : IEnumerable<ParticleSystem> {
            foreach (ParticleSystem particle in particles) {
                particle.Stop(withChildren);
            }
        }
        
        public static void Clear<T>(this T particles, bool withChildren = true) where T : IEnumerable<ParticleSystem> {
            foreach (ParticleSystem particle in particles) {
                particle.Clear(withChildren);
            }
        }
        
        public static void PlayWhenStop(this ParticleSystem particle, bool withChildren = false) {
            if (particle.isPlaying == false) {
                particle.Play(withChildren);
            }
        }
        
        public static void StopWhenActive(this ParticleSystem particle, bool withChildren = false) {
            if (particle.isPlaying) {
                particle.Stop(withChildren);
            }
        }
    }
}