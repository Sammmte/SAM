using System;
using System.Collections;
using System.Collections.Generic;

namespace SAM.Collections
{
    public interface IPoolable
    {
        bool IsAvailable();
    }

    public class ObjectPool<T> : ICollection<T> where T : IPoolable
    {
        private List<T> objects;

        private Func<T> factoryMethod;

        public ObjectPool()
        {
            objects = new List<T>();
        }

        public int Count
        {
            get
            {
                return objects.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T GetFirstAvailable()
        {
            for(int i = 0; i < objects.Count; i++)
            {
                T obj = objects[i];

                if (obj.IsAvailable())
                {
                    return obj;
                }
            }

            return default;
        }

        public T[] GetAvailable(int count)
        {
            if(count < 1)
            {
                return null;
            }

            List<T> requested = null;

            for(int i = 0, j = 0; i < objects.Count && j < count; i++)
            {
                T obj = objects[i];

                if(obj.IsAvailable())
                {
                    if(requested == null)
                    {
                        requested = new List<T>();
                    }

                    requested.Add(obj);

                    j++;
                }
            }

            if(requested != null)
            {
                return requested.ToArray();
            }
            else
            {
                return null;
            }
        }

        public void SetFactoryMethod(Func<T> method)
        {
            factoryMethod = method;
        }

        public void Fill(int count, Func<T> method)
        {
            if (method == null)
                return;

            for(int i = 0; i < count; i++)
            {
                Add(method());
            }
        }

        public void Fill(int count)
        {
            if (factoryMethod == null)
                return;

            for(int i = 0; i < count; i++)
            {
                Add(factoryMethod());
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            objects.AddRange(collection);
        }

        public void RemoveRange(int beginIndex, int count)
        {
            objects.RemoveRange(beginIndex, count);
        }
        
        public void Add(T item)
        {
            if(objects.Contains(item))
            {
                throw new ArgumentException("object " + item.ToString() + " already added to pool");
            }
            else
            {
                objects.Add(item);
            }
        }

        public void Clear()
        {
            objects.Clear();
        }

        public bool Contains(T item)
        {
            return objects.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            objects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return objects.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
