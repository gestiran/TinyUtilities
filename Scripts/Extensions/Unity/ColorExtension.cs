// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class ColorExtension {
        public static string ToHex(this Color color) => $"#{ToByte(color.r):X2}{ToByte(color.g):X2}{ToByte(color.b):X2}";

        private static byte ToByte(float value) => (byte)(Mathf.Clamp01(value) * 255);
    }
}