using System;

namespace SAM
{
    public class ValidatorAND : Validator
    {
        protected override bool Validation()
        {
            Delegate[] invocationList = predicate.GetInvocationList();

            foreach (Func<bool> validation in invocationList)
            {
                if (!validation())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
