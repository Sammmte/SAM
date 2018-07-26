using System;

namespace SAM.Tasks
{
    public sealed class ActionTask : Task, IDelegateTask<Action>
    {
        private Action action;

        public ActionTask()
        {

        }

        public ActionTask(Action _action) : this()
        {
            Override(_action);
        }

        public void Override(Action _action)
        {
            action = _action;
        }

        public void Add(Action _action)
        {
            action += _action;
        }

        public void Remove(Action _action)
        {
            action -= _action;
        }

        protected override bool InnerTask(object obj)
        {
            try
            {
                action();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
