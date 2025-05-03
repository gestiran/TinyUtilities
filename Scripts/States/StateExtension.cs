using System;

namespace TinyUtilities.States {
    public static class StateExtension {
        public static void ChangeState<T1, T2>(this T1 stateMachine, T2 state) where T1 : IStateMachine where T2 : IState {
            if (stateMachine.currentState != null) {
                stateMachine.currentState.Exit();
            }
            
            stateMachine.currentState = state;
            stateMachine.currentState.Enter();
        }
        
        public static bool TryChangeState<T1, T2>(this T1 stateMachine, Func<T2> createState) where T1 : IStateMachine where T2 : IState {
            if (stateMachine.currentState != null && stateMachine.currentState.GetType() == typeof(T2)) {
                return false;
            }
            
            ChangeState(stateMachine, createState.Invoke());
            return true;
        }
    }
}