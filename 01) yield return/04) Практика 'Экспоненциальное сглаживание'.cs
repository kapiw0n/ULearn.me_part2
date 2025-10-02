using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> inputSequence,
double smoothingFactor)
        {
            // stores prev data
            double? previousResult = null;

            // process
            foreach (var dataItem in inputSequence)
            {
                // calc new smoothed value
                var currentValue = previousResult.HasValue
                    ? smoothingFactor * dataItem.OriginalY + (1 - smoothingFactor) * previousResult.Value
                    : dataItem.OriginalY;

                previousResult = currentValue;

                // return datapoint
                yield return dataItem.WithExpSmoothedY(currentValue);
            }
        }
    }
}