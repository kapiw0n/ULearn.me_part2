public static IEnumerable<Edge> FindMinimumSpanningTree(IEnumerable<Edge> edges)
{
    var tree = new List<Edge>();
	foreach (var edge in edges.OrderBy(x => x.Weight))
		if (!HasCycle(new List<Edge>(tree){edge}))
			tree.Add(edge);
	return tree;
}