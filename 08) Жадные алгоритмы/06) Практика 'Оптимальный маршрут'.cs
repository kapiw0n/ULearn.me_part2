using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;
using Point = Greedy.Architecture.Point; // Фикс будет потом щас настроения нету

namespace Greedy
{
    public class NotGreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var pathFinder = new DijkstraPathFinder();
            var currentPath = new Stack<PathWithCost>();
            var remainingChests = new HashSet<Point>(state.Chests);
            var pathCache = new Dictionary<(Point start, Point end), PathWithCost>();
            List<PathWithCost> bestFoundPath = null;

            foreach (var chest in state.Chests)
            {
                var result = FindPathFromChest(
                    remainingChests,
                    state.Position,
                    chest,
                    0,
                    state,
                    currentPath,
                    pathCache,
                    pathFinder,
                    ref bestFoundPath);

                if (result != null)
                    return result;
            }

            return bestFoundPath != null ? BuildFullPath(bestFoundPath) : new List<Point>();
        }

        private List<Point> FindPathFromChest(
            HashSet<Point> remainingChests,
            Point previousPosition,
            Point currentChest,
            int accumulatedCost,
            State state,
            Stack<PathWithCost> currentPath,
            Dictionary<(Point start, Point end), PathWithCost> pathCache,
            DijkstraPathFinder pathFinder,
            ref List<PathWithCost> bestFoundPath)
        {
            var pathToChest = GetPathToPoint(
                previousPosition,
                currentChest,
                pathCache,
                state,
                pathFinder);

            if (pathToChest == null)
                return null;

            accumulatedCost += pathToChest.Cost;
            remainingChests.Remove(currentChest);
            currentPath.Push(pathToChest);

            if (accumulatedCost <= state.Energy && remainingChests.Count > 0)
            {
                foreach (var nextChest in remainingChests.ToList())
                {
                    var result = FindPathFromChest(
                        remainingChests,
                        currentChest,
                        nextChest,
                        accumulatedCost,
                        state,
                        currentPath,
                        pathCache,
                        pathFinder,
                        ref bestFoundPath);

                    if (result != null)
                        return result;
                }
            }
            else if (accumulatedCost <= state.Energy && remainingChests.Count == 0)
            {
                return BuildFullPath(currentPath.ToList());
            }

            remainingChests.Add(currentChest);
            currentPath.Pop();
            UpdateBestPath(ref bestFoundPath, currentPath);

            return null;
        }

        private void UpdateBestPath(ref List<PathWithCost> bestPath, Stack<PathWithCost> currentPath)
        {
            if (bestPath == null || bestPath.Count < currentPath.Count)
                bestPath = currentPath.ToList();
        }

        private List<Point> BuildFullPath(List<PathWithCost> pathSegments)
        {
            var fullPath = new List<Point>();
            for (int i = pathSegments.Count - 1; i >= 0; i--)
            {
                var segment = pathSegments[i];
                for (int j = 1; j < segment.Path.Count; j++)
                    fullPath.Add(segment.Path[j]);
            }
            return fullPath;
        }

        private PathWithCost GetPathToPoint(
            Point from,
            Point to,
            Dictionary<(Point start, Point end), PathWithCost> pathCache,
            State state,
            DijkstraPathFinder pathFinder)
        {
            var key = (from, to);
            if (pathCache.TryGetValue(key, out var cachedPath))
                return cachedPath;

            var path = pathFinder.GetPathsByDijkstra(state, from, new[] { to }).FirstOrDefault();
            pathCache[key] = path;
            return path;
        }
    }
}