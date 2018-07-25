using System.Collections;

namespace SAM.Coroutines
{
    public sealed class Coroutine : YieldInstruction
    {
        private IEnumerator iterator;
        public bool paused;

        public Coroutine(IEnumerator _iterator)
        {
            iterator = _iterator;
        }

        public override bool KeepWaiting
        {
            get
            {
                if(paused)
                {
                    return true;
                }
                else
                {
                    return RecursiveMoveNext(iterator);
                }
            }
        }
        
        private bool RecursiveMoveNext(IEnumerator recursiveIterator)
        {
            if (recursiveIterator.Current is YieldInstruction)
            {
                if (((YieldInstruction)recursiveIterator.Current).KeepWaiting)
                {
                    return true;
                }
            }
            else if (recursiveIterator.Current is IEnumerator)
            {
                if (RecursiveMoveNext((IEnumerator)recursiveIterator.Current))
                {
                    return true;
                }
            }

            bool moveNext = recursiveIterator.MoveNext();

            if(moveNext)
            {
                if (recursiveIterator.Current is YieldInstruction || recursiveIterator.Current is IEnumerator)
                {
                    return RecursiveMoveNext(recursiveIterator);
                }

                return true;
            }

            return false;
        }

        public void Override(IEnumerator _iterator)
        {
            iterator = _iterator;
        }
    }
}
