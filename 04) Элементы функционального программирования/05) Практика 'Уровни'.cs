using System;
using System.Collections.Generic;
namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics defaultPhysics = new();

        public static IEnumerable<Level> CreateLevels()
        {
            yield return CreateZeroLevel();
            yield return CreateHeavyLevel();
            yield return CreateUpLevel();
            yield return CreateWhiteHoleLevel();
            yield return CreateBlackHoleLevel();
            yield return CreateBlackAndWhiteLevel();
        }
//zzero
        static Level CreateZeroLevel() => new("Zero",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (areaSize, position) => Vector.Zero, defaultPhysics);
//heavy
        static Level CreateHeavyLevel() => new("Heavy",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (areaSize, position) => new Vector(0, 0.9), defaultPhysics);
//up
        static Level CreateUpLevel() => new("Up",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(700, 500),
            (areaSize, position) => new Vector(0, -300 / (areaSize.Y - position.Y + 300.0)), defaultPhysics);
//whiyehole
        static Level CreateWhiteHoleLevel() => new("WhiteHole",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (areaSize, position) => {
                var directionToWhiteHole = (position - new Vector(600, 200)).Normalize();
                var distanceToWhiteHole = (position - new Vector(600, 200)).Length;
				return directionToWhiteHole * (140 * distanceToWhiteHole /
(distanceToWhiteHole * distanceToWhiteHole + 1));
            }, defaultPhysics);
//blackchole
        static Level CreateBlackHoleLevel() => new("BlackHole",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (areaSize, position) => {
                var blackHoleCenter = (new Vector(200, 500) + new Vector(600, 200)) / 2;
                var directionToBlackHole = (blackHoleCenter - position).Normalize();
                var distanceToBlackHole = (position - blackHoleCenter).Length;
				return directionToBlackHole * (300 * distanceToBlackHole /
(distanceToBlackHole * distanceToBlackHole + 1));
            }, defaultPhysics);
//b&w (refactor)
        static Level CreateBlackAndWhiteLevel() => new("BlackAndWhite",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (areaSize, position) => {
                var directionToWhiteHole = (position - new Vector(600, 200)).Normalize();
                var distanceToWhiteHole = (position - new Vector(600, 200)).Length;
				var whiteHoleEffect = directionToWhiteHole * (140 * distanceToWhiteHole /
(distanceToWhiteHole * distanceToWhiteHole + 1));
                
                var blackHoleCenter = (new Vector(200, 500) + new Vector(600, 200)) / 2;
                var directionToBlackHole = (blackHoleCenter - position).Normalize();
                var distanceToBlackHole = (position - blackHoleCenter).Length;
				var blackHoleEffect = directionToBlackHole * (300 * distanceToBlackHole /
(distanceToBlackHole * distanceToBlackHole + 1));
                
                return (whiteHoleEffect + blackHoleEffect) / 2;
            }, defaultPhysics);
    }
}