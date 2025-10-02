private static IEnumerable<T> Take<T>(IEnumerable<T> source, int count)
{
    return source.Take(count);
}