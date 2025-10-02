using System;
using System.Collections.Generic;
using System.Linq;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        // List to store items
        private readonly List<T> items = new List<T>();
        // Object for synchronization
        private readonly object lockObject = new object();

        // Ind
        public T this[int index]
        {
            get
            {
                lock (lockObject) 
                {
                    if (index < 0 || index >= items.Count)
                        return null; // 
                    return items[index]; 
                }
            }
            set
            {
                if (value == null) 
                    return;

                lock (lockObject) 
                {
                    if (index < 0)
                        return; // Ignore if index is negative

                    if (index == items.Count) 
                    {
                        items.Add(value);
                    }
                    else if (index < items.Count) 
                    {
                        items[index] = value; // Replace item at the specified index
                        if (index + 1 < items.Count) // Remove items after the replaced item
                            items.RemoveRange(index + 1, items.Count - index - 1);
                    }
                }
            }
        }

        // get the last item
        public T LastItem()
        {
            lock (lockObject)
            {
                return items.LastOrDefault(); 
            }
        }

        // Append item 
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            if (item == null)
                return;

            lock (lockObject) 
            {
                T currentLastItem = items.LastOrDefault();
                if (ReferenceEquals(currentLastItem, knownLastItem) || 
                    (!ReferenceEquals(currentLastItem, null) && currentLastItem.Equals(knownLastItem)))
                {
                    items.Add(item); 
                }
            }
        }

        // Property to get the count of items
        public int Count
        {
            get
            {
                lock (lockObject) 
                {
                    return items.Count; 
                }
            }
        }
    }
}