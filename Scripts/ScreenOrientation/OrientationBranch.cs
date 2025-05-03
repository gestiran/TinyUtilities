using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities.ScreenOrientation {
    public sealed class OrientationBranch : MonoBehaviour {
        [field: SerializeField, Required]
        public GameObject portrait { get; private set; }
        
        [field: SerializeField, Required]
        public GameObject landscape { get; private set; }
        
        private void Start() {
            ScreenOrientationUtility.Apply(ToPortrait, ToLandscape, UpdateState);
            
            ScreenOrientationUtility.onPortrait += ToPortrait;
            ScreenOrientationUtility.onLandscape += ToLandscape;
        }
        
        private void OnDestroy() {
            ScreenOrientationUtility.onPortrait -= ToPortrait;
            ScreenOrientationUtility.onLandscape -= ToLandscape;
        }
        
        private void UpdateState() {
            if (ScreenOrientationUtility.IsPortrait()) {
                ToPortrait();
            } else {
                ToLandscape();
            }
        }
        
        private void ToPortrait() {
            portrait.SetActive(true);
            landscape.SetActive(false);
        }
        
        private void ToLandscape() {
            portrait.SetActive(false);
            landscape.SetActive(true);
        }
    }
}