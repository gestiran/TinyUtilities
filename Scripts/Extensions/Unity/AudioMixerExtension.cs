using UnityEngine.Audio;

namespace TinyUtilities.Extensions.Unity {
    public static class AudioMixerExtension {
        public static void TransitionToSnapshots(this AudioMixer mixer, string snapshotName, float weight, float timeToReach) {
            AudioMixerSnapshot snapshot = mixer.FindSnapshot(snapshotName);
            mixer.TransitionToSnapshots(new []{snapshot}, new float[]{weight}, timeToReach);
        }
    }
}