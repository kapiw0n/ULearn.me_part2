public static (string, int)[] GetMostFrequentWords(string text, int count)
{
	return Regex.Split(text.ToLower(), @"\W+")
		.Where(word => word != "")
		.GroupBy(word => word)
		.Select(group => (group.Key, group.Count()))
		.OrderByDescending(group => group.Item2)
		.ThenBy(group => group.Item1)
		.Take(count)
		.ToArray();
}