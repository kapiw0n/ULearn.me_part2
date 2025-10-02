public static ILookup<string, int> BuildInvertedIndex(Document[] documents)
{
	return documents
              .SelectMany(d => Regex.Split(d.Text.ToLower(), @"\W+")
                  .Where(word => word != "")
                  .Distinct()
                  .Select(x => (d.Id, woord:x)))
              .ToLookup(x => x.woord, x => x.Id);
}