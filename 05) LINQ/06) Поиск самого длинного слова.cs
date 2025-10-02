public static string GetLongest(IEnumerable<string> words)
{
	return words.Min(word => Tuple.Create(-word.Length, word)).Item2;
}