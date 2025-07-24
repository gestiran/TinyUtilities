// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public sealed class ColorRendererAnimation : MonoBehaviour, ISelfValidator {
        [field: SerializeField, HideLabel, HorizontalGroup("H1")]
        public Color from { get; private set; } = Color.white;
        
        [field: SerializeField, HideLabel, HorizontalGroup("H1")]
        public Color to { get; private set; } = Color.white;
        
        [field: SerializeField]
        public float duration { get; private set; } = 1f;
        
        [field: SerializeField]
        public bool ignoreTimeScale { get; private set; }
        
        [field: SerializeField]
        public bool isLoop { get; private set; }
        
        [field: SerializeField, BoxGroup("Loop"), HorizontalGroup("Loop/H1"), HideLabel, SuffixLabel("loops", true), ShowIf(nameof(isLoop))]
        public int loops { get; private set; } = int.MaxValue;
        
        [field: SerializeField, HorizontalGroup("Loop/H1"), HideLabel, ShowIf(nameof(isLoop))]
        public LoopType loopType { get; private set; }
        
        [SerializeField, FoldoutGroup(InspectorNames.GENERATED), ReadOnly, Required]
        private Renderer _thisRenderer;
        
        private bool _isInit;
        private Material _material;
        private int _colorId;
        
        public void Init() {
            if (_isInit == false) {
                InitProcess();
                _isInit = true;
            }
        }
        
        public void Play() {
            Init();
            
            if (isLoop) {
                this.DOColor(to, duration).From(from).SetLoops(loops, loopType).SetEase(Ease.Linear).SetUpdate(ignoreTimeScale);
            } else {
                this.DOColor(to, duration).From(from).SetEase(Ease.Linear).SetUpdate(ignoreTimeScale);
            }
        }
        
        public void Kill(bool complete = false) {
            Init();
            this.DOKill(complete);
        }
        
        public Color GetColor() {
        #if UNITY_EDITOR
            
            if (_material == null) {
                Debug.LogError("ColorRendererAnimation.SetColor - Can't use without init!");
                return Color.white;
            }
            
        #endif
            
            return _material.GetColor(_colorId);
        }
        
        public void SetColor(Color color) {
        #if UNITY_EDITOR
            
            if (_material == null) {
                Debug.LogError("ColorRendererAnimation.SetColor - Can't use without init!");
                return;
            }
            
        #endif
            
            _material.SetColor(_colorId, color);
        }
        
        private void InitProcess() {
            _colorId = Shader.PropertyToID("_Color");
            _material = _thisRenderer.material;
            _thisRenderer.sharedMaterial = _material;
        }
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR
            duration = Mathf.Max(0.01f, duration);
            
            if (_thisRenderer != null) {
                if (_thisRenderer.sharedMaterial == null) {
                    result.AddError("Can't find material!");
                } else {
                    Shader targetShader = Shader.Find("Mobile/Color");
                    
                    if (_thisRenderer.sharedMaterial.shader.Equals(targetShader) == false) {
                        result.AddError("Invalid shader!").WithFix(() => ApplyShader(targetShader));
                    }
                }
            }
        #endif
        }
        
    #if UNITY_EDITOR
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        private void Reset() {
            _thisRenderer = GetComponent<Renderer>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        private void ApplyShader(Shader shader) {
            _thisRenderer.sharedMaterial.shader = shader;
            UnityEditor.EditorUtility.SetDirty(_thisRenderer.sharedMaterial);
        }
        
    #endif
    }
}