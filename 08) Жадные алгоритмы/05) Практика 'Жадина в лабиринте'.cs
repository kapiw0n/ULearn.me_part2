using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;
//норм?
namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            if (state.Goal == 0)
                return new List<Point>();

            var pathData = new PathData
            {
                Finder = new DijkstraPathFinder(),
                RemainingChests = new HashSet<Point>(state.Chests),
                CurrentPosition = state.Position,
                ConsumedEnergy = 0
            };

            var accumulatedPath = new List<Point>();

            for (int targetCount = 0; targetCount < state.Goal; targetCount++)
            {
                if (!TryProcessNextTarget(state, pathData, accumulatedPath))
                    return new List<Point>();
            }
            
            return accumulatedPath;
        }

        private bool TryProcessNextTarget(State state, PathData pathData, List<Point> accumulatedPath)
        {
            if (!pathData.RemainingChests.Any())
                return false;

            var shortestPath = pathData.Finder
                .GetPathsByDijkstra(state, pathData.CurrentPosition, pathData.RemainingChests)
                .FirstOrDefault();

            if (shortestPath == null)
                return false;

            pathData.RemainingChests.Remove(shortestPath.End);
            pathData.CurrentPosition = shortestPath.End;
            pathData.ConsumedEnergy += shortestPath.Cost;

            if (pathData.ConsumedEnergy > state.Energy)
                return false;

            accumulatedPath.AddRange(shortestPath.Path.Skip(1));
            return true;
        }

        private class PathData
        {
            public DijkstraPathFinder Finder { get; set; }
            public HashSet<Point> RemainingChests { get; set; }
            public Point CurrentPosition { get; set; }
            public int ConsumedEnergy { get; set; }
        }
    }
}