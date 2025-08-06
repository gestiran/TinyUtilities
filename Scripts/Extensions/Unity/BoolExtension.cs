// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using JetBrains.Annotations;

namespace TinyUtilities.Extensions.Unity {
    public static class BoolExtension {
        [Pure]
        public static string ToText(this bool value) => value ? "True" : "False";
    }
}