using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class ParticleSystemExtension {
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