using System;
using System.Threading;

namespace SAM.Tasks
{
    /// <summary>
    /// Reusable asynchronous task
    /// </summary>
    public abstract class Task
    {
        private bool done;
        private bool succeed;
        private bool isRunning;

        /// <summary>
        /// if the task has finished
        /// </summary>
        public bool Done
        {
            get
            {
                return done;
            }
        }

        /// <summary>
        /// if the task succeed.
        /// </summary>
        public bool Succeed
        {
            get
            {
                return succeed;
            }
        }

        /// <summary>
        /// if the task is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

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
        /// <param name="obj"></param>
        public void DoTask(object obj)
        {
            GeneralReset();

            Reset();

            thread = new Thread(TaskModel);

            thread.Start(obj);
        }

        private void TaskModel(object obj)
        {
            isRunning = true;

            succeed = InnerTask(obj);

            done = true;
            isRunning = false;

            if(succeed)
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
            done = false;
            succeed = false;
            isRunning = false;
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
