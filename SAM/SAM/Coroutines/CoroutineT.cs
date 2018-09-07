using System.Collections;

namespace SAM.Coroutines
{
    public class Coroutine<T> : Coroutine
    {
        public T Result { get; protected set; }

        public Coroutine(IEnumerator iterator) : base(iterator)
        {

        }

        protected override bool RecursiveMoveNext(IEnumerator recursiveIterator)
        {
            if (recursiveIterator.Current is IYieldInstruction yieldInst)
            {
                if (yieldInst.KeepWaiting)
                {
                    return true;
                }
            }
            else if (recursiveIterator.Current is IEnumerator enumerator)
            {
                if (RecursiveMoveNext(enumerator))
                {
                    return true;
                }
            }
            else if (recursiveIterator.Current is T result)
            {
                Result = result;
            }

            bool moveNext = recursiveIterator.MoveNext();

            if (moveNext)
            {
                if (recursiveIterator.Current is IYieldInstruction || recursiveIterator.Current is IEnumerator)
                {
                    return RecursiveMoveNext(recursiveIterator);
                }

                return true;
            }

            return false;
        }
    }
}
