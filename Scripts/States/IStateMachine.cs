namespace TinyUtilities.States {
    public interface IStateMachine {
        public IState currentState { get; set; }
    }
}