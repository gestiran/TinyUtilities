using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;     
#endif

namespace TinyUtilities.CustomTypes {
#if ODIN_INSPECTOR
    [InlineProperty]
#endif
    [Serializable]
    public sealed class ResizeData {
    #if ODIN_INSPECTOR
        [field: HorizontalGroup, HideLabel, SuffixLabel("Size", true)]
    #endif
        [field: SerializeField]
        public float size { get; private set; }
        
    #if ODIN_INSPECTOR
        [field: HorizontalGroup, HideLabel, SuffixLabel("Distance", true)]
    #endif
        [field: SerializeField]
        public float distance { get; private set; }
        
    #if ODIN_INSPECTOR
        [field: HorizontalGroup, HideLabel, SuffixLabel("FoV", true)]
    #endif
        [field: SerializeField]
        public float fov { get; private set; }
        
        public ResizeData(float size, float distance, float fov) {
            this.size = size;
            this.distance = distance;
            this.fov = fov;
        }
    }
}