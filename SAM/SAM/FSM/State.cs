using System;

namespace SAM.FSM
{
    public delegate void ChangedStateEvent<TState>(TState previous, TState current) where TState : Enum;

    public abstract class State<TState, TTrigger> where TState : Enum where TTrigger : Enum
    {
        public TState InnerState { get; protected set; }

        public FSM<TState, TTrigger> stateMachine { get; protected set; }

        protected State(FSM<TState, TTrigger> stateMachine, TState state)
        {
            this.stateMachine = stateMachine;
            InnerState = state;
        }

        internal void Enter()
        {
            OnEnter();
        }

        internal void Exit()
        {
            OnExit();
        }

        internal void Update()
        {
            OnUpdate();
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract void OnUpdate();
    }

    public abstract class State<TState, TTrigger, TChangedStateEventArgs> : State<TState, TTrigger> where TState : Enum where TTrigger : Enum where TChangedStateEventArgs : struct
    {
        new public FSM<TState, TTrigger, TChangedStateEventArgs> stateMachine
        {
            get
            {
                return (FSM<TState, TTrigger, TChangedStateEventArgs>)base.stateMachine;
            }

            protected set
            {
                base.stateMachine = value;
            }
        }

        protected State(FSM<TState, TTrigger, TChangedStateEventArgs> stateMachine, TState state) : base(null, state)
        {
            this.stateMachine = stateMachine;
        }

        new private void Enter()
        {

        }

        internal void Enter(TChangedStateEventArgs e)
        {
            OnEnter(ref e);
        }

        protected sealed override void OnEnter()
        {
            
        }

        protected abstract void OnEnter(ref TChangedStateEventArgs e);
    }
}
