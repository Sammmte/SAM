namespace SAM.Coroutines
{
    public abstract class YieldInstruction
    {
        public abstract bool KeepWaiting
        {
            get;
        }
    }
}
