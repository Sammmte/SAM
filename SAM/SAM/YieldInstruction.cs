namespace SAM.Coroutines
{
    public abstract class YieldInstruction
    {
        /// <summary>
        /// Use this property to keep a coroutine suspended returning true. To let coroutine proceed, return false.
        /// </summary>
        public abstract bool KeepWaiting
        {
            get;
        }
    }
}
