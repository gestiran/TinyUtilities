// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using TinyUtilities.Extensions.Unity;
using UnityEngine;

namespace TinyUtilities.CustomTypes {
    [Serializable]
    public sealed class MeshCorners {
        public float xSize => max.x - min.x;
        public float ySize => max.y - min.y;
        public float zSize => max.z - min.z;
        
        public float xMin { get => min.x; set => min.x = value; }
        public float xMax { get => max.x; set => max.x = value; }
        
        public float yMin { get => min.y; set => min.y = value; }
        public float yMax { get => max.y; set => max.y = value; }
        
        public float zMin { get => min.z; set => min.z = value; }
        public float zMax { get => max.z; set => max.z = value; }
        
        public Vector3 min;
        public Vector3 max;
        
        public MeshCorners() { }
        
        public MeshCorners(Vector3 min, Vector3 max) {
            this.min = min;
            this.max = max;
        }
        
        public MeshCorners(float size) {
            min = new Vector3(size, size, size);
            max = new Vector3(size, size, size);
        }
        
        public static MeshCorners operator +(MeshCorners current, Vector3 value) {
            current.min += value;
            current.max += value;
            return current;
        }
        
        public static MeshCorners operator -(MeshCorners current, Vector3 value) {
            current.min -= value;
            current.max -= value;
            return current;
        }
        
        public static MeshCorners operator *(MeshCorners current, Vector3 value) {
            current.min = current.min.MultiplyValues(value);
            current.max = current.max.MultiplyValues(value);
            return current;
        }
        
        public static MeshCorners operator *(MeshCorners current, float value) {
            current.min *= value;
            current.max *= value;
            return current;
        }
        
        public static MeshCorners operator /(MeshCorners current, Vector3 value) {
            current.min = current.min.DivideValues(value);
            current.max = current.max.DivideValues(value);
            return current;
        }
        
        public static MeshCorners operator /(MeshCorners current, float value) {
            current.min /= value;
            current.max /= value;
            return current;
        }
        
        public float SizeMax() => Mathf.Max(xSize, ySize, zSize);
        
        public override string ToString() => $"MeshCorners(xMin: {xMin}, xMin: {xMax}, yMin: {yMin}, yMin: {yMax}, zMin: {zMin}, zMin: {zMax})";
    }
}