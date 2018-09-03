using System;
using System.Collections.Generic;

namespace SAM.FSM
{
    public interface IFSM
    {
        void UpdateCurrentState();
    }

    public interface IFSM<TState> : IFSM, IEnumerable<TState> where TState : Enum
    {
        TState InnerCurrentState { get; }

        event ChangedStateEvent<TState> onStateChanged;

        void AddState(TState state);
        void RemoveState(TState state);
        void StartBy(TState state);
        bool ContainsState(TState state);
    }
}
