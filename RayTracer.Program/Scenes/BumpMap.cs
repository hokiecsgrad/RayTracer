using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program
{
    class BumpMapScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov) 
            {
                Transform = Transformation.ViewTransform(
                                new Point(1, 2, -5), // view from
                                new Point(0, 2, 0),// view to
                                new Vector(0, 1, 0)),   // vector up
                
                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(-20, 20, -20),
                new Color(1, 1, 1)
            );
            var light2 = new PointLight(
                new Point(-20, 10, -5),
                new Color(1, 1, 1)
            );
            var areaLight = new AreaLight(
                new Point(-100, 100, -100),
                new Vector(4, 0, 0),
                6,
                new Vector(0, 4, 0),
                6,
                new Color(1.0, 1.0, 1.0)
            );


            var floor = new Plane()
            {
                Material = new Material()
                {
                    Color = Color.White,
                    Ambient = 0.1,
                    Diffuse = 0.1,
                    Specular = 0.0,
                    Reflective = 0.4,
                },
            };


            FileStream stream = File.OpenRead("/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Scenes/Textures/brickwall.ppm");
            StreamReader reader = new StreamReader(stream);
            var wallTexture = PpmReader.ReadCanvasFromPpm(reader);
            var textureLeft = new UvImage(wallTexture);
            var textureFront = textureLeft;
            var textureRight = textureLeft;
            var textureBack = textureLeft;
            var textureUp = textureLeft;
            var textureDown = textureLeft;

            stream = File.OpenRead("/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Scenes/Textures/brickwall_normal.ppm");
            reader = new StreamReader(stream);
            var wallNormalMap = PpmReader.ReadCanvasFromPpm(reader);
            var normalLeft = new UvImage(wallNormalMap);
            var normalFront = normalLeft;
            var normalRight = normalLeft;
            var normalBack = normalLeft;
            var normalUp = normalLeft;
            var normalDown = normalLeft;

            var mapCubeMaterial = new Material()
            {
                Pattern = new CubeMap(textureLeft, textureFront, textureRight, textureBack, textureUp, textureDown),
                NormalMap = new CubeMap(normalLeft, normalFront, normalRight, normalBack, normalUp, normalDown),
                Ambient = 0.7,
                Specular = 0.0,
                Diffuse = 0.8,
            };
            
            var box = new Cube()
            {
                Transform = 
                    Transformation.Translation(0, 2, 0) *
                    Transformation.Rotation_y(1.9),
                Material = mapCubeMaterial,
            };

            World world = new World();
            world.Shapes = new List<Shape> {floor, box};
            world.Lights = new List<ILight> {light};

            return (world, camera);
        }
    }
}