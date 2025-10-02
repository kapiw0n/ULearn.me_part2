using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class DijkstraData
    {
        public Architecture.Point? CameFrom { get; init; } // Previous node
        public int TotalCost { get; init; } // Path cost
    }

    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(
            State state,
            Architecture.Point start,
            IEnumerable<Architecture.Point> targets)
        {
            var targetPoints = new HashSet<Architecture.Point>(targets);
            var openSet = new HashSet<Architecture.Point> { start }; // Nodes to explore
            var closedSet = new HashSet<Architecture.Point>(); // Visited nodes
            var pathData = InitializePathData(start);

            while (openSet.Count > 0)
            {
                var currentNode = FindCheapestNode(openSet, pathData); // Min cost node
                if (!currentNode.HasValue) break;

                if (TryProcessTargetNode(currentNode.Value, targetPoints, pathData, out var path))
                    yield return path;

                if (ShouldTerminateSearch(targetPoints)) yield break;

                ProcessNeighbors(currentNode.Value, state, pathData, openSet, closedSet);
                UpdateNodeSets(currentNode.Value, openSet, closedSet); // Move to visited
            }
        }
//all other idk
        private Dictionary<Architecture.Point, DijkstraData> InitializePathData(Architecture.Point start)
        {
            return new Dictionary<Architecture.Point, DijkstraData>
            {
                [start] = new DijkstraData { TotalCost = 0, CameFrom = null }
            };
        }

        private bool TryProcessTargetNode(
            Architecture.Point node,
            HashSet<Architecture.Point> targets,
            Dictionary<Architecture.Point, DijkstraData> pathData,
            out PathWithCost path)
        {
            path = null;
            if (!targets.Contains(node)) return false;

            path = ReconstructPath(pathData, node);
            targets.Remove(node);
            return true;
        }

        private bool ShouldTerminateSearch(HashSet<Architecture.Point> targets)
        {
            return targets.Count == 0;
        }

        private void UpdateNodeSets(
            Architecture.Point node,
            HashSet<Architecture.Point> openSet,
            HashSet<Architecture.Point> closedSet)
        {
            openSet.Remove(node);
            closedSet.Add(node);
        }

        private Architecture.Point? FindCheapestNode(
            HashSet<Architecture.Point> openSet,
            Dictionary<Architecture.Point, DijkstraData> pathData)
        {
            Architecture.Point? result = null;
            var minCost = int.MaxValue;
            
            foreach (var node in openSet)
            {
                if (pathData.TryGetValue(node, out var data) && data.TotalCost < minCost)
                {
                    minCost = data.TotalCost;
                    result = node;
                }
            }
            return result;
        }

        private void ProcessNeighbors(
            Architecture.Point current,
            State state,
            Dictionary<Architecture.Point, DijkstraData> pathData,
            HashSet<Architecture.Point> openSet,
            HashSet<Architecture.Point> closedSet)
        {
            foreach (var neighbor in GetAdjacentPoints(current, state))
            {
                if (closedSet.Contains(neighbor)) continue;

                var tentativeCost = pathData[current].TotalCost + state.CellCost[neighbor.X, neighbor.Y];
                if (!pathData.ContainsKey(neighbor) || tentativeCost < pathData[neighbor].TotalCost)
                {
                    pathData[neighbor] = new DijkstraData { TotalCost = tentativeCost, CameFrom = current };
                    openSet.Add(neighbor);
                }
            }
        }

        private IEnumerable<Architecture.Point> GetAdjacentPoints(
            Architecture.Point position,
            State mapState)
        {
            return new[]
            {
                new Architecture.Point(position.X, position.Y + 1),
                new Architecture.Point(position.X + 1, position.Y),
                new Architecture.Point(position.X, position.Y - 1),
                new Architecture.Point(position.X - 1, position.Y)
            }.Where(p => mapState.InsideMap(p) && !mapState.IsWallAt(p));
        }

        private PathWithCost ReconstructPath(
            Dictionary<Architecture.Point, DijkstraData> pathData,
            Architecture.Point end)
        {
            var pathSteps = new LinkedList<Architecture.Point>();
            var currentStep = end;
            
            while (true)
            {
                pathSteps.AddFirst(currentStep);
                var previous = pathData[currentStep].CameFrom;
                if (!previous.HasValue) break;
                currentStep = previous.Value;
            }

            return new PathWithCost(pathData[end].TotalCost, pathSteps.ToArray());
        }
    }
}