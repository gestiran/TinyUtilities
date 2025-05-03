using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TinyUtilities.ScreenOrientation {
    public static class ScreenOrientationUtility {
        public static UnityEngine.ScreenOrientation orientation { get; private set; }
        
        public static event Action onChange;
        public static event Action onPortrait;
        public static event Action onLandscape;
        
        static ScreenOrientationUtility() => ListenOrientation();
        
        public static void Apply(Action portrait, Action landscape, Action auto) {
            if (IsPortrait()) {
                portrait.Invoke();
            } else if (IsLandscape()) {
                landscape.Invoke();
            } else {
                auto.Invoke();
            }
        }
        
        public static bool IsAuto() => IsAuto(orientation);
        
        public static bool IsAuto(UnityEngine.ScreenOrientation value) => value is UnityEngine.ScreenOrientation.AutoRotation;
        
        public static bool IsPortrait() => IsPortrait(orientation);
        
        public static bool IsPortrait(UnityEngine.ScreenOrientation value) => value is UnityEngine.ScreenOrientation.Portrait or UnityEngine.ScreenOrientation.PortraitUpsideDown;
        
        public static bool IsLandscape() => IsLandscape(orientation);
        
        public static bool IsLandscape(UnityEngine.ScreenOrientation value) => value is UnityEngine.ScreenOrientation.LandscapeLeft or UnityEngine.ScreenOrientation.LandscapeRight;
        
        private static void OnPortrait() => onPortrait?.Invoke();
        
        private static void OnLandscape() => onLandscape?.Invoke();
        
        private static async void ListenOrientation() {
            orientation = Screen.orientation;
            
            while (Application.isPlaying) {
                if (orientation != Screen.orientation) {
                    await UniTask.Yield();
                    
                    orientation = Screen.orientation;
                    onChange?.Invoke();
                    Apply(OnPortrait, OnLandscape, () => { });
                }
                
                await UniTask.Yield();
            }
        }
    }
}