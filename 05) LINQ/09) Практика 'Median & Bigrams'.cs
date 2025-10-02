using System;
using System.Collections.Generic;
using System.Linq;
namespace linq_slideviews;

public static class ExtensionsTask
{
    public static double Median(this IEnumerable<double> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        
        var sorted = items.OrderBy(x => x).ToArray(); // orderby
        if (sorted.Length == 0)
            throw new InvalidOperationException("Sequence contains no elements.");
        
        int mid = sorted.Length / 2;
        return (sorted.Length % 2 == 1) 
            ? sorted[mid] 
            : (sorted[mid - 1] + sorted[mid]) / 2;
    }

    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        T previous = default;
        bool isFirst = true;
        
        foreach (var current in items)
        {
            if (!isFirst)
                yield return (previous!, current);
            
            previous = current;
            isFirst = false;
        }
    }
}