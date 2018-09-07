using System.Collections;
using System.Collections.Generic;

namespace SAM.Coroutines
{
    public sealed class CoroutineNest : IYieldInstruction, IEnumerable<Coroutine>
    {
        private List<Coroutine> coroutines;
        
        /// <summary>
        /// if is set on true the UpdateAsRoot method does what the KeepWaiting property does, else the method does nothing.
        /// </summary>
        public bool isRoot;

        public int Count
        {
            get
            {
                return coroutines.Count;
            }
        }

        public bool KeepWaiting
        {
            get
            {
                return AreCoroutinesOver();
            }
        }

        private int currentIndex;

        public CoroutineNest()
        {
            coroutines = new List<Coroutine>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_isRoot">if is set on true the UpdateAsRoot method does what the KeepWaiting property does, else the method does nothing.</param>
        public CoroutineNest(bool _isRoot) : this()
        {
            isRoot = _isRoot;
        }

        /// <summary>
        /// Adds a new coroutine. Note that the same coroutine could be loaded twice.
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public Coroutine AddCoroutine(Coroutine coroutine)
        {
            coroutines.Add(coroutine);

            return coroutine;
        }

        /// <summary>
        /// Returns true if the given coroutine exists.
        /// </summary>
        public bool Contains(Coroutine coroutine)
        {
            return coroutines.Contains(coroutine);
        }

        /// <summary>
        /// Pauses all subscribed coroutines. The coroutines will not be removed.
        /// </summary>
        public void PauseAll()
        {
            for (int i = 0; i < coroutines.Count; ++i)
            {
                if(Contains(coroutines[i]))
                {
                    coroutines[i].paused = true;
                }
            }
        }

        /// <summary>
        /// Pauses the given coroutine if it was found.
        /// </summary>
        /// <param name="coroutine"></param>
        public void PauseCoroutine(Coroutine coroutine)
        {
            if (Contains(coroutine))
            {
                coroutine.paused = true;
            }
        }

        /// <summary>
        /// Resumes the given coroutine if it was found.
        /// </summary>
        /// <param name="coroutine"></param>
        public void ResumeCoroutine(Coroutine coroutine)
        {
            if(Contains(coroutine))
            {
                coroutine.paused = false;
            }
        }

        /// <summary>
        /// Resumes all subscribed coroutines.
        /// </summary>
        public void ResumeAll()
        {
            for (int i = 0; i < coroutines.Count; ++i)
            {
                if (Contains(coroutines[i]))
                {
                    coroutines[i].paused = false;
                }
            }
        }

        /// <summary>
        /// Removes the reference to the given coroutine if it was found.
        /// </summary>
        public void RemoveCoroutine(Coroutine coroutine)
        {
            if(Contains(coroutine))
            {
                coroutines.Remove(coroutine);
                currentIndex--;
            }
        }

        /// <summary>
        /// Removes all subscribed coroutines.
        /// </summary>
        public void RemoveAll()
        {
            coroutines.Clear();
            currentIndex = 0;
        }

        private bool AreCoroutinesOver()
        {
            for (currentIndex = 0; currentIndex < coroutines.Count; currentIndex++)
            {
                Coroutine current = coroutines[currentIndex];

                if (current.KeepWaiting == false)
                {
                    RemoveCoroutine(current);
                }
            }

            return Count > 0;
        }

        /// <summary>
        /// if isRoot is set on true this method does what the KeepWaiting property does, else does nothing. 
        /// Note that yield a root CoroutineNest and update it would execute the coroutines twice in a frame.
        /// </summary>
        public void UpdateAsRoot()
        {
            if(isRoot)
            {
                AreCoroutinesOver();
            }
        }

        public IEnumerator<Coroutine> GetEnumerator()
        {
            return coroutines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
