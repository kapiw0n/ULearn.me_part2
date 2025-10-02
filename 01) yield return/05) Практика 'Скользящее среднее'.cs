using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> dataStream, int averagingWindow)
        {
            // сircular buffer
            var valueBuffer = new Queue<double>();
            
            // accum for sum
            double cumulativeSum = 0;
            
            // track actual elements
            int currentElements = 0;

            foreach (var measurement in dataStream)
            {
                ProcessNewValue(measurement.OriginalY, valueBuffer, ref cumulativeSum, ref currentElements);
                MaintainWindowSize(averagingWindow, valueBuffer, ref cumulativeSum, ref currentElements);
                
                // calculate and return
                yield return measurement.WithAvgSmoothedY(CalculateAverage(cumulativeSum, currentElements));
            }
        }

        private static void ProcessNewValue(double newValue, Queue<double> buffer, ref double sum, ref int count)
        {
            // add new value
            buffer.Enqueue(newValue);
            sum += newValue;
            count++;
        }

        private static void MaintainWindowSize(int windowSize, Queue<double> buffer, ref double sum, ref int count)
        {
            // maintain fws
            if (count > windowSize)
            {
                sum -= buffer.Dequeue();
                count--;
            }
        }

        private static double CalculateAverage(double sum, int count)
        {
            return sum / count;
        }
    }
}