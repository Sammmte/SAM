using System;
using System.Collections;
using System.Collections.Generic;

namespace SAM.FSM
{
    public class FSM<TState, TTrigger> : IFSM<TState> where TState : Enum where TTrigger : Enum
    {
        protected Dictionary<TState, State<TState, TTrigger>> states;
        protected List<Transition<TState, TTrigger>> transitions;

        private bool kickoff = false;

        public int StateCount
        {
            get
            {
                return states.Count;
            }
        }

        public State<TState, TTrigger> CurrentState { get; protected set; }

        public TState InnerCurrentState { get; protected set; }

        public virtual event ChangedStateEvent<TState> onStateChanged;

        public FSM()
        {
            states = new Dictionary<TState, State<TState, TTrigger>>();
            transitions = new List<Transition<TState, TTrigger>>();
        }

        public void AddState(State<TState, TTrigger> state)
        {
            states.Add(state.InnerState, state);
        }

        public void AddState(TState state)
        {
            states.Add(state, null);
        }

        public void MakeTransition(TState stateFrom, TTrigger trigger, TState stateTo)
        {
            if (states.ContainsKey(stateFrom) && states.ContainsKey(stateTo))
            {
                Transition<TState, TTrigger> transition = new Transition<TState, TTrigger>(stateFrom, trigger, stateTo);

                if (transitions.Contains(transition) == false)
                {
                    transitions.Add(transition);
                }
            }
            else
            {
                throw new ArgumentException(nameof(TState) + " value not added as a valid State");
            }
        }

        public void BreakTransition(TState stateFrom, TTrigger trigger)
        {
            if (states.ContainsKey(stateFrom))
            {
                Transition<TState, TTrigger> transition = new Transition<TState, TTrigger>(stateFrom, trigger);

                transitions.Remove(transition);
            }
        }

        public T GetState<T>()
        {
            foreach(KeyValuePair<TState, State<TState, TTrigger>> state in states)
            {
                if(state.Value is T obtained)
                {
                    return obtained;
                }
            }

            return default;
        }

        public T GetStateByInnerState<T>(TState innerState)
        {
            foreach (KeyValuePair<TState, State<TState, TTrigger>> state in states)
            {
                if (state.Key.Equals(innerState) && state.Value is T obtained)
                {
                    return obtained;
                }
            }

            return default;
        }

        public T[] GetStates<T>()
        {
            List<T> requested = null;

            foreach (KeyValuePair<TState, State<TState, TTrigger>> state in states)
            {
                if (state.Value is T obtained)
                {
                    if(requested == null)
                    {
                        requested = new List<T>();
                    }

                    requested.Add(obtained);
                }
            }

            if(requested == null)
            {
                return null;
            }
            else
            {
                T[] array = requested.ToArray();

                requested.Clear();

                return array;
            }
        }

        public bool ContainsState(TState state)
        {
            return states.ContainsKey(state);
        }

        public bool ContainsTransition(TState state, TTrigger trigger)
        {
            return transitions.Contains(new Transition<TState, TTrigger>(state, trigger));
        }

        public void UpdateCurrentState()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        public void StartBy(TState state)
        {
            if (kickoff == false)
            {
                if(SetState(state))
                {
                    kickoff = true;
                }
                else
                {
                    throw new ArgumentException("State was not found");
                }
            }
        }

        protected bool SetState(TState state)
        {
            if (states.ContainsKey(state))
            {
                InnerCurrentState = state;

                CurrentState = states[state];

                if (CurrentState != null)
                {
                    CurrentState.Enter();
                }

                return true;
            }

            return false;
        }

        public virtual void Trigger(TTrigger trigger)
        {
            Transition<TState, TTrigger> transition = new Transition<TState, TTrigger>(InnerCurrentState, trigger);

            if (transitions.Contains(transition))
            {
                State<TState, TTrigger> previous = CurrentState;
                transition = transitions[transitions.IndexOf(transition)];

                TState previosState = InnerCurrentState;
                TState nextState = transition.stateTo;

                if (previous != null)
                {
                    previous.Exit();
                }

                SetState(nextState);

                if(onStateChanged != null)
                {
                    onStateChanged(previosState, nextState);
                }
            }
        }

        public void RemoveState(TState state)
        {
            if(InnerCurrentState.Equals(state))
            {
                throw new InvalidOperationException("Cannot remove current running state");
            }
            else
            {
                states.Remove(state);
            }
        }

        public IEnumerator<TState> GetEnumerator()
        {
            return states.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class FSM<TState, TTrigger, TChangedStateEventArgs> : FSM<TState, TTrigger> where TState : Enum where TTrigger : Enum where TChangedStateEventArgs : struct
    {
        new public State<TState, TTrigger, TChangedStateEventArgs> CurrentState
        {
            get
            {
                return (State<TState, TTrigger, TChangedStateEventArgs>)base.CurrentState;
            }

            protected set
            {
                base.CurrentState = value;
            }
        }

        private bool kickoff;

        public override event ChangedStateEvent<TState> onStateChanged;

        new private void AddState(State<TState, TTrigger> state)
        {

        }

        public void AddState(State<TState, TTrigger, TChangedStateEventArgs> state)
        {
            states.Add(state.InnerState, state);
        }

        new public void StartBy(TState state)
        {
            if (kickoff == false)
            {
                kickoff = true;
                TChangedStateEventArgs e = default;
                SetState(state, ref e);
            }
        }

        new private void SetState(TState state)
        {

        }

        protected void SetState(TState state, ref TChangedStateEventArgs e)
        {
            if (states.ContainsKey(state))
            {
                InnerCurrentState = state;

                CurrentState = (State<TState, TTrigger, TChangedStateEventArgs>)states[state];

                if (CurrentState != null)
                {
                    CurrentState.Enter(e);
                }
            }
        }

        public override void Trigger(TTrigger trigger)
        {
            TChangedStateEventArgs e = default;
            Trigger(trigger, ref e);
        }

        public void Trigger(TTrigger trigger, ref TChangedStateEventArgs e)
        {
            Transition<TState, TTrigger> transition = new Transition<TState, TTrigger>(InnerCurrentState, trigger);

            if (transitions.Contains(transition))
            {
                State<TState, TTrigger, TChangedStateEventArgs> previous = CurrentState;
                transition = transitions[transitions.IndexOf(transition)];

                TState previousState = InnerCurrentState;
                TState nextState = transition.stateTo;

                if(previous != null)
                {
                    previous.Exit();
                }

                SetState(nextState, ref e);

                if(onStateChanged != null)
                {
                    onStateChanged(previousState, nextState);
                }
            }
        }
    }
}
