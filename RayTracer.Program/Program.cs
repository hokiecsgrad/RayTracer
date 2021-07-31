using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using RayTracer;

namespace RayTracer.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            World world;
            Camera camera;
            Canvas canvas;

            string output = GetOutputImagePath();
            int width = 800;
            int height = 600;
            double fov = 0.8;

            (world, camera) = SetupWorld(width, height, fov);
            canvas = Render(world, camera, new SteppedSampler(camera, 5));
            //canvas = Render(world, camera, new DefaultSampler(camera));
            //canvas = Render( world, camera, new AntiAliasSampler( camera, 8 ) );
            //canvas = Render( world, camera, new FocalBlurSampler( camera, 1.0, 0.1, 8 ) );

            SaveCanvas(canvas, output + "bumpmap.ppm");
        }

        public static (World, Camera) SetupWorld(int width, int height, double fov)
        {
            var scene = new BumpMapScene();
            return scene.Setup(width, height, fov);
        }

        public static Canvas Render(World world, Camera camera, ISampler sampler)
        {
            var sw = new Stopwatch();
            sw.Start();
            Canvas canvas = camera.Render(world, sampler);
            sw.Stop();

            var pixels = canvas.Width * canvas.Height;
            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine($"{(double)pixels / sw.ElapsedMilliseconds}px/ms");
            Console.WriteLine($"Intersection tests: {Stats.Tests}");
            Console.WriteLine($"Primary rays:       {Stats.PrimaryRays}");
            Console.WriteLine($"Secondary rays:     {Stats.SecondaryRays}");
            Console.WriteLine($"Shadow rays:        {Stats.ShadowRays}");
            //Console.WriteLine($"Super sampling:     {args.N}x");

            return canvas;
        }

        private static void SaveCanvas(Canvas canvas, string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }

        private static string GetOutputImagePath()
        {
            StringBuilder imagePath = new StringBuilder();
            string currpath = Directory.GetCurrentDirectory();

            string[] pathParts = currpath.Split(
                Path.DirectorySeparatorChar,
                StringSplitOptions.RemoveEmptyEntries);

            int index = 0;
            foreach (string part in pathParts)
            {
                imagePath.Append(Path.DirectorySeparatorChar + pathParts[index++]);
                if (part == "RayTracer.Program") break;
            }
            imagePath.Append(Path.DirectorySeparatorChar);
            imagePath.Append("images");
            imagePath.Append(Path.DirectorySeparatorChar);

            if (!Directory.Exists(imagePath.ToString()))
                Directory.CreateDirectory(imagePath.ToString());

            return imagePath.ToString();
        }
    }
}
