public static int GetMinPathCost(List<Edge> edges, int startNode, int finalNode)
{
    var maxNodeIndex = edges.SelectMany(e => new[] { e.From, e.To })
                          .Concat(new[] { startNode, finalNode })
                          .Max();
    var opt = Enumerable.Repeat(int.MaxValue, maxNodeIndex + 1).ToArray();
    opt[startNode] = 0;
    for (var i = 0; i < maxNodeIndex; i++)
    {
        bool updated = false;
        foreach (var edge in edges)
        {
            if (opt[edge.From] != int.MaxValue && 
                opt[edge.To] > opt[edge.From] + edge.Cost)
            {
                opt[edge.To] = opt[edge.From] + edge.Cost;
                updated = true;
            }
        }
        if (!updated) break;
    }

    return opt[finalNode];
}