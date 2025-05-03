using UnityEngine.UI;

namespace TinyUtilities.Extensions.Unity {
    public static class ButtonExtension {
        public static void InteractableTrue(this Button[] buttons) => buttons.Interactable(true);
        
        public static void InteractableFalse(this Button[] buttons) => buttons.Interactable(false);
        
        public static void Interactable(this Button[] buttons, bool value) {
            for (int i = 0; i < buttons.Length; i++) {
                buttons[i].interactable = value;
            }
        }
    }
}