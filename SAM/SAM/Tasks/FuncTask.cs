using System;

namespace SAM.Tasks
{
    public sealed class FuncTask<T> : Task<T>, IDelegateTask<Func<T>>
    {
        private Func<T> func;

        public FuncTask()
        {

        }

        public FuncTask(Func<T> _func) : this()
        {
            Override(_func);
        }

        public void Override(Func<T> _func)
        {
            func = _func;
        }

        public void Add(Func<T> _func)
        {
            func += _func;
        }

        public void Remove(Func<T> _func)
        {
            func -= _func;
        }

        protected override bool InnerTask(object obj)
        {
            try
            {
                Data = func();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
