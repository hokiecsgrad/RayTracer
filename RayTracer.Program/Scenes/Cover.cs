using System;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program
{
    class CoverScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov) 
            {
                Transform = Transformation.ViewTransform(
                                new Point(-10, 10, -15),    // view from
                                new Point(6, 0, 6),      // view to
                                new Vector(-0.45, 1, 0)), // vector up
                
                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(50, 100, -50),
                new Color(0.8, 0.8, 0.8)
            );

            // an optional second light for additional illumination
            var light2 = new PointLight(
                new Point(-400, 50, -10),
                new Color(0.2, 0.2, 0.2)
            );

            // ======================================================
            // define some constants to avoid duplication
            // ======================================================
            var white_material = new Material()
            {
                Color = new Color(1, 1, 1),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1,
            };

            var blue_material = new Material()
            {
                Color = new Color(0.537, 0.831, 0.914),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1,
            };

            var red_material = new Material()
            {
                Color = new Color(0.941, 0.322, 0.388),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1,
            };

            var purple_material = new Material()
            {
                Color = new Color(0.373, 0.404, 0.550),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1,
            };

            var standard_transform = 
                Transformation.Scaling(0.5, 0.5, 0.5) *
                Transformation.Translation(1, -1, 1);

            var large_object = 
                Transformation.Scaling(3.5, 3.5, 3.5) *
                Transformation.Translation(1, -1, 1);

            var medium_object = 
                Transformation.Scaling(3, 3, 3) *
                Transformation.Translation(1, -1, 1);

            var small_object = 
                Transformation.Scaling(2, 2, 2) *
                Transformation.Translation(1, -1, 1);

            // ======================================================
            // a white backdrop for the scene
            // ======================================================
            var backdrop = new Plane()
            {
                Material = new Material()
                {
                    Color = Color.White,
                    Ambient = 1.0,
                    Diffuse = 0.0,
                    Specular = 0.0,
                },
                Transform = 
                    Transformation.Translation(0, 0, 500) * 
                    Transformation.Rotation_x(1.5707963267948966),
            };

            // ======================================================
            // describe the elements of the scene
            // ======================================================
            var obj1 = new Sphere()
            {
                Material = new Material()
                {
                    Color = new Color(0.373, 0.404, 0.550),
                    Ambient = 0.0,
                    Specular = 1.0,
                    Diffuse = 0.2,
                    Shininess = 200,
                    Reflective = 0.7,
                    Transparency = 0.7,
                    RefractiveIndex = 1.5
                },
                Transform = 
                    Transformation.Translation(0, 0, 0) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj2 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(8, 0, 0) *
                    Transformation.Scaling(3, 3, 3),
            };

            var obj3 = new Cube()
            {
                Material = red_material,
                Transform = 
                    Transformation.Translation(0, 0, 8) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj4 = new Cube()
            {
                Material = blue_material,
                Transform = 
                    Transformation.Translation(16, 1.5, -0.5) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj5 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(8, 0, 8) *
                    Transformation.Scaling(2, 2, 2),
            };

            var obj6 = new Cube()
            {
                Material = purple_material,
                Transform = 
                    Transformation.Translation(16, 0, 8) *
                    Transformation.Scaling(3, 3, 3),
            };

            var obj7 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(-0.25, 0.25, 16) *
                    Transformation.Scaling(3, 3, 3),
            };

            var obj8 = new Cube()
            {
                Material = blue_material,
                Transform = 
                    Transformation.Translation(8, 2, 16) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj9 = new Cube()
            {
                Material = red_material,
                Transform = 
                    Transformation.Translation(18, 4, 16) *
                    Transformation.Scaling(3, 3, 3),
            };

            var obj10 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(16, 4, 30) *
                    Transformation.Scaling(2, 2, 2),
            };

            var obj11 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(40, 3, 18) *
                    Transformation.Scaling(2, 2, 2),
            };

            var obj12 = new Cube()
            {
                Material = blue_material,
                Transform = 
                    Transformation.Translation(-0.5, -9, 0.25) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj13 = new Cube()
            {
                Material = red_material,
                Transform = 
                    Transformation.Translation(8, -8, 0) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj14 = new Cube()
            {
                Material = white_material,
                Transform =
                    Transformation.Translation(16, -8, 0) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj15 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(0, -8, 8) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj16 = new Cube()
            {
                Material = purple_material,
                Transform = 
                    Transformation.Translation(-0.5, -8.5, 16) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj17 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(0, -16, 8) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            var obj18 = new Cube()
            {
                Material = white_material,
                Transform = 
                    Transformation.Translation(-1, -17, 16) *
                    Transformation.Scaling(3.5, 3.5, 3.5),
            };

            World world = new World();
            world.Shapes = new List<Shape> {backdrop, obj1, obj2, obj3, obj4, obj5, obj6, obj7, obj8, obj9, obj10, obj11, obj12, obj13, obj14, obj15, obj16, obj17, obj18 };
            world.Lights = new List<PointLight> {light};

            return (world, camera);
        }
    }
}