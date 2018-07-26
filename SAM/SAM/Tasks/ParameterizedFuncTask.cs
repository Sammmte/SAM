namespace SAM.Tasks
{
    public sealed class ParameterizedFuncTask<T> : Task<T>, IParameterizedDelegateTask<ParameterizedFunc<T>>
    {
        private struct FuncWithParams
        {
            public object[] parameters;
            public ParameterizedFunc<T> func;

            public T Execute()
            {
                return func(parameters);
            }
        }

        private FuncWithParams funcWithParams;

        public ParameterizedFuncTask()
        {
            funcWithParams = new FuncWithParams();
        }

        public ParameterizedFuncTask(ParameterizedFunc<T> _func) : this()
        {
            Override(_func);
        }

        public void Override(ParameterizedFunc<T> _func)
        {
            funcWithParams.func = _func;
        }

        public void Add(ParameterizedFunc<T> _func)
        {
            funcWithParams.func += _func;
        }

        public void Remove(ParameterizedFunc<T> _func)
        {
            funcWithParams.func -= _func;
        }

        public void SetParams(params object[] _parameters)
        {
            funcWithParams.parameters = _parameters;
        }

        protected override bool InnerTask(object obj)
        {
            try
            {
                Data = funcWithParams.Execute();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
