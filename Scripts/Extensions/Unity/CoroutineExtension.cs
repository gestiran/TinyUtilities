// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class CoroutineExtension {
        public static Coroutine StopCoroutineResult<T>(this T root, Coroutine coroutine) where T : ICoroutineRunner {
            if (coroutine == null) {
                return null;
            }
            
            root.StopCoroutine(coroutine);
            return null;
        }
        
        public static Coroutine RestartCoroutine<T>(this T root, Coroutine coroutine, IEnumerator enumerator) where T : ICoroutineRunner {
            if (coroutine != null) {
                root.StopCoroutine(coroutine);
            }
            
            return root.StartCoroutine(enumerator);
        }
    }
}