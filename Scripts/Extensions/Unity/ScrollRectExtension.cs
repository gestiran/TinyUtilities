using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Extensions.Unity {
    public static class ScrollRectExtension {
        public static void MoveToElementVertical(this ScrollRect scroll, int id, float cellSize, float offset = 0) {
            RectTransform content = scroll.content;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, offset + cellSize * id);
        }
    }
}