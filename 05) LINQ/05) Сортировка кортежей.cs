public static List<string> GetSortedWords(string text)
{
    return text
        .Split()
        .Select(w => w.ToLower().Trim(new[] { ',', '.' }))
        .Distinct()
        .Select(w => Tuple.Create(w.Length, w))
        .OrderBy(w => w)
		.Select(w => w.Item2)
		.Where(w => w != string.Empty)
        .ToList();
}