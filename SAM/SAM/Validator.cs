using System;

namespace SAM
{
    public abstract class Validator
    {
        protected Func<bool> predicate;

        public virtual void AddValidation(Func<bool> predicate)
        {
            this.predicate += predicate;
        }

        public virtual void RemoveValidation(Func<bool> predicate)
        {
            this.predicate -= predicate;
        }

        public bool IsValid()
        {
            if (predicate == null)
            {
                return true;
            }
            else
            {
                return Validation();
            }

        }

        protected abstract bool Validation();
    }
}
