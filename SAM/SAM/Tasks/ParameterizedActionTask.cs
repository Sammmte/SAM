namespace SAM.Tasks
{
    public sealed class ParameterizedActionTask : Task, IParameterizedDelegateTask<ParameterizedAction>
    {
        private struct ActionWithParams
        {
            public object[] parameters;
            public ParameterizedAction action;

            public void Execute()
            {
                action(parameters);
            }
        }

        private ActionWithParams actionWithParams;

        public ParameterizedActionTask()
        {
            actionWithParams = new ActionWithParams();
        }

        public ParameterizedActionTask(ParameterizedAction _action) : this()
        {
            Override(_action);
        }

        public void Override(ParameterizedAction _action)
        {
            actionWithParams.action = _action;
        }

        public void Add(ParameterizedAction _action)
        {
            actionWithParams.action += _action;
        }

        public void Remove(ParameterizedAction _action)
        {
            actionWithParams.action -= _action;
        }

        public void SetParams(params object[] parameters)
        {
            actionWithParams.parameters = parameters;
        }

        protected override bool InnerTask(object obj)
        {
            try
            {
                actionWithParams.Execute();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
