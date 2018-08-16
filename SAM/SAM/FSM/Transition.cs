using System;

namespace SAM.FSM
{
    public struct Transition<TState, TTrigger> : IEquatable<Transition<TState, TTrigger>> where TState : Enum where TTrigger : Enum
    {
        public readonly TState stateFrom;
        public readonly TTrigger trigger;
        public readonly TState stateTo;

        public Transition(TState stateFrom, TTrigger trigger, TState stateTo)
        {
            this.stateFrom = stateFrom;
            this.trigger = trigger;
            this.stateTo = stateTo;
        }

        public bool Equals(Transition<TState, TTrigger> other)
        {
            if (stateFrom.Equals(other.stateFrom) && trigger.Equals(other.trigger))
            {
                return true;
            }

            return false;
        }
    }
}
