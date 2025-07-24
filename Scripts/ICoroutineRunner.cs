// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections;
using UnityEngine;

namespace TinyUtilities {
    public interface ICoroutineRunner {
        public Coroutine StartCoroutine(IEnumerator enumerator);
        
        public void StopCoroutine(Coroutine coroutine);
    }
}