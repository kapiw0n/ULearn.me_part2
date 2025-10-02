public static int LevenshteinDistance(string first, string second)
{
	var opt = new int[first.Length + 1, second.Length + 1];
	for (var i = 0; i <= first.Length; ++i)
		opt[i, 0] = i;
	for (var i = 0; i <= second.Length; ++i)
		opt[0, i] = i;
	for (var i = 1; i <= first.Length; ++i)
		for (var j = 1; j <= second.Length; ++j)
		{
			opt[i, j] = opt[i - 1, j - 1];
			if (first[i - 1] != second[j - 1])
			{
				opt[i, j] = Math.Min(opt[i - 1, j], opt[i, j]);
				opt[i, j] = Math.Min(opt[i, j - 1], opt[i, j]);
				opt[i, j]++;
			}
		}
	return opt[first.Length, second.Length];
}