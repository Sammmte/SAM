using System;
using System.Threading;

namespace SAM.Tasks
{
    /// <summary>
    /// Reusable asynchronous task
    /// </summary>
    public abstract class Task
    {
        /// <summary>
        /// if the task has finished
        /// </summary>
        public bool Done { get; protected set; }

        /// <summary>
        /// if the task succeed.
        /// </summary>
        public bool Succeed { get; protected set; }

        /// <summary>
        /// if the task is running.
        /// </summary>
        public bool IsRunning { get; protected set; }

        protected Thread thread;

        /// <summary>
        /// Gets triggered when the task success.
        /// </summary>
        public event Action onSucceed;

        /// <summary>
        /// Gets triggered when the task fails.
        /// </summary>
        public event Action onFailed;

        protected internal Task()
        {

        }

        /// <summary>
        /// Execute the task without parameters
        /// </summary>
        public void DoTask()
        {
            DoTask(null);
        }

        /// <summary>
        /// Execute the task with an object parameter
        /// </summary>
        /// <param name="param"></param>
        public void DoTask(object param)
        {
            GeneralReset();

            Reset();

            thread = new Thread(TaskModel);

            thread.Start(param);
        }

        private void TaskModel(object obj)
        {
            IsRunning = true;

            try
            {
                Succeed = InnerTask(obj);
            }
            catch
            {
                Succeed = false;
            }
            

            Done = true;
            IsRunning = false;

            if(Succeed)
            {
                if(onSucceed != null)
                {
                    onSucceed();
                }
            }
            else
            {
                if(onFailed != null)
                {
                    onFailed();
                }
            }

        }

        /// <summary>
        /// Represents the task to execute. Return true if the task has succeed, else return false.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected abstract bool InnerTask(object obj);

        private void GeneralReset()
        {
            Done = false;
            Succeed = false;
            IsRunning = false;
        }

        /// <summary>
        /// Use this method to do anything you need before the task begin.
        /// </summary>
        protected virtual void Reset()
        {
            //custom reset for derived classes
        }
    }
}
