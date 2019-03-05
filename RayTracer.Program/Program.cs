using System;
using System.IO;
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
            World world;
            Camera camera;

            var width = 200;
            var height = 200;
            (world, camera) = new TeapotScene().Setup(width, height, Math.PI / 2);

            var pixels = width*height;
            var sw = new Stopwatch();
            sw.Start();

            Canvas canvas = camera.Render(world);

            //var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Cover.ppm";
            var filename = "/Users/ryan.hagan/Documents/VSCode Proejects/RayTracer/RayTracer.Program/teapot.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();

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
