using System;
using System.Collections.Generic;

namespace SAM.FSM
{
    public delegate void ChangedStateEvent<TState, TTrigger, TEventArgs>(State<TState, TTrigger, TEventArgs> state, ref TEventArgs e) where TState : Enum where TTrigger : Enum where TEventArgs : struct;

    public abstract class State<TState, TTrigger, TChangedStateEventArgs> where TState : Enum where TTrigger : Enum where TChangedStateEventArgs : struct
    {
        protected FSM<TState, TTrigger, TChangedStateEventArgs> stateMachine;
        private List<Transition<TState, TTrigger>> transitions;

        protected TState innerState;

        public TState InnerState
        {
            get
            {
                return innerState;
            }
        }

        public FSM<TState, TTrigger, TChangedStateEventArgs> StateMachine
        {
            get
            {
                return stateMachine;
            }
        }

        public event ChangedStateEvent<TState, TTrigger, TChangedStateEventArgs> onEnter;
        public event Action<State<TState, TTrigger, TChangedStateEventArgs>> onExit;

        protected State(FSM<TState, TTrigger, TChangedStateEventArgs> stateMachine, TState state)
        {
            this.stateMachine = stateMachine;
            innerState = state;

            transitions = new List<Transition<TState, TTrigger>>();
        }

        internal void AddTransition(TTrigger trigger, TState stateTo)
        {
            Transition<TState, TTrigger> transition = new Transition<TState, TTrigger>(innerState, trigger, stateTo);
            
            if (transitions.Contains(transition) == false)
            {
                transitions.Add(transition);
            }
        }

        internal void RemoveTransition(TTrigger trigger)
        {
            Transition<TState, TTrigger> transition = new Transition<TState, TTrigger>(innerState, trigger, default(TState));

            transitions.Remove(transition);
        }

        public bool ContainsTransition(TTrigger trigger)
        {
            foreach(Transition<TState, TTrigger> transition in transitions)
            {
                if(transition.trigger.Equals(trigger))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetNext(TTrigger trigger, out TState nextState)
        {
            foreach(Transition<TState, TTrigger> transition in transitions)
            {
                if(transition.trigger.Equals(trigger))
                {
                    nextState = transition.stateTo;

                    return true;
                }
            }

            nextState = default;

            return false;
        }

        internal void Enter(TChangedStateEventArgs e)
        {
            OnEnter(ref e);

            if (onEnter != null)
            {
                onEnter(this, ref e);
            }
        }

        internal void Exit()
        {
            OnExit();

            if (onExit != null)
            {
                onExit(this);
            }
        }

        internal void Update()
        {
            OnUpdate();
        }

        protected abstract void OnEnter(ref TChangedStateEventArgs e);
        protected abstract void OnExit();
        protected abstract void OnUpdate();
    }
}
