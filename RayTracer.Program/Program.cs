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
            var scene = new RefractionScene();
            var width = 400;
            var height = 300;
            var fov = Math.PI/3;
            var filename = "/Users/ryan.hagan/Documents/VSCode Proejects/RayTracer/RayTracer.Program/refraction.ppm";

            World world;
            Camera camera;

            var sw = new Stopwatch();
            sw.Start();

            (world, camera) = scene.Setup(width, height, fov);
            Canvas canvas = camera.Render(world, new DefaultSampler(camera));

            Program.SaveCanvasToFile(canvas, filename);

            sw.Stop();

            var pixels = width*height;
            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine($"{(double)pixels / sw.ElapsedMilliseconds}px/ms");
            Console.WriteLine($"Intersection tests: {Stats.Tests}");
            Console.WriteLine($"Primary rays:       {Stats.PrimaryRays}");
            Console.WriteLine($"Secondary rays:     {Stats.SecondaryRays}");
            Console.WriteLine($"Shadow rays:        {Stats.ShadowRays}");
            //Console.WriteLine($"Super sampling:     {args.N}x");
            //Console.WriteLine($"Output:             {Path.GetFullPath("out.ppm")}");
        }

        private static void SaveCanvasToFile(Canvas canvas, string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }


        static void Main(string[] args)
        {
            Render();
        }
    }
}
