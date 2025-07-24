// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

namespace TinyUtilities.Extensions.Unity {
    public static class BoolExtension {
        public static string ToText(this bool value) => value ? "True" : "False";
    }
}