// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine.UI;

namespace TinyUtilities.Extensions.Unity {
    public static class ButtonExtension {
        public static void InteractableTrue<T>(this T buttons) where T : IEnumerable<Button> => buttons.Interactable(true);
        
        public static void InteractableFalse<T>(this T buttons) where T : IEnumerable<Button> => buttons.Interactable(false);
        
        public static T Interactable<T>(this T buttons, bool value) where T : IEnumerable<Button> {
            foreach (Button button in buttons) {
                button.interactable = value;
            }
            
            return buttons;
        }
    }
}