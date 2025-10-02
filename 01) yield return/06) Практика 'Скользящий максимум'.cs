using System.Collections.Generic;

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> sequence, int slidingWindowLength)
        {
            // linked list
            var maxCandidates = new LinkedList<DataPoint>();
            var currentWindow = new Queue<DataPoint>();
            int elementsInWindow = 0;

            foreach (var dataItem in sequence)
            {
                // new element to the window
                currentWindow.Enqueue(dataItem);
                UpdateMaxCandidates(maxCandidates, dataItem);
                elementsInWindow++;

                // maintain window size
                if (elementsInWindow > slidingWindowLength)
                {
                    RemoveExpiredElement(currentWindow, maxCandidates);
                    elementsInWindow--;
                }

                // current maximum
                yield return dataItem.WithMaxY(maxCandidates.First!.Value.OriginalY);
            }
        }

        // maintains the deque
        private static void UpdateMaxCandidates(LinkedList<DataPoint> candidates, DataPoint newPoint)
        {
            while (candidates.Count > 0 && candidates.Last!.Value.OriginalY <= newPoint.OriginalY)
            {
                candidates.RemoveLast();
            }

            candidates.AddLast(newPoint);
        }

        // removes expired element
        private static void RemoveExpiredElement(Queue<DataPoint> window, LinkedList<DataPoint> candidates)
        {
            var oldestPoint = window.Dequeue();
            if (candidates.First!.Value == oldestPoint)
            {
                candidates.RemoveFirst();
            }
        }
    }
}