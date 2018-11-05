using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    // Simple implementation of a concurrent list with locking
    public class ConcurrentList<TValue> : IList<TValue>
    {
        private object _lock = new object();
        private List<TValue> _storage = new List<TValue>();

        public TValue this[int index]
        {
            get
            {
                lock (_lock)
                {
                    if (index < 0)
                    {
                        index = this.Count - index;
                    }
                    return _storage[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    if (index < 0)
                    {
                        index = this.Count - index;
                    }
                    _storage[index] = value;
                }
            }
        }

        public void Sort()
        {
            lock (_lock)
            {
                _storage.Sort();
            }
        }

        public int Count
        {
            get
            {
                return _storage.Count;
            }
        }

        bool ICollection<TValue>.IsReadOnly
        {
            get
            {
                return ((IList<TValue>)_storage).IsReadOnly;
            }
        }

        public void Add(TValue item)
        {
            lock (_lock)
            {
                _storage.Add(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _storage.Clear();
            }
        }

        public bool Contains(TValue item)
        {
            lock (_lock)
            {
                return _storage.Contains(item);
            }
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _storage.CopyTo(array, arrayIndex);
            }
        }


        public int IndexOf(TValue item)
        {
            lock (_lock)
            {
                return _storage.IndexOf(item);
            }
        }

        public void Insert(int index, TValue item)
        {
            lock (_lock)
            {
                _storage.Insert(index, item);
            }
        }

        public bool Remove(TValue item)
        {
            lock (_lock)
            {
                return _storage.Remove(item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                _storage.RemoveAt(index);
            }
        }

        public List<TValue> ToArray()
        {
            lock (_lock)
            {
                return new List<TValue>(_storage);
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {

            lock (_lock)
            {
                lock (_lock)
                {
                    return (IEnumerator<TValue>)_storage.ToArray().GetEnumerator();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
