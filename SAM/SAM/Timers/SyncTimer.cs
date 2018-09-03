using System;

namespace SAM.Timers
{
    public class SyncTimer
    {
        public float Interval { get; set; }
        public bool Loop { get; set; }

        private float currentTime;
        private bool active;
        
        public event Action<SyncTimer> onTick;

        public SyncTimer()
        {

        }

        public void Start()
        {
            currentTime = 0;
            active = true;
        }

        public void Update(float elapsed)
        {
            if(active)
            {
                currentTime += elapsed;

                if (currentTime >= Interval)
                {
                    if (!Loop)
                    {
                        Stop();
                    }
                    else
                    {
                        currentTime = 0;
                    }

                    if (onTick != null)
                    {
                        onTick(this);
                    }
                }
            }
        }

        public void Stop()
        {
            active = false;
        }
    }
}
