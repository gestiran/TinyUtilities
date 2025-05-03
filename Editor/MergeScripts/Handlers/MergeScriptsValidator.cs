using System.Runtime.CompilerServices;
using TinyUtilities.Editor.MergeScripts.Pairs;
using UnityEngine;

namespace TinyUtilities.Editor.MergeScripts.Handlers {
    public sealed class MergeScriptsValidator {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValid(ChangePair[] pairs) {
            if (pairs == null) {
                Debug.LogError("GUID array is null!");
                return false;
            }
            
            if (pairs.Length == 0) {
                Debug.LogError("GUID array is empty!");
                return false;
            }
            
            for (int pairId = 0; pairId < pairs.Length; pairId++) {
                if (IsValid(pairs[pairId])) {
                    continue;
                }
                
                Debug.LogError($"Invalid pair with id {pairId}!");
                return false;
            }
            
            return true;
        }
        
        private bool IsValid(ChangePair pair) {
            if (pair == null) {
                Debug.LogError("Invalid pair!");
                return false;
            }
            
            switch (pair) {
                case ChangeGUIDPair other:
                    if (IsValid(other)) {
                        return true;
                    }
                    
                    break;
                
                case ChangeNamespacePair other:
                    if (IsValid(other)) {
                        return true;
                    }
                    
                    break;
                
                case ChangeAssemblyPair other:
                    if (IsValid(other)) {
                        return true;
                    }
                    
                    break;
            }
            
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValid(ChangeGUIDPair pair) {
            if (string.IsNullOrEmpty(pair.current)) {
                Debug.LogError("Invalid current GUID!");
                return false;
            }
            
            if (string.IsNullOrEmpty(pair.next)) {
                Debug.LogError("Invalid new GUID!");
                return false;
            }
            
            if (pair.current.Equals(pair.next)) {
                Debug.LogError("Current equal new!");
                return false;
            }
            
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValid(ChangeNamespacePair pair) {
            if (string.IsNullOrEmpty(pair.current)) {
                Debug.LogError("Invalid current Namespace!");
                return false;
            }
            
            if (string.IsNullOrEmpty(pair.next)) {
                Debug.LogError("Invalid new Namespace!");
                return false;
            }
            
            if (pair.current.Equals(pair.next)) {
                Debug.LogError("Current equal new!");
                return false;
            }
            
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValid(ChangeAssemblyPair pair) {
            if (string.IsNullOrEmpty(pair.targetNamespace)) {
                Debug.LogError("Invalid target namespace in Assembly!");
                return false;
            }
            
            if (string.IsNullOrEmpty(pair.current)) {
                Debug.LogError("Invalid current Assembly!");
                return false;
            }
            
            if (string.IsNullOrEmpty(pair.next)) {
                Debug.LogError("Invalid new Assembly!");
                return false;
            }
            
            if (pair.current.Equals(pair.next)) {
                Debug.LogError("Current equal new!");
                return false;
            }
            
            return true;
        }
    }
}