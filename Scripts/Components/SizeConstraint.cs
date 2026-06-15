using System.Diagnostics.Contracts;
using TinyUtilities.CustomTypes;
using TinyUtilities.Extensions.Unity;
using UnityEngine;

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public sealed class SizeConstraint : MonoBehaviour {
        [field: SerializeField]
        public bool isActive { get; private set; }
        
        [field: SerializeField]
        public Camera targetCamera { get; private set; }
        
        [field: SerializeField]
        public ResizeData constant { get; private set; }
        
        private void Start() => RecalculateConstant();
        
        private void LateUpdate() {
            if (isActive) {
                float size = targetCamera.CalculateObjectResize(transform.position, constant);
                transform.localScale = new Vector3(size, size, size);
            } 
        }
        
        public void Enable() => isActive = true;
        
        public void Disable() => isActive = false;
        
        public void SetTarget(Camera target) => targetCamera = target;
        
        public void SetConstant(ResizeData data) => constant = data;
        
        public void RecalculateConstant() {
            if (targetCamera != null) {
                SetConstant(CalculateConstant());
            }
        }
        
        [Pure]
        private ResizeData CalculateConstant() {
            float size = transform.localScale.Median();
            float distance = Vector3.Distance(transform.position, targetCamera.transform.position);
            float fov = targetCamera.fieldOfView;
            
            return new ResizeData(size, distance, fov);
        }
    }
}