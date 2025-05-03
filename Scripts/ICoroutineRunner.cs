using System.Collections;
using UnityEngine;

namespace TinyUtilities {
    public interface ICoroutineRunner {
        public Coroutine StartCoroutine(IEnumerator enumerator);
        
        public void StopCoroutine(Coroutine coroutine);
    }
}