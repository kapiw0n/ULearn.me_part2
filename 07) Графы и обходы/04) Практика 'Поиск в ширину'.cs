using System.Collections.Generic;
using System.Linq;
namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
        {
            var visited = new HashSet<Point> { start };
            var queue = InitializeQueue(start);
            var chestsSet = CreateChestsSet(chests);
            var foundPaths = new List<SinglyLinkedList<Point>>();

            ProcessQueue(map, queue, visited, chestsSet, foundPaths);
            return foundPaths;
        }

        private static Queue<SinglyLinkedList<Point>> InitializeQueue(Point start)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start));
            return queue;
		}

        private static HashSet<Point> CreateChestsSet(Chest[] chests)
        {
            return new HashSet<Point>(chests.Select(c => c.Location));
		}

        private static void ProcessQueue(
            Map map,
            Queue<SinglyLinkedList<Point>> queue,
            HashSet<Point> visited,
            HashSet<Point> chestsSet,
            List<SinglyLinkedList<Point>> foundPaths)
        {
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                HandleCurrentNode(current, chestsSet, foundPaths);
                if (ShouldStopProcessing(chestsSet)) break;
                ProcessNeighbors(map, current, queue, visited);
            }
		}

        private static void HandleCurrentNode(
            SinglyLinkedList<Point> current,
            HashSet<Point> chestsSet,
            List<SinglyLinkedList<Point>> foundPaths)
        {
            if (chestsSet.Contains(current.Value))
            {
                foundPaths.Add(current);
                chestsSet.Remove(current.Value);
            }
		}

        private static bool ShouldStopProcessing(HashSet<Point> chestsSet)
        {
            return chestsSet.Count == 0;
        }

        private static void ProcessNeighbors(
            Map map,
            SinglyLinkedList<Point> current,
            Queue<SinglyLinkedList<Point>> queue,
            HashSet<Point> visited)
        {
            foreach (var direction in Walker.PossibleDirections)
            {
                var nextPoint = current.Value + direction;
                if (IsValidNeighbor(map, nextPoint, visited))
                {
                    visited.Add(nextPoint);
                    queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, current));
                }
            }
		}

        private static bool IsValidNeighbor(Map map, Point nextPoint, HashSet<Point> visited)
        {
            return map.InBounds(nextPoint) &&
                   map.Dungeon[nextPoint.X, nextPoint.Y] != MapCell.Wall &&
                   !visited.Contains(nextPoint);
        }
    }
}