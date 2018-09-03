using System;

namespace SAM.FSM.BFSM
{
    public abstract class StateBehaviour<TState, TTrigger> where TState : Enum where TTrigger : Enum
    {
        public BFSM<TState, TTrigger> StateMachine { get; protected set; }

        public TState ParentState { get; protected set; }

        public StateBehaviour(BFSM<TState, TTrigger> stateMachine, TState parentState)
        {
            StateMachine = stateMachine;
            ParentState = parentState;
        }

        internal void EnterState()
        {
            OnEnterState();
        }

        internal void UpdateBehaviour()
        {
            OnUpdateState();
        }

        internal void ExitState()
        {
            OnExitState();
        }

        protected abstract void OnEnterState();
        protected abstract void OnUpdateState();
        protected abstract void OnExitState();
    }
}
