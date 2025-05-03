using UnityEngine;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public sealed class DontDestroyOnLoad : MonoBehaviour {
        private void Start() => DontDestroyOnLoad(gameObject);
    }
}