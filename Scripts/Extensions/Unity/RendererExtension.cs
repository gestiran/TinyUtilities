using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class RendererExtension {
        public static void SetEnabled<T>(this T[] renderers, bool value) where T : Renderer {
            for (int rendererId = 0; rendererId < renderers.Length; rendererId++) {
                renderers[rendererId].enabled = value;
            }
        }
    }
}