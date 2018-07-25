using System.Collections.Generic;

namespace SAM.Coroutines
{
    public sealed class CoroutineNest : YieldInstruction
    {
        private List<Coroutine> coroutines;
        private List<Coroutine> removed;
        
        /// <summary>
        /// if is set on true the UpdateAsRoot method does what the KeepWaiting property does, else the method does nothing.
        /// </summary>
        public bool isRoot;

        public int Count
        {
            get
            {
                return coroutines.Count - removed.Count;
            }
        }

        public override bool KeepWaiting
        {
            get
            {
                return AreCoroutinesOver();
            }
        }

        public CoroutineNest()
        {
            coroutines = new List<Coroutine>();
            removed = new List<Coroutine>();
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
            return coroutines.Contains(coroutine) && removed.Contains(coroutine) == false;
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
                removed.Add(coroutine);
            }
        }

        /// <summary>
        /// Removes all subscribed coroutines.
        /// </summary>
        public void RemoveAll()
        {
            for(int i = 0; i < coroutines.Count; ++i)
            {
                RemoveCoroutine(coroutines[i]);
            }
        }

        private void Flush()
        {
            foreach(Coroutine coroutine in removed)
            {
                coroutines.Remove(coroutine);
            }

            removed.Clear();
        }

        private bool AreCoroutinesOver()
        {
            if(Count > 0)
            {
                Flush();

                for (int i = 0; i < coroutines.Count; ++i)
                {
                    Coroutine current = coroutines[i];

                    if (removed.Contains(current) == false)
                    {
                        if (current.KeepWaiting == false)
                        {
                            RemoveCoroutine(current);
                        }
                    }
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
    }
}
