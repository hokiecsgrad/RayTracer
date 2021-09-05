using System;
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
            string sceneName = arguments.scene;

            World world;
            Camera camera;
            Canvas canvas;

            (world, camera) = SetupWorld(width, height, fov, sceneName);
            //canvas = Render(world, camera, new SteppedSampler(camera, 5));
            //canvas = Render(world, camera, new DefaultSampler(camera));
            canvas = Render(world, camera, new SuperSampler(camera, numSamples));
            //canvas = Render(world, camera, new AntiAliasSampler(camera, 4));
            //canvas = Render(world, camera, new FocalBlurSampler(camera, 1.0, 0.1, 8));

            SaveCanvas(canvas, output);
        }

        public static (World, Camera) SetupWorld(int width, int height, double fov, string sceneName)
        {
            //ObjectHandle handle = Activator.CreateInstance(
            //        "RayTracer.Cli",
            //        "RayTracer.Cli.Scenes." + sceneName + "Scene");
            //IScene scene = (IScene)handle.Unwrap();

            string fileContents = System.IO.File.ReadAllText(@"/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Cli/Scenes/Sphere.yaml");
            YamlStream yaml = new YamlStream();
            yaml.Load(new StringReader(fileContents));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            foreach (var entry in mapping.Children)
            {
                Console.WriteLine(entry);
            }

            IScene scene = new CoverScene();
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
