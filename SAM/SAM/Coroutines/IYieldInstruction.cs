namespace SAM.Coroutines
{
    public interface IYieldInstruction
    {
        /// <summary>
        /// Use this property to keep a coroutine suspended returning true. To let coroutine proceed, return false.
        /// </summary>
        bool KeepWaiting { get; }
    }
}
