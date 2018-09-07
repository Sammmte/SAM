using System;

namespace SAM.Tasks
{
    /// <summary>
    /// Task that can store data of a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Task<T> : Task
    {
        protected internal DataContainer<T> dataContainer;

        public event Action<T> onCatchData;

        /// <summary>
        /// The data obtained by the task
        /// </summary>
        public T Data
        {
            get
            {
                return dataContainer.data;
            }

            internal set
            {
                dataContainer.data = value;
            }
        }

        protected Task()
        {
            dataContainer = new DataContainer<T>();

            onSucceed += OnCatchData;
        }

        private void OnCatchData()
        {
            if(onCatchData != null)
            {
                onCatchData(Data);
            }
        }
    }
}
