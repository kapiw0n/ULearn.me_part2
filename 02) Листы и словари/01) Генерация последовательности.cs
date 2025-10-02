public static IEnumerable<int> GenerateCycle(int maxValue)
{
    int current = 0;
    while (true)
    {
        yield return current;
        current = (current + 1) % maxValue;
	}
}