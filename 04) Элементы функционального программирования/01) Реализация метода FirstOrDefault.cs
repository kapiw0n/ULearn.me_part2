private static T FirstOrDefault<T>(IEnumerable<T> source, Func<T, bool> filter)
{
    if (source == null) throw new ArgumentNullException(nameof(source));
    if (filter == null) throw new ArgumentNullException(nameof(filter));
    
    foreach (var item in source) {
        if (filter(item))
            return item;
    }
    return default(T);
}