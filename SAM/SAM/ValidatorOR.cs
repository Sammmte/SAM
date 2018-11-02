using System;

namespace SAM
{
    public class ValidatorOR : Validator
    {
        protected override bool Validation()
        {
            Delegate[] invocationList = predicate.GetInvocationList();

            foreach (Func<bool> validation in invocationList)
            {
                if (validation())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
