// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TinyUtilities.Unity {
    public class TrackedScrollRect : ScrollRect {
        public bool IsDragging { get; private set; }
        
        public override void OnBeginDrag(PointerEventData eventData) {
            base.OnBeginDrag(eventData);
            IsDragging = true;
        }
        
        public override void OnEndDrag(PointerEventData eventData) {
            base.OnEndDrag(eventData);
            IsDragging = false;
        }
    }
}