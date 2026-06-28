// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

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