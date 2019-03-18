using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program
{
    class TextureMapScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov) 
            {
                Transform = Transformation.ViewTransform(
                                new Point(1, 2, -7), // view from
                                new Point(0, 1, 0),// view to
                                new Vector(0, 1, 0)),   // vector up
                
                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(-100, 100, -100),
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

            var pedestal = new Cylinder()
            {
                Minimum = 0.0,
                Maximum = 0.1,
                IsClosed = true,
                Material = new Material()
                {
                    Color = Color.White,
                    Ambient = 0.1,
                    Diffuse = 0.2,
                    Specular = 0.0,
                    Reflective = 0.1,
                },
            };

            FileStream stream = File.OpenRead("/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Scenes/Textures/earthmap1k.ppm");
            StreamReader reader = new StreamReader(stream);
            var earthTexture = PpmReader.ReadCanvasFromPpm(reader);
            stream = File.OpenRead("/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Scenes/Textures/earthspec1k.ppm");
            reader = new StreamReader(stream);
            var earthSpecMap = PpmReader.ReadCanvasFromPpm(reader);
            stream = File.OpenRead("/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Scenes/Textures/earthbump1k.ppm");
            reader = new StreamReader(stream);
            var earthBumpMap = PpmReader.ReadCanvasFromPpm(reader);
            var earth = new Sphere()
            {
                Transform = 
                    Transformation.Translation(0, 1.1, 0) * 
                    Transformation.Rotation_y(1.9),
                Material = new Material()
                {
                    Pattern = new TextureMap(
                        new UvImage(earthTexture),
                        TextureMapper.SphericalMap
                    ),
                    SpecularMap = new TextureMap(
                        new UvImage(earthSpecMap),
                        TextureMapper.SphericalMap
                    ),
                    BumpMap = new TextureMap(
                        new UvImage(earthBumpMap),
                        TextureMapper.SphericalMap
                    ),
                    Ambient = 0.3,
                    Diffuse = 0.9,
                    Specular = 0.0,
                    Shininess = 300,
                    Reflective = 0.7,
                },
            };

            stream = File.OpenRead("/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Scenes/Textures/earthcloudmap.ppm");
            reader = new StreamReader(stream);
            var cloudTexture = PpmReader.ReadCanvasFromPpm(reader);
            var clouds = new Sphere()
            {
                Transform = 
                    Transformation.Translation(0, 1.1, 0) * 
                    Transformation.Scaling(1.1, 1.1, 1.1),
                Material = new Material()
                {
                    Pattern = new TextureMap(
                        new UvImage(cloudTexture),
                        TextureMapper.SphericalMap
                    ),
                    Diffuse = 0.1,
                    Specular = 0.0,
                    Shininess = 100,
                    Ambient = 0.1,
                    Transparency = 1.0,
                },
                HitBy = RayType.NoShadows,
            };


            World world = new World();
            world.Shapes = new List<Shape> {floor, pedestal, earth};
            world.Lights = new List<ILight> {light};

            return (world, camera);
        }
    }
}