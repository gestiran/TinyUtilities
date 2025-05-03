using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinyUtilities.Editor.MergeScripts.Pairs;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.MergeScripts.Handlers {
    public sealed class MergeScriptsHandler {
        public async UniTask ChangeProcess(ChangePair[] pairs, Action<int, int> progress, Action onComplete, CancellationToken cancellation) {
            string[] paths = GetAllPaths("t:prefab");
            await ChangeProcess(paths, pairs, progress, cancellation);
            
            paths = GetAllPaths("t:scene");
            await ChangeProcess(paths, pairs, progress, cancellation);
            
            onComplete.Invoke();
        }
        
        private async UniTask ChangeProcess(string[] paths, ChangePair[] pairs, Action<int, int> progress, CancellationToken cancellation) {
            for (int pathId = 0; pathId < paths.Length; pathId++) {
                string text = await File.ReadAllTextAsync(paths[pathId], cancellation);
                
                
                for (int pairId = 0; pairId < pairs.Length; pairId++) {
                    if (TryChange(text, pairs[pairId], out string result)) {
                        await File.WriteAllTextAsync(paths[pathId], result, cancellation);
                    }
                }
                
                progress.Invoke(pathId, paths.Length);
            }
            
            progress.Invoke(paths.Length, paths.Length);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryChange(string text, ChangePair pair, out string result) {
            switch (pair) {
                case ChangeGUIDPair other:
                    if (TryChange(text, other, out result)) {
                        return true;
                    }
                    
                    break;
                
                case ChangeNamespacePair other:
                    if (TryChange(text, other, out result)) {
                        return true;
                    }
                    
                    break;
                
                case ChangeAssemblyPair other:
                    if (TryChange(text, other, out result)) {
                        return true;
                    }
                    
                    break;
            }
            
            result = text;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryChange(string text, ChangeGUIDPair pair, out string result) {
            string current = $"guid: {pair.current},";
            
            if (text.Contains(current)) {
                result = text.Replace(current, $"guid: {pair.next}");
                return true;
            }
            
            result = text;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryChange(string text, ChangeNamespacePair pair, out string result) {
            string current = $"ns: {pair.current},";
            
            if (text.Contains(current)) {
                result = text.Replace(current, $"ns: {pair.next},");
                return true;
            }
            
            result = text;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryChange(string text, ChangeAssemblyPair pair, out string result) {
            string current = "ns: " + pair.targetNamespace + ", asm: " + pair.current + "}";
            
            if (text.Contains(current)) {
                result = text.Replace(current, "ns: " + pair.targetNamespace + ", asm: " + pair.next + "}");
                return true;
            }
            
            result = text;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string[] GetAllPaths(string filter) {
            string dataPath = GetDataPath();
            string[] assets = AssetDatabase.FindAssets(filter);
            
            for (int assetId = 0; assetId < assets.Length; assetId++) {
                assets[assetId] = Path.Combine(dataPath, AssetDatabase.GUIDToAssetPath(assets[assetId]));
            }
            
            return assets;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDataPath() => Path.GetDirectoryName(Application.dataPath);
    }
}