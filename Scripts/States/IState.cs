// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

namespace TinyUtilities.States {
    public interface IState {
        public void Enter();
        
        public void Exit();
    }
}