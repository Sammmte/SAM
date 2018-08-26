using System;
using System.Collections.Generic;

namespace SAM.FSM
{
    public class FSM<TState, TTrigger, TChangedStateEventArgs> where TState : Enum where TTrigger : Enum where TChangedStateEventArgs : struct
    {
        private Dictionary<TState, State<TState, TTrigger, TChangedStateEventArgs>> states;

        public State<TState, TTrigger, TChangedStateEventArgs> CurrentState { get; private set; }

        public int StateCount
        {
            get
            {
                return states.Count;
            }
        }

        public FSM()
        {
            states = new Dictionary<TState, State<TState, TTrigger, TChangedStateEventArgs>>();
        }

        public void AddState(State<TState, TTrigger, TChangedStateEventArgs> state)
        {
            states.Add(state.InnerState, state);
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
            if(CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        public void StartBy(TState state)
        {
            if(CurrentState == null)
            {
                TChangedStateEventArgs e = default(TChangedStateEventArgs);
                SetState(state, ref e);
            }
        }

        private void SetState(TState state, ref TChangedStateEventArgs e)
        {
            if(states.ContainsKey(state))
            {
                CurrentState = states[state];

                CurrentState.Enter(e);
            }
        }

        public void Trigger(TTrigger trigger)
        {
            TChangedStateEventArgs e = default(TChangedStateEventArgs);
            Trigger(trigger, ref e);
        }

        public void Trigger(TTrigger trigger, ref TChangedStateEventArgs e)
        {
            if (CurrentState != null && CurrentState.ContainsTransition(trigger))
            {
                State<TState, TTrigger, TChangedStateEventArgs> previous = CurrentState;

                TState nextState;

                previous.TryGetNext(trigger, out nextState);

                previous.Exit();

                SetState(nextState, ref e);
            }
        }
    }
}
