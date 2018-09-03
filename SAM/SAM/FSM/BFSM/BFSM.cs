using System;
using System.Collections;
using System.Collections.Generic;

namespace SAM.FSM.BFSM
{
    public class BFSM<TState, TTrigger> : IFSM<TState> where TState : Enum where TTrigger : Enum
    {
        protected Dictionary<TState, List<StateBehaviour<TState, TTrigger>>> states;
        protected List<Transition<TState, TTrigger>> transitions;

        private int currentIndex;

        private bool kickoff;

        public TState InnerCurrentState { get; protected set; }

        public event ChangedStateEvent<TState> onStateChanged;

        public BFSM()
        {
            states = new Dictionary<TState, List<StateBehaviour<TState, TTrigger>>>();
        }

        public void UpdateCurrentState()
        {
            if(kickoff)
            {
                UpdateBehaviours(InnerCurrentState);
            }
        }

        public void AddState(TState state)
        {
            if(states.ContainsKey(state))
            {
                throw new ArgumentException(nameof(TState) + " value is already added");
            }
            else
            {
                states.Add(state, new List<StateBehaviour<TState, TTrigger>>());
            }
        }

        public void RemoveState(TState state)
        {
            if (InnerCurrentState.Equals(state))
            {
                throw new InvalidOperationException("Cannot remove current running state");
            }
            else
            {
                states.Remove(state);
            }
        }

        public void AddBehaviour(StateBehaviour<TState, TTrigger> behaviour)
        {
            TState parentState = behaviour.ParentState;

            if(states.ContainsKey(parentState))
            {
                states[parentState].Add(behaviour);
            }
        }

        public void RemoveBehaviour(StateBehaviour<TState, TTrigger> behaviour)
        {
            TState parentState = behaviour.ParentState;

            if(states.ContainsKey(parentState))
            {
                if(states[parentState].Remove(behaviour))
                {
                    currentIndex--;
                }
            }
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

        public void StartBy(TState state)
        {
            if (kickoff == false)
            {
                if (SetState(state))
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

                EnterBehaviours(InnerCurrentState);

                return true;
            }

            return false;
        }

        public void Trigger(TTrigger trigger)
        {
            Transition<TState, TTrigger> transition = new Transition<TState, TTrigger>(InnerCurrentState, trigger);

            if (transitions.Contains(transition))
            {
                transition = transitions[transitions.IndexOf(transition)];

                TState previosState = InnerCurrentState;
                TState nextState = transition.stateTo;

                ExitBehaviours(previosState);

                SetState(nextState);

                if (onStateChanged != null)
                {
                    onStateChanged(previosState, nextState);
                }
            }
        }

        public T GetBehaviour<T>()
        {
            foreach(TState state in this)
            {
                List<StateBehaviour<TState, TTrigger>> behaviours = states[state];

                foreach (StateBehaviour<TState, TTrigger> behaviour in behaviours)
                {
                    if (behaviour is T obtained)
                    {
                        return obtained;
                    }
                }
            }

            return default;
        }

        public T GetBehaviourFromState<T>(TState state)
        {
            if(states.ContainsKey(state))
            {
                List<StateBehaviour<TState, TTrigger>> behaviours = states[state];

                foreach (StateBehaviour<TState, TTrigger> behaviour in behaviours)
                {
                    if (behaviour is T obtained)
                    {
                        return obtained;
                    }
                }
            }

            return default;
        }

        public T[] GetBehaviours<T>()
        {
            List<T> requested = null;

            foreach (TState state in this)
            {
                List<StateBehaviour<TState, TTrigger>> behaviours = states[state];

                foreach (StateBehaviour<TState, TTrigger> behaviour in behaviours)
                {
                    if (behaviour is T obtained)
                    {
                        if(requested == null)
                        {
                            requested = new List<T>();
                        }

                        requested.Add(obtained);
                    }
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

        public T[] GetBehavioursFromState<T>(TState state)
        {
            List<T> requested = null;

            if(states.ContainsKey(state))
            {
                List<StateBehaviour<TState, TTrigger>> behaviours = states[state];

                foreach (StateBehaviour<TState, TTrigger> behaviour in behaviours)
                {
                    if (behaviour is T obtained)
                    {
                        if (requested == null)
                        {
                            requested = new List<T>();
                        }

                        requested.Add(obtained);
                    }
                }
            }

            if (requested == null)
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

        public IEnumerator<TState> GetEnumerator()
        {
            return states.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void ExitBehaviours(TState state)
        {
            List<StateBehaviour<TState, TTrigger>> behaviours = states[state];

            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].ExitState();
            }
        }

        private void EnterBehaviours(TState state)
        {
            List<StateBehaviour<TState, TTrigger>> behaviours = states[state];

            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].EnterState();
            }
        }

        private void UpdateBehaviours(TState state)
        {
            List<StateBehaviour<TState, TTrigger>> behaviours = states[state];

            for (currentIndex = 0; currentIndex < behaviours.Count; currentIndex++)
            {
                behaviours[currentIndex].UpdateBehaviour();
            }
        }
    }
}
