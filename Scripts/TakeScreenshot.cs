// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities {
    public class TakeScreenshot : MonoBehaviour {
        private void Update() {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
                ScreenCapture.CaptureScreenshot($"D:/PROJECTS/Screenshots/screen{Random.Range(0, 9999999):0000000}.png", 4);
            }
        }
    }
}