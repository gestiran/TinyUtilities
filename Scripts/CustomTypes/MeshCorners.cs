// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;

namespace TinyUtilities.CustomTypes {
    [Serializable]
    public sealed class MeshCorners {
        public float xMin;
        public float xMax;
        
        public float yMin;
        public float yMax;
        
        public float zMin;
        public float zMax;
    }
}