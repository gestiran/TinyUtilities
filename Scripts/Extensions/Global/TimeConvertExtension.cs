// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

namespace TinyUtilities.Extensions.Global {
    public static class TimeConvertExtension {
        public static int ToMS(this float seconds) => (int)(seconds * 1000f);
        
        public static float ToSec(this int milliseconds) => milliseconds * 0.001f;
        
        public static float ToSec(this double milliseconds) => (float)(milliseconds * 0.001);
    }
}