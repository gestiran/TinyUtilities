// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class RendererExtension {
        public static T[] SetEnabled<T>(this T[] renderers, bool value) where T : Renderer {
            foreach (T renderer in renderers) {
                renderer.enabled = value;
            }
            
            return renderers;
        }
        
        public static List<T> SetEnabled<T>(this List<T> renderers, bool value) where T : Renderer {
            foreach (T renderer in renderers) {
                renderer.enabled = value;
            }
            
            return renderers;
        }
        
        public static T[] SetMaterial<T>(this T[] renderers, Material value) where T : Renderer {
            foreach (T renderer in renderers) {
                renderer.material = value;
            }
            
            return renderers;
        }
        
        public static List<T> SetMaterial<T>(this List<T> renderers, Material value) where T : Renderer {
            foreach (T renderer in renderers) {
                renderer.material = value;
            }
            
            return renderers;
        }
    }
}