// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;

namespace TinyUtilities.Extensions.Global {
    public static class BoolExtension {
        [Pure]
        public static string ToText(this bool value) => value ? "True" : "False";
    }
}