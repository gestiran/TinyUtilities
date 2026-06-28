// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Reflection;
using System.Threading;
using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.EnterPlayMode.AsyncTools {
    public sealed class AsyncToolsModule {
        private bool _isEnable;
        
        private readonly AsyncToolsPrefs _prefs;

        public AsyncToolsModule() {
            _prefs = new AsyncToolsPrefs();
            Init();
        }

        public void Init() {
            _isEnable = _prefs.LoadIsEnable();
        }

        public void PlayModeStateChanged(PlayModeStateChange state) {
            if (_isEnable == false) {
                return;
            } 
            
            if (state != PlayModeStateChange.ExitingPlayMode) {
                return;
            }
            
            StopAllAsync();
        }

        public void Draw() {
            _isEnable = GUIDrawUtility.DrawToggle("Stop all async", _isEnable, _prefs.SaveIsEnable);
        }

        private void StopAllAsync() {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            Type[] constructorParameters = new Type[] { typeof(int) };

            SynchronizationContext currentContext = SynchronizationContext.Current;
            ConstructorInfo constructor = currentContext.GetType().GetConstructor(flags, null, constructorParameters, null);

            object[] parameters = { Thread.CurrentThread.ManagedThreadId };
            
            if (constructor == null) {
                return;
            }

            SynchronizationContext newContext = constructor.Invoke(parameters) as SynchronizationContext;

            SynchronizationContext.SetSynchronizationContext(newContext);
        }
    }
}