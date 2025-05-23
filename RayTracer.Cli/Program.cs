﻿using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using PowerArgs;
using RayTracer;
using RayTracer.Cli.Scenes;
using System.Runtime.Remoting;
using YamlDotNet.RepresentationModel;

namespace RayTracer.Cli
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class Program
    {
        private static RayTracerArgs arguments;

        [HelpHook]
        [ArgShortcut("-?")]
        [ArgDescription("Shows this help")]
        public static bool help { get; set; }

        public static void Main(string[] args)
        {
            arguments = Args.Parse<RayTracerArgs>(args);

            string output = GetOutputImagePath() + arguments.filename;
            int width = arguments.width;
            int height = arguments.height;
            double fov = arguments.fov;
            int numSamples = arguments.n;
            string sampler = arguments.sampler;
            string sceneName = arguments.scene;

            World world;
            Camera camera;
            Canvas canvas;

            (world, camera) = SetupWorldFromYaml(width, height, fov, sceneName);
            //TODO: I'm working on a Yaml based scene definition file so that I can 
            // change the scene easily without having to rebuild the entire app.
            //(world, camera) = SetupWorldFromScene(width, height, fov, sceneName);
            camera.ProgressMonitor = new ParallelConsoleProgressMonitor(height);

            switch (sampler) 
            {
                case "aa":
                    numSamples = 8;
                    canvas = Render(world, camera, new AntiAliasSampler(camera, numSamples));
                    break;
                case "blur":
                    canvas = Render(world, camera, new FocalBlurSampler(camera, 1.0, 0.1, 8));
                    break;
                case "debug": 
                    if (numSamples == 1) numSamples = 5;
                    canvas = Render(world, camera, new SteppedSampler(camera, numSamples));
                    break;
                default:
                    if (numSamples == 1)
                        canvas = Render(world, camera, new DefaultSampler(camera));
                    else
                        canvas = Render(world, camera, new SuperSampler(camera, numSamples));
                    break;
            }

            SaveCanvas(canvas, output);
        }

        public static (World, Camera) SetupWorldFromYaml(int width, int height, double fov, string sceneName)
        {
            string yamlString = File.ReadAllText($"../RayTracer.Cli/Scenes/{sceneName}.yaml");
            YamlParser yamlParser = new YamlParser(yamlString);
            yamlParser.Parse();

            World world = new World();
            world.Shapes = yamlParser.Shapes;
            world.Lights = yamlParser.Lights;
            yamlParser.Camera.HSize = width;
            yamlParser.Camera.VSize = height;
            yamlParser.Camera.FieldOfView = fov;
            yamlParser.Camera.CalculatePixelSize();

            return (world, yamlParser.Camera);
        }

        public static (World, Camera) SetupWorldFromScene(int width, int height, double fov, string sceneName)
        {
            ObjectHandle handle = Activator.CreateInstance(
                    "RayTracer.Cli",
                    "RayTracer.Cli.Scenes." + sceneName + "Scene");
            IScene scene = (IScene)handle.Unwrap();
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
            Console.WriteLine($"Canvas size:        {canvas.Width}x{canvas.Height}");
            Console.WriteLine($"Intersection tests: {Stats.Tests}");
            Console.WriteLine($"Primary rays:       {Stats.PrimaryRays}");
            Console.WriteLine($"Secondary rays:     {Stats.SecondaryRays}");
            Console.WriteLine($"Shadow rays:        {Stats.ShadowRays}");
            Console.WriteLine($"Super sampling:     {arguments.n}x");

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
                if (part == "RayTracer.Cli") break;
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
