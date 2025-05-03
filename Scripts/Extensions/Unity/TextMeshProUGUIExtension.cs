using TMPro;

namespace TinyUtilities.Extensions.Unity {
    public static class TextMeshProUGUIExtension {
        public static void SetText(this TextMeshProUGUI[] objects, string sourceText) {
            for (int i = 0; i < objects.Length; i++) {
                objects[i].SetText(sourceText);
            }
        }
    }
}