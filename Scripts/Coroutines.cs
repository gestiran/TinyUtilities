// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities {
    public static class Coroutines {
        public static IEnumerator ActionAfterDelay(float delay, Action action) {
            yield return new WaitForSeconds(delay);
            action();
        }
        
        public static IEnumerator ActionAfterDelayRealtime(float delay, Action action) {
            yield return new WaitForSecondsRealtime(delay);
            action();
        }
        
        public static IEnumerator ActionAfterFrame(Action action) {
            yield return new WaitForEndOfFrame();
            action();
        }
        
        public static IEnumerator WaitAll(List<Coroutine> coroutines) {
            for (int coroutineId = 0; coroutineId < coroutines.Count; coroutineId++) {
                yield return coroutines[coroutineId];
            }
        }
    }
}