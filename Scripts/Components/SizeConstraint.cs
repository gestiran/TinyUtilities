// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;
using TinyUtilities.CustomTypes;
using TinyUtilities.Extensions;
using UnityEngine;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
#if UNITY_6000_3_OR_NEWER
    [AddComponentMenu("Constraints/Size Constraint")]
#else
    [AddComponentMenu("Miscellaneous/Size Constraint")]
#endif
    public sealed class SizeConstraint : MonoBehaviour {
        [field: SerializeField, InspectorName("Is Active")]
        public bool constraintActive;
        
        [field: SerializeField]
        public Camera source { get; private set; }
        
        [field: SerializeField]
        public ResizeData constant { get; private set; } = new ResizeData(1f, 1f, 30f);
        
        private void LateUpdate() {
            if (constraintActive) {
                float size = source.CalculateObjectResize(transform.position, constant);
                transform.localScale = new Vector3(size, size, size);
            }
        }
        
        public void SetSource(Camera target) => source = target;
        
        public void SetConstant(ResizeData data) => constant = data;
        
        public void RecalculateConstant() {
            if (source != null) {
                SetConstant(CalculateConstant());
            }
        }
        
        [Pure]
        private ResizeData CalculateConstant() {
            float size = transform.localScale.Median();
            float distance = Vector3.Distance(transform.position, source.transform.position);
            float fov = source.fieldOfView;
            
            return new ResizeData(size, distance, fov);
        }
    }
}