using System;
using System.Collections;
using System.Collections.Generic;

namespace SAM.Collections
{
    public struct ValuePair<T, U>
    {
        public T Value1 { get; set; }
        public U Value2 { get; set; }

        public ValuePair(T v1, U v2)
        {
            Value1 = v1;
            Value2 = v2;
        }
    }

    public class UniquePairMap<T, U> : ICollection<ValuePair<T, U>>
    {
        private List<ValuePair<T, U>> values;

        public UniquePairMap()
        {
            values = new List<ValuePair<T, U>>();
        }

        public int Count
        {
            get
            {
                return values.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T GetFirstBySecond(U value)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ValuePair<T, U> valuePair = values[i];

                if (valuePair.Value2.Equals(value))
                {
                    return valuePair.Value1;
                }
            }

            throw new ArgumentException(value + " Value not found");
        }

        public U GetSecondByFirst(T value)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ValuePair<T, U> valuePair = values[i];

                if (valuePair.Value1.Equals(value))
                {
                    return valuePair.Value2;
                }
            }

            throw new ArgumentException(value + " Value not found");
        }

        public void SetFirstBySecond(U referenceValue, T overrideValue)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ValuePair<T, U> valuePair = values[i];

                if (valuePair.Value2.Equals(referenceValue))
                {
                    values[i] = new ValuePair<T, U>(overrideValue, valuePair.Value2);
                }
            }

            throw new ArgumentException(referenceValue + " Value not found");
        }

        public void SetSecondByFirst(T referenceValue, U overrideValue)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ValuePair<T, U> valuePair = values[i];

                if (valuePair.Value1.Equals(referenceValue))
                {
                    values[i] = new ValuePair<T, U>(valuePair.Value1, overrideValue);
                }
            }

            throw new ArgumentException(referenceValue + " Value not found");
        }

        public void Add(ValuePair<T, U> item)
        {
            foreach (ValuePair<T, U> val in values)
            {
                if (val.Value1.Equals(item.Value1))
                {
                    throw new ArgumentException("value 1 " + val.Value1.ToString() + " already added to collection");
                }
                else if (val.Value2.Equals(item.Value2))
                {
                    throw new ArgumentException("value 2 " + val.Value2.ToString() + " already added to collection");
                }
            }

            values.Add(item);
        }

        public void Add(T v1, U v2)
        {
            Add(new ValuePair<T, U>(v1, v2));
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(ValuePair<T, U> item)
        {
            return values.Contains(item);
        }

        public bool ContainsValue(T value)
        {
            foreach (ValuePair<T, U> v in values)
            {
                if (v.Value1.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsValue(U value)
        {
            foreach (ValuePair<T, U> v in values)
            {
                if (v.Value2.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(ValuePair<T, U>[] array, int arrayIndex)
        {
            values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ValuePair<T, U>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public bool Remove(ValuePair<T, U> item)
        {
            return values.Remove(item);
        }

        public bool Remove(T value)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ValuePair<T, U> v = values[i];

                if (v.Value1.Equals(value))
                {
                    return Remove(v);
                }
            }

            return false;
        }

        public bool Remove(U value)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ValuePair<T, U> v = values[i];

                if (v.Value2.Equals(value))
                {
                    return Remove(v);
                }
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

