using System;
using System.Collections.Generic;

namespace SAM.FSM
{
    public class FSM<TState, TTrigger> where TState : Enum where TTrigger : Enum
    {
        private Dictionary<TState, State<TState, TTrigger>> states;

        private State<TState, TTrigger> currentState;

        public State<TState, TTrigger> CurrentState
        {
            get
            {
                return currentState;
            }
        }

        public FSM()
        {
            states = new Dictionary<TState, State<TState, TTrigger>>();
        }

        public void AddState(TState state_p, State<TState, TTrigger> state)
        {
            states.Add(state_p, state);
        }

        public void MakeTransition(TState stateFrom, TTrigger trigger, TState stateTo)
        {
            if(states.ContainsKey(stateFrom) && states.ContainsKey(stateTo))
            {
                states[stateFrom].AddTransition(trigger, stateTo);
            }
            else
            {
                throw new ArgumentException(nameof(TState) + " value not added as a valid State");
            }
        }

        public void BreakTransition(TState stateFrom, TTrigger trigger)
        {
            if(states.ContainsKey(stateFrom))
            {
                states[stateFrom].RemoveTransition(trigger);
            }
        }

        public void UpdateCurrentState()
        {
            if(currentState != null)
            {
                currentState.Update();
            }
        }

        public void StartBy(TState state)
        {
            if(currentState == null)
            {
                SetState(state);
            }
        }

        private void SetState(TState state)
        {
            if(states.ContainsKey(state))
            {
                currentState = states[state];

                currentState.Enter();
            }
        }

        public void Trigger(TTrigger trigger)
        {
            if(currentState != null && currentState.ContainsTransition(trigger))
            {
                State<TState, TTrigger> previous = currentState;

                TState nextState = default(TState);

                previous.TryGetNext(trigger, out nextState);

                previous.Exit();

                currentState = states[nextState];

                currentState.Enter();
            }
        }
    }
}
