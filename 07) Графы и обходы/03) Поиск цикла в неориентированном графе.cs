public static bool HasCycle(List<Node> graph)
{
    var visited = new HashSet<Node>();
    var finished = new HashSet<Node>();
    var stack = new Stack<Node>();
    visited.Add(graph.First());
    stack.Push(graph.First());
    while (stack.Count != 0)
    {
        var node = stack.Pop();
        foreach (var nextNode in node.IncidentNodes)
        {
			if(finished.Contains(nextNode)) continue;
			if (visited.Contains(nextNode)) return true;
            stack.Push(nextNode);
			visited.Add(nextNode);
        }
        finished.Add(node);
    }
    return false;
}