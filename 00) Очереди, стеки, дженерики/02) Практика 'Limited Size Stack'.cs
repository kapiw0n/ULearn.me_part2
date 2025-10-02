using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class LimitedSizeStack<T>
    {
        private readonly int _maxSize;
        private readonly LinkedList<T> _stack;

        public LimitedSizeStack(int maxSize)
        {
            _maxSize = maxSize;
            _stack = new LinkedList<T>();
        }

        public void Push(T item)
        {
            if (_maxSize <= 0)
            {
                return;
            }

            if (_stack.Count >= _maxSize)
            {
                _stack.RemoveFirst();
            }

            _stack.AddLast(item);
        }

        public T Pop()
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            var value = _stack.Last.Value;
            _stack.RemoveLast();
            return value;
        }

        public int Count => _stack.Count;
    }
}