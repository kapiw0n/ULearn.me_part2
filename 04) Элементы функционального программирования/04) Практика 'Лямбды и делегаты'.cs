using System;
namespace func_rocket;
public class ForcesTask
{
    public static RocketForce GetThrustForce(double forceValue) => rocket =>
    {
        var thrustDirection = new Vector(Math.Cos(rocket.Direction), Math.Sin(rocket.Direction));
        return thrustDirection * forceValue;
    };
//check gravity force
    public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize) => rocket =>
    {
        return gravity(spaceSize, rocket.Location);
    };

    public static RocketForce Sum(params RocketForce[] forces) => rocket =>
    {
        Vector totalForce = Vector.Zero;
        foreach (var force in forces)
        {
            totalForce += force(rocket); //sum all forces
        }
        return totalForce;
    };
}