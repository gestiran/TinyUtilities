using UnityEngine;

namespace TinyUtilities.Components {
    public sealed class ScrollButtonMoveNext : ScrollButtonMove {
        protected override void OnClick() => _scroll.MoveToElement(_scroll.currentElement + 1);
        
        protected override bool IsActive() => _scroll.currentElement < _scroll.elementsCount - 1;
        
    #if UNITY_EDITOR
        [ContextMenu(InspectorNames.SOFT_RESET)]
        protected override void Reset() => base.Reset();
    #endif
    }
}