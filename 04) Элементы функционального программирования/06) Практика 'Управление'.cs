using System;
namespace func_rocket
{
    public class ControlTask
    {
        public static double OverallAngle;
//control
        public static Turn ControlRocket(Rocket rocket, Vector targetPosition)
        {
			var directionVector = new Vector(targetPosition.X - rocket.Location.X,
targetPosition.Y - rocket.Location.Y);

            double angleDifference = directionVector.Angle - rocket.Direction;
            double velocityAngleDifference = directionVector.Angle - rocket.Velocity.Angle;

            if (GetAbsoluteValue(angleDifference) < 0.5 || GetAbsoluteValue(velocityAngleDifference) < 0.5)
            {
                OverallAngle = (angleDifference + velocityAngleDifference) / 2;
            }
            else
            {
                OverallAngle = angleDifference;
            }

            if (OverallAngle < 0)
                return Turn.Left;
            return OverallAngle > 0 ? Turn.Right : Turn.None;
        }

        private static double GetAbsoluteValue(double value)
        {
            return value < 0 ? -value : value;
        }
    }
}