using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    public class BestKeeper<T> : IReadOnlyCollection<T>
    {
        readonly T[] _items;
        private int _count;
        private readonly Comparer<T> _comp;

        public BestKeeper(int capacity, Comparison<T> comparator)
            : this(capacity, Comparer<T>.Create(comparator))
        {
        }

        public BestKeeper(int capacity, Comparer<T> comparator)
        {
            _items = new T[capacity];
            _comp = comparator;
        }

        public bool AddCandidate(T candidate)
        {
            int index = Array.BinarySearch(_items, 0, _count, candidate, _comp);

            if (index < 0) index = ~index;
            if (index >= Capacity) return false;

            int moveLength = Count - index - 1;
            if (moveLength > 0) Array.Copy(_items, index, _items, index + 1, moveLength);
            _items[index] = candidate;
            if (_count < Capacity) _count++;
            return true;

        }

        public int Capacity => _items.Length;
        public int Count => _count;

        public IEnumerator<T> GetEnumerator() => _items.Take(_count).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
