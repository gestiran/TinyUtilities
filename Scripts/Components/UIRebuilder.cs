using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIRebuilder : MonoBehaviour {
        public void ForceRebuild() => LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        
        public void MarkRebuild() => LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }
}