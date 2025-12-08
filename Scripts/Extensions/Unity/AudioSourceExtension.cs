// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class AudioSourceExtension {
        public static AudioSource SetPitch(this AudioSource source, float pitch) {
            source.pitch = pitch;
            return source;
        }
        
        public static AudioSource SetVolume(this AudioSource source, float volume) {
            source.volume = volume;
            return source;
        }
    }
}