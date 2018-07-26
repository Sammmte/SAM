using System;

namespace SAM.Tasks
{
    public interface IDelegateTask<T> where T : Delegate
    {
        void Override(T deleg);

        void Add(T deleg);

        void Remove(T deleg);
    }

    public delegate TResult ParameterizedFunc<TResult>(params object[] paramereters);

    public delegate void ParameterizedAction(params object[] paramereters);

}
