using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TinyUtilities.Editor.MergeScripts.Handlers;
using TinyUtilities.Editor.MergeScripts.Pairs;
using TinyUtilities.Extensions.Global;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.MergeScripts {
    public sealed class MergeScriptsWindow : OdinEditorWindow {
        [DisableIf("IsActive")]
        [ShowInInspector, ListDrawerSettings(NumberOfItemsPerPage = 5), ValueDropdown("PairsDropdown", CopyValues = true, DrawDropdownForListElements = false)]
        private ChangePair[] _pairs;
        
        [ShowIf("IsActive")]
        [ShowInInspector, ProgressBar(0, 100, MaxGetter = nameof(_maxProgress)), ReadOnly, HideLabel]
        private int _currentProgress;
        
        private int _maxProgress;
        private MergeScriptsHandler _handler;
        private MergeScriptsValidator _validator;
        private CancellationTokenSource _cancellation;
        
        [MenuItem("Window/TinyUtilities/Merge Scripts", priority = 0)]
        private static void OpenWindow() => GetWindow<MergeScriptsWindow>("Merge Scripts").Show();
        
        [OnInspectorInit]
        private void Init() {
            _pairs = Array.Empty<ChangePair>();
            _handler = new MergeScriptsHandler();
            _validator = new MergeScriptsValidator();
        }
        
        [DisableIf("IsActive")]
        [Button, HorizontalGroup("Control")]
        private void Start() {
            if (_validator.IsValid(_pairs) == false) {
                return;
            }
            
            Stop();
            
            _cancellation = new CancellationTokenSource();
            _handler.ChangeProcess(_pairs, DisplayProgress, OnComplete, _cancellation.Token).Forget();
        }
        
        [EnableIf("IsActive")]
        [Button, HorizontalGroup("Control")]
        private void Stop() => _cancellation = _cancellation.Reset();
        
        private void DisplayProgress(int current, int max) {
            _currentProgress = current;
            _maxProgress = max;
            
            Repaint();
        }
        
        private void OnComplete() {
            Stop();
            Repaint();
            
            Debug.LogError("Merge Scripts - Complete success!");
        }
        
        private ValueDropdownList<ChangePair> PairsDropdown() {
            return new ValueDropdownList<ChangePair>() {
                new ValueDropdownItem<ChangePair>("Change GUID", new ChangeGUIDPair()),
                new ValueDropdownItem<ChangePair>("Change Namespace", new ChangeNamespacePair()),
                new ValueDropdownItem<ChangePair>("Change Assembly", new ChangeAssemblyPair()),
            };
        }
        
        private bool IsActive() => _cancellation != null;
    }
}