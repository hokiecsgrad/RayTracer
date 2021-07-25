using System;
using System.IO;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class Projectile
    {
        public Point Position { get; set; }
        public Vector Velocity { get; set; }
    }

    public class Environment
    {
        public Vector Gravity { get; set; }
        public Vector Wind { get; set; }
    }

    public class ProjectileTests : TestBase
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
            
            while (cannonBall.Position.y > 0)
                cannonBall = Tick(cannonBall, earth);

            Console.WriteLine($"CannonBall traveled to position {cannonBall.Position.x}");
        }

        [Fact]
        public void OutputBasicProjectileToImageFile()
        {
            var cannonBall = new Projectile { Position = new Point(0, 1, 0), 
                                              Velocity = new Vector(1, 1.8, 0).Normalize() * 11.25 };
            var earth = new Environment { Gravity = new Vector(0, -0.1, 0), 
                                          Wind = new Vector(-0.01, 0, 0) };
            var canvas = new Canvas(900, 550);


            string filename = imagePath.ToString() + "Projectile.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);


            while (cannonBall.Position.y > 0)
            {
                canvas.SetPixel((int)Math.Round(cannonBall.Position.x), 
                                canvas.Height - (int)Math.Round(cannonBall.Position.y), 
                                new Color(1,0,0));
                cannonBall = Tick(cannonBall, earth);
            }

            Console.WriteLine($"CannonBall traveled to position {cannonBall.Position.x}");
            
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}