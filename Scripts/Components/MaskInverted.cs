using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TinyUtilities.Components {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Mask (Inverted)", 13)]
    public sealed class MaskInverted : UIBehaviour, ICanvasRaycastFilter, IMaterialModifier {
        [NonSerialized]
        private RectTransform m_RectTransform;
        
        public RectTransform rectTransform => m_RectTransform ?? (m_RectTransform = GetComponent<RectTransform>());
        
        [SerializeField]
        private bool m_ShowMaskGraphic = true;
        
        public bool showMaskGraphic {
            get => m_ShowMaskGraphic;
            set {
                if (m_ShowMaskGraphic == value) {
                    return;
                }
                
                m_ShowMaskGraphic = value;
                
                if (graphic != null) {
                    graphic.SetMaterialDirty();
                }
            }
        }
        
        [NonSerialized]
        private Graphic m_Graphic;
        
        [NonSerialized]
        private Graphic[] m_SubGraphic;
        
        public Graphic graphic => m_Graphic ?? (m_Graphic = GetComponent<Graphic>());
        
        [NonSerialized]
        private Material m_MaskMaterial;
        
        [SerializeField]
        private Material m_UnmaskMaterial;
        
        public bool MaskEnabled() => IsActive() && graphic != null;
        
        [Obsolete("Not used anymore.")]
        public void OnSiblingGraphicEnabledDisabled() { }
        
        protected override void OnEnable() {
            base.OnEnable();
            
            if (graphic != null) {
                graphic.canvasRenderer.hasPopInstruction = true;
                graphic.SetMaterialDirty();
                
                if (graphic is MaskableGraphic maskableGraphic) {
                    maskableGraphic.isMaskingGraphic = true;
                }
            }
            
            UpdateSubGraphics(m_UnmaskMaterial);
        }
        
        protected override void OnDisable() {
            base.OnDisable();
            
            if (graphic != null) {
                graphic.SetMaterialDirty();
                graphic.canvasRenderer.hasPopInstruction = false;
                graphic.canvasRenderer.popMaterialCount = 0;
                
                if (graphic is MaskableGraphic maskableGraphic) {
                    maskableGraphic.isMaskingGraphic = false;
                }
            }
            
            UpdateSubGraphics(null);
            
            StencilMaterial.Remove(m_MaskMaterial);
            m_MaskMaterial = null;
        }
        
    #if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            
            if (!IsActive()) {
                return;
            }
            
            if (graphic != null) {
                if (graphic is MaskableGraphic maskableGraphic) {
                    maskableGraphic.isMaskingGraphic = true;
                }
                
                graphic.SetMaterialDirty();
            }
            
            UpdateSubGraphics(m_UnmaskMaterial);
        }
        
    #endif
        
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
            if (!isActiveAndEnabled) {
                return true;
            }
            
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, sp, eventCamera);
        }
        
        public Material GetModifiedMaterial(Material baseMaterial) {
            if (!MaskEnabled()) {
                return baseMaterial;
            }
            
            Transform rootSortCanvas = MaskUtilities.FindRootSortOverrideCanvas(transform);
            int stencilDepth = MaskUtilities.GetStencilDepth(transform, rootSortCanvas);
            
            if (stencilDepth >= 8) {
                Debug.LogWarning("Attempting to use a stencil mask with depth > 8", gameObject);
                return baseMaterial;
            }
            
            ColorWriteMask color = m_ShowMaskGraphic ? ColorWriteMask.All : 0;
            
            Material maskMaterial = StencilMaterial.Add(baseMaterial, 1, StencilOp.Replace, CompareFunction.Always, color);
            StencilMaterial.Remove(m_MaskMaterial);
            m_MaskMaterial = maskMaterial;
            
            return m_MaskMaterial;
        }
        
        private void UpdateSubGraphics(Material material) {
            List<MaskableGraphic> children = ListPool<MaskableGraphic>.Get();
            GetComponentsInChildren(children);
            
            int instanceId = gameObject.GetInstanceID();
            
            for (int childId = 0; childId < children.Count; childId++) {
                if (children[childId] == null) {
                    continue;
                }
                
                if (children[childId].gameObject.GetInstanceID() == instanceId) {
                    continue;
                }
                
                children[childId].material = material;
            }
            
            ListPool<MaskableGraphic>.Release(children);
        }
    }
}