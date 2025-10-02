static T Max<T>(T[] source) where T : IComparable
{
	if(source.Length == 0)
		return default(T);
	return source.Max();
}