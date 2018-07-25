using System.Collections.Generic;

namespace SAM.Coroutines
{
    public sealed class CoroutineManager
    {
        private List<Coroutine> coroutines;

        public CoroutineManager()
        {
            coroutines = new List<Coroutine>();
        }

        /// <summary>
        /// Adds the given coroutine to the list or resumes a paused one. If the coroutine is added and running, it does nothing.
        /// </summary>
        /// <returns>The same coroutine running</returns>
        public Coroutine StartCoroutine(Coroutine coroutine)
        {
            if (coroutines.Contains(coroutine))
            {
                coroutine.paused = false;
            }
            else
            {
                coroutines.Add(coroutine);
            }

            coroutine.check = false;

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
        /// Stops all the existing coroutines. The coroutines will not be removed.
        /// </summary>
        public void StopAllCoroutines()
        {
            for(int i = 0; i < coroutines.Count; ++i)
            {
                coroutines[i].paused = true;
            }
        }

        /// <summary>
        /// Stops the given coroutine if it was found.
        /// </summary>
        /// <param name="coroutine"></param>
        public void StopCoroutine(Coroutine coroutine)
        {
            if(Contains(coroutine))
            {
                coroutine.paused = true;
            }
        }

        /// <summary>
        /// Removes the reference to the given coroutine if it was found.
        /// </summary>
        public void RemoveCoroutine(Coroutine coroutine)
        {
            coroutines.Remove(coroutine);
        }

        /// <summary>
        /// Removes all subscribed coroutines.
        /// </summary>
        public void RemoveAll()
        {
            coroutines.Clear();
        }

        /// <summary>
        /// Updates the stored coroutines
        /// </summary>
        public void Update()
        {
            foreach(Coroutine c in coroutines)
            {
                c.check = false;
            }

            for(int i = 0; i < coroutines.Count; ++i)
            {
                int currentCount = coroutines.Count;

                Coroutine current = coroutines[i];

                //if the coroutine has ended, it is removed
                if (current.check == false && current.KeepWaiting == false)
                {
                    RemoveCoroutine(current);
                }

                current.check = true;

                if(currentCount != coroutines.Count)
                {
                    i = -1;
                }
            }
        }
    }
}
