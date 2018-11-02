using System;
using System.Collections;
using System.Threading.Tasks;

namespace SAM
{
    public class ValidatorAsync : Validator
    {
        private Func<bool> isValidDelegate;

        private Validator validator;

        public ValidatorAsync(Validator validator)
        {
            this.validator = validator;

            isValidDelegate = Validation;
        }

        protected override bool Validation()
        {
            return this.validator.IsValid();
        }

        public override void AddValidation(Func<bool> predicate)
        {
            validator.AddValidation(predicate);
        }

        public override void RemoveValidation(Func<bool> predicate)
        {
            validator.RemoveValidation(predicate);
        }

        public void IsValidAsync(Action<bool> asyncCallback, Action asyncFailedCallback)
        {
            Task task;

            task = Task.Run(

                delegate ()
                {
                    try
                    {
                        if (asyncCallback != null)
                        {
                            asyncCallback(Validation());
                        }
                    }
                    catch
                    {
                        if (asyncFailedCallback != null)
                        {
                            asyncFailedCallback();
                        }
                    }

                }

                );
        }

        public IEnumerator IsValidAsyncCoroutine(Action<bool> syncCallback, Action syncFailedCallback)
        {
            Task<bool> task;

            task = Task.Run<bool>(isValidDelegate);

            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                if (syncFailedCallback != null)
                {
                    syncFailedCallback();
                }
            }
            else
            {
                bool result = task.Result;

                if (syncCallback != null)
                {
                    syncCallback(result);
                }
            }

            task.Dispose();
        }
    }
}
