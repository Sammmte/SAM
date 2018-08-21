using System;
using System.Collections.Generic;

namespace SAM.FSM
{
    public abstract class State<TState, TTrigger> where TState : Enum where TTrigger : Enum
    {
        protected FSM<TState, TTrigger> stateMachine;
        private List<Transition<TState, TTrigger>> transitions;

        protected TState innerState;

        public TState InnerState
        {
            get
            {
                return innerState;
            }
        }

        public FSM<TState, TTrigger> StateMachine
        {
            get
            {
                return stateMachine;
            }
        }

        public event Action onEnter;
        public event Action onExit;

        protected State(FSM<TState, TTrigger> stateMachine, TState state)
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

        internal void Enter()
        {
            OnEnter();

            if (onEnter != null)
            {
                onEnter();
            }
        }

        internal void Exit()
        {
            OnExit();

            if (onExit != null)
            {
                onExit();
            }
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
        public abstract void Update();
    }
}
