using System;
using System.IO;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class Projectile
    {
        public RayTuple Position { get; set; }
        public RayTuple Velocity { get; set; }
    }

    public class Environment
    {
        public RayTuple Gravity { get; set; }
        public RayTuple Wind { get; set; }
    }

    public class ProjectileTests
    {
        public Projectile Tick(Projectile proj, Environment env)
        {
            var newPosition = proj.Position + proj.Velocity;
            var newVelocity = proj.Velocity + env.Gravity + env.Wind;
            return new Projectile { Position = newPosition, Velocity = newVelocity };
        }

        [Fact]
        public void CreateBasicProjectileWithEnvironment()
        {
            var cannonBall = new Projectile { Position = new Point(0, 1, 0), 
                                              Velocity = new Vector(1, 1, 0).Normalize() };
            var earth = new Environment { Gravity = new Vector(0, -0.1, 0), 
                                          Wind = new Vector(-0.01, 0, 0) };
            
            while (cannonBall.Position.Item2 > 0)
            {
                cannonBall = Tick(cannonBall, earth);
                Console.WriteLine(cannonBall.Position);
            }

            Console.WriteLine($"CannonBall traveled to position {cannonBall.Position.Item1}");
        }

        [Fact]
        public void OutputBasicProjectileToImageFile()
        {
            var cannonBall = new Projectile { Position = new Point(0, 1, 0), 
                                              Velocity = new Vector(1, 1.8, 0).Normalize() * 11.25 };
            var earth = new Environment { Gravity = new Vector(0, -0.1, 0), 
                                          Wind = new Vector(-0.01, 0, 0) };
            var canvas = new Canvas(900, 550);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/ProjectileSmokeTest.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);

            while (cannonBall.Position.Item2 > 0)
            {
                canvas.SetPixel((int)Math.Round(cannonBall.Position.Item1), 
                                canvas.Height - (int)Math.Round(cannonBall.Position.Item2), 
                                new Color(1,0,0));
                cannonBall = Tick(cannonBall, earth);
                Console.WriteLine(cannonBall.Position);
            }

            Console.WriteLine($"CannonBall traveled to position {cannonBall.Position.Item1}");
            
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}