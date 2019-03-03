using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using RayTracer;

namespace RayTracer.Program
{
    class Program
    {
        public static void Render()
        {
            var floor = new Plane();
            floor.Transform = Transformation.Translation(0, -10, 0);
            floor.Material = new Material();
            floor.Material.Pattern = new Checkers(new Color(0,0,0), new Color(1,1,1));
            floor.Material.Ambient = 0.1;
            floor.Material.Diffuse = 0.9;
            floor.Material.Specular = 0.9;
            floor.Material.Shininess = 200;

            var group = new Group();
            var innerSphere = new Sphere();
            innerSphere.CastsShadow = false;
            innerSphere.Transform = Transformation.Scaling(0.5, 0.5, 0.5);
            innerSphere.Material = new Material();
            innerSphere.Material.Ambient = 0;
            innerSphere.Material.Diffuse = 0.1;
            innerSphere.Material.Specular = 0.9;
            innerSphere.Material.Shininess = 300;
            innerSphere.Material.Reflective = 1;
            innerSphere.Material.Transparency = 1;
            innerSphere.Material.RefractiveIndex = 1.00029;
            group.AddShape(innerSphere);

            var outerSphere = new Sphere();
            outerSphere.CastsShadow = false;
            outerSphere.Material = new Material();
            outerSphere.Material.Ambient = 0;
            outerSphere.Material.Diffuse = 0.1;
            outerSphere.Material.Specular = 0.9;
            outerSphere.Material.Shininess = 300;
            outerSphere.Material.Reflective = 1;
            outerSphere.Material.Transparency = 1;
            outerSphere.Material.RefractiveIndex = 1.52;
            group.AddShape(outerSphere);

            World world = new World();
            world.Shapes = new List<Shape> {floor, group};
            world.Light = new PointLight(new Point(20, 10, 0), new Color(0.7, 0.7, 0.7));

            var pixels = 300 * 300;
            Camera camera = new Camera(300, 300, Math.PI/3);
            camera.Transform = Transformation.ViewTransform(new Point(0, 2.5, 0),
                                new Point(0, 0, 0),
                                new Vector(-1, 0, 0));


            var sw = new Stopwatch();
            sw.Start();

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/SphereInSphere.ppm";
            PpmWriter.WriteCanvasToPpm(filename, canvas);

            sw.Stop();

            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine($"{(double)pixels / sw.ElapsedMilliseconds}px/ms");
            Console.WriteLine($"Intersection tests: {Stats.Tests}");
            Console.WriteLine($"Primary rays:       {Stats.PrimaryRays}");
            Console.WriteLine($"Secondary rays:     {Stats.SecondaryRays}");
            Console.WriteLine($"Shadow rays:        {Stats.ShadowRays}");
            //Console.WriteLine($"Super sampling:     {args.N}x");
            //Console.WriteLine($"Output:             {Path.GetFullPath("out.ppm")}");
        }

        static void Main(string[] args)
        {
            Render();
        }
    }
}
