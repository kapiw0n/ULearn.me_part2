public static List<Point> ParsePoints(IEnumerable<string> lines)
{
	return lines
	.Select(line => line.Split()
	.Select(c => int.Parse(c))
	.ToArray())
	.Select(p => new Point(p[0], p[1]))
	.ToList();
}