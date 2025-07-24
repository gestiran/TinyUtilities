// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
#endif

namespace TinyUtilities.Vibration {
    /// <summary> A VibrationEffect describes a haptic effect to be performed by a Vibrator.
    /// These effects may be any number of things, from single shot vibrations to complex waveforms. </summary>
    public static class VibrationEffect {
    #if UNITY_ANDROID && !UNITY_EDITOR
        public static bool hasAmplitudeControl => _hasAmplitudeControl;
        public static bool isSupportVibrationEffect => _isSupportVibrationEffect;
        
        private static readonly AndroidJavaObject _vibrator;
        private static readonly AndroidJavaClass _vibrationEffect;
        private static readonly bool _isSupportVibrationEffect;
        private static readonly bool _hasAmplitudeControl;
        private static readonly int _defaultAmplitude;
        private static readonly int _androidVersion;
        
        private const string _CREATE_ONE_SHOT_METHOD = "createOneShot";
        private const string _CREATE_WAVEFORM_METHOD = "createWaveform";
        private const string _VIBRATE_METHOD = "vibrate";
        private const string _CANCEL_METHOD = "cancel";
    #endif
        
        static VibrationEffect() {
        #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            
            _vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            
            _isSupportVibrationEffect = _androidVersion >= 26;
            
            if (_isSupportVibrationEffect) {
                _vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
                _defaultAmplitude = Mathf.Clamp(_vibrationEffect.GetStatic<int>("DEFAULT_AMPLITUDE"), 1, 255);
                _hasAmplitudeControl = _vibrator.Call<bool>("hasAmplitudeControl");
            }
            
            if (Application.platform == RuntimePlatform.Android) {
                string androidVersion = SystemInfo.operatingSystem;
                int sdkPos = androidVersion.IndexOf("API-", StringComparison.Ordinal);
                _androidVersion = int.Parse(androidVersion.Substring(sdkPos + 4, 2));
            } else {
                _androidVersion = 0;
            }
            
            if (_androidVersion < 0) {
                Handheld.Vibrate(); // Auto add VIBRATION permission  
            }
        #endif
        }
        
        /// <summary> Create a one shot vibration </summary>
        /// <param name="milliseconds"> The number of milliseconds to vibrate. This must be a positive number </param>
        public static void CreateOneShot(long milliseconds) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            CreateOneShot(milliseconds, _defaultAmplitude);
        #endif
        }
        
        /// <summary> Create a one shot vibration </summary>
        /// <param name="milliseconds"> The number of milliseconds to vibrate. This must be a positive number </param>
        /// /// <param name="amplitude"> The amplitude values of the timing / amplitude pairs. Amplitude values must be between 0 and 255, or equal to DEFAULT_AMPLITUDE.
        /// An amplitude value of 0 implies the motor is off </param>
        public static void CreateOneShot(long milliseconds, int amplitude) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            Cancel();
            
            milliseconds = Math.Max(milliseconds, 1);
            amplitude = Mathf.Clamp(amplitude, 1, 255);
            
            if (_isSupportVibrationEffect) {
                if (_hasAmplitudeControl) {
                    using AndroidJavaObject createOneShot = _vibrationEffect.CallStatic<AndroidJavaObject>(_CREATE_ONE_SHOT_METHOD, milliseconds, amplitude);
                    _vibrator.Call(_VIBRATE_METHOD, createOneShot);
                } else {
                    using AndroidJavaObject createOneShot = _vibrationEffect.CallStatic<AndroidJavaObject>(_CREATE_ONE_SHOT_METHOD, milliseconds, _defaultAmplitude);
                    _vibrator.Call(_VIBRATE_METHOD, createOneShot);
                }
            } else {
                _vibrator.Call(_VIBRATE_METHOD, milliseconds);
            }
        #elif UNITY_IOS
            Handheld.Vibrate();
        #endif
        }
        
        public static void CreatePredefined(PredefinedEffect effect) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            switch (effect) {
                case PredefinedEffect.DoubleClick: CreateWaveform(new long[] { 0, 64, 0, 64 }); break;
                case PredefinedEffect.Tick: CreateOneShot(32); break;
                case PredefinedEffect.HeavyClick: CreateOneShot(128); break;
                default: CreateOneShot(64); break;
            }
        #elif UNITY_IOS
            Handheld.Vibrate();
        #endif
        }
        
        /// <summary> Create a waveform vibration </summary>
        /// <param name="timings"> The timing values, in milliseconds, of the timing / amplitude pairs. Timing values of 0 will cause the pair to be ignored </param>
        public static void CreateWaveform(long[] timings) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            int[] amplitudes = new int[timings.Length];
            
            for (int amplitudeId = 2; amplitudeId < amplitudes.Length; amplitudeId += 2) {
                amplitudes[amplitudeId] = _defaultAmplitude;
            }
            
            CreateWaveform(timings, amplitudes, -1);
        #endif
        }
        
        /// <summary> Create a waveform vibration </summary>
        /// <param name="timings"> The timing values, in milliseconds, of the timing / amplitude pairs. Timing values of 0 will cause the pair to be ignored </param>
        /// <param name="amplitude"> The amplitude values of the timing / amplitude pairs. Amplitude values must be between 0 and 255, or equal to DEFAULT_AMPLITUDE.
        /// An amplitude value of 0 implies the motor is off </param>
        public static void CreateWaveform(long[] timings, int amplitude) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            int[] amplitudes = new int[timings.Length];
            
            for (int amplitudeId = 2; amplitudeId < amplitudes.Length; amplitudeId += 2) {
                amplitudes[amplitudeId] = amplitude;
            }
            
            CreateWaveform(timings, amplitudes, -1);
        #endif
        }
        
        /// <summary> Create a waveform vibration </summary>
        /// <param name="timings"> The timing values, in milliseconds, of the timing / amplitude pairs. Timing values of 0 will cause the pair to be ignored </param>
        /// <param name="amplitudes"> The amplitude values of the timing / amplitude pairs. Amplitude values must be between 0 and 255, or equal to DEFAULT_AMPLITUDE.
        /// An amplitude value of 0 implies the motor is off </param>
        public static void CreateWaveform(long[] timings, int[] amplitudes) => CreateWaveform(timings, amplitudes, -1);
        
        /// <summary> Create a waveform vibration </summary>
        /// <param name="timings"> The timing values, in milliseconds, of the timing / amplitude pairs. Timing values of 0 will cause the pair to be ignored </param>
        /// <param name="amplitudes"> The amplitude values of the timing / amplitude pairs. Amplitude values must be between 0 and 255, or equal to DEFAULT_AMPLITUDE.
        /// An amplitude value of 0 implies the motor is off </param>
        /// <param name="repeat"> The index into the timings array at which to repeat, or -1 if you don't want to repeat indefinitely. </param>
        public static void CreateWaveform(long[] timings, int[] amplitudes, int repeat) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            Cancel();
            
            for (int timingId = 1; timingId < timings.Length; timingId += 2) {
                if (timings[timingId] < 1) {
                    timings[timingId] = 1;
                }
                
                if (amplitudes[timingId] < 1) {
                    amplitudes[timingId] = 1;
                } else if (amplitudes[timingId] > 255) {
                    amplitudes[timingId] = 255;
                }
            }
            
            if (_isSupportVibrationEffect) {
                if (_hasAmplitudeControl) {
                    using AndroidJavaObject createWaveform = _vibrationEffect.CallStatic<AndroidJavaObject>(_CREATE_WAVEFORM_METHOD, timings, amplitudes, repeat);
                    _vibrator.Call(_VIBRATE_METHOD, createWaveform);
                } else {
                    using AndroidJavaObject createWaveform = _vibrationEffect.CallStatic<AndroidJavaObject>(_CREATE_WAVEFORM_METHOD, timings, repeat);
                    _vibrator.Call(_VIBRATE_METHOD, createWaveform);
                }
            } else {
                _vibrator.Call(_VIBRATE_METHOD, timings, repeat);
            }
            
        #elif UNITY_IOS
            Handheld.Vibrate();
        #endif
        }
        
        public static void Cancel() {
        #if UNITY_ANDROID && !UNITY_EDITOR
            _vibrator.Call(_CANCEL_METHOD);
        #endif
        }
    }
}