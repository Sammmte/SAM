namespace SAM
{
    public class StructReference<T> where T : struct
    {
        public T innerStruct;

        public StructReference(T value)
        {
            innerStruct = value;
        }
    }
}
