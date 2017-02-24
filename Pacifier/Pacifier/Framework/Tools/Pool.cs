using System;
using System.Collections.Generic;

namespace CloudColony.Framework.Tools
{
    public interface IPoolable
    {
        void Reset();
    }

    /**
     * This fun little class keeps the Garbage collector happy! :D
     * It uses a pool that we can pick up objects from and then put them back after use to avoid heavy allocation
     */
    public abstract class Pool<T>
    {
        private List<T> _available = new List<T>();
        private List<T> _inUse = new List<T>();

        public T GetObject()
        {
            lock (_available)
            {
                if (_available.Count != 0)
                {
                    T po = _available[0];
                    _inUse.Add(po);
                    _available.RemoveAt(0);
                    return po;
                }
                else
                {
                    T po = newObject();
                    _inUse.Add(po);
                    return po;
                }
            }
        }

        public abstract T newObject();


        public void ReleaseObject(T obj)
        {
            Reset(obj);

            lock (_available)
            {
                _available.Add(obj);
                _inUse.Remove(obj);
            }
        }

        private void Reset(T obj)
        {
            if (obj is IPoolable)
                ((IPoolable)obj).Reset();
            else
                throw new Exception("The object is not typeof @Poolable");
        }
    }
}
