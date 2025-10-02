public static int[] ParseNumbers(IEnumerable<string> lines)
{
	return lines
		.Where(s => s.Length > 0)
		.Select(s => int.Parse(s))
		.ToArray();
}