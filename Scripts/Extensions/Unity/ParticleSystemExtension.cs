using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class ParticleSystemExtension {
        public static void Stop(this ParticleSystem[] particles, bool withChildren = true) {
            for (int particleId = 0; particleId < particles.Length; particleId++) {
                particles[particleId].Stop(withChildren);
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
        
        public static void Clear(this ParticleSystem[] particles, bool withChildren = true) {
            for (int particleId = 0; particleId < particles.Length; particleId++) {
                particles[particleId].Clear(withChildren);
            }
        }
    }
}