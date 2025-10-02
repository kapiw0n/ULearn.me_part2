using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var (startPaths, exitPaths) = CalculatePaths(map);
            var validChests = GetValidChests(map, startPaths, exitPaths);

            return validChests.Count > 0 
                ? BuildPathThroughChest(map, ChooseBestChest(validChests)) 
                : GetDirectExitPath(map);
        }

        private static (Dictionary<Point, int>, Dictionary<Point, int>) CalculatePaths(Map map)
        {
            var startPaths = BfsTask.FindPaths(map, map.InitialPosition, map.Chests)
                .ToDictionary(p => p.Value, p => p.Length - 1);
            
            var exitPaths = BfsTask.FindPaths(map, map.Exit, map.Chests)
                .ToDictionary(p => p.Value, p => p.Length - 1);

            return (startPaths, exitPaths);
        }

        private static List<(int length, byte value, Point point)> GetValidChests(
            Map map, 
            Dictionary<Point, int> startPaths,
            Dictionary<Point, int> exitPaths)
        {
            return map.Chests
                .Select(chest => CheckChestAccess(chest, startPaths, exitPaths))
                .Where(t => t.isAccessible)
                .Select(t => (
                    t.pathLength, 
                    t.chest.Value, 
                    t.chest.Location
                ))
                .ToList();
        }

        private static (Chest chest, bool isAccessible, int pathLength) CheckChestAccess(
            Chest chest,
            Dictionary<Point, int> startPaths,
            Dictionary<Point, int> exitPaths)
        {
            var hasStart = startPaths.TryGetValue(chest.Location, out var toChest);
            var hasExit = exitPaths.TryGetValue(chest.Location, out var fromChest);
            return (
                chest,
                isAccessible: hasStart && hasExit,
                pathLength: hasStart && hasExit ? toChest + fromChest : int.MaxValue
            );
        }

        private static (int length, byte value, Point point) ChooseBestChest(
            List<(int length, byte value, Point point)> chests)
        {
            return chests
                .OrderBy(c => c.length)
                .ThenByDescending(c => c.value)
                .First();
        }

        private static MoveDirection[] BuildPathThroughChest(Map map, (int, byte, Point) chestInfo)
        {
            var (_, _, chestPoint) = chestInfo;
            var pathToChest = BfsTask.FindPaths(map, map.InitialPosition, new[] { new Chest(chestPoint, 0) }).First();
            var pathFromChest = BfsTask.FindPaths(map, chestPoint, new[] { new Chest(map.Exit, 0) }).First();
            
            return CombinePaths(pathToChest, pathFromChest);
        }

        private static MoveDirection[] CombinePaths(
            SinglyLinkedList<Point> pathToPoint,
            SinglyLinkedList<Point> pathFromPoint)
        {
            var pathToChest = ConvertPathToDirections(pathToPoint);
            var pathFromChest = ConvertPathToDirections(pathFromPoint);
            
            return pathToChest.Concat(pathFromChest).ToArray();
        }

        private static MoveDirection[] GetDirectExitPath(Map map)
        {
            var path = BfsTask.FindPaths(map, map.InitialPosition, new[] { new Chest(map.Exit, 0) })
                .FirstOrDefault();
            return path != null 
                ? ConvertPathToDirections(path) 
                : Array.Empty<MoveDirection>();
        }

        private static MoveDirection[] ConvertPathToDirections(SinglyLinkedList<Point> path)
        {
            return path.Reverse()
                .Zip(path.Reverse().Skip(1), (a, b) => Walker.ConvertOffsetToDirection(b - a))
                .ToArray();
        }
    }
}