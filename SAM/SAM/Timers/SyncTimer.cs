using System;

namespace SAM.Timers
{
    public class SyncTimer
    {
        public float Interval { get; set; }
        public bool Loop { get; set; }

        private float currentTime;

        private bool active;

        public bool Active
        {
            get
            {
                return active && !Paused;
            }
        }
        public bool Paused { get; private set; }
        
        public event Action<SyncTimer> onTick;

        public SyncTimer()
        {

        }

        public void Start()
        {
            currentTime = 0;
            active = true;
            Paused = false;
        }

        public void Update(float elapsed)
        {
            if(Active)
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
            UnPause();
        }

        public void Pause()
        {
            Paused = true;
        }

        public void UnPause()
        {
            Paused = false;
        }
    }
}
