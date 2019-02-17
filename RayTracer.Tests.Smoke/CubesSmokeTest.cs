using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class CubesSmokeTests
    {
        [Fact]
        public void RenderBasicScene()
        {

            // ======================================================
            // describe the elements of the scene
            // ======================================================

            // floor/ceiling
            var floorCeiling = new Cube();
            floorCeiling.Transform = Transformation.Translation(0, 3, 0) * Transformation.Scaling(20, 7, 20);
            floorCeiling.Material = new Material();
            floorCeiling.Material.Pattern = new Checkers(new Color(0,0,0), new Color(0.25, 0.25, 0.25));
            floorCeiling.Material.Pattern.Transform = Transformation.Translation(0, 0.1, 0) * Transformation.Scaling(0.07, 0.07, 0.07);
            floorCeiling.Material.Ambient = 0.25;
            floorCeiling.Material.Diffuse = 0.7;
            floorCeiling.Material.Specular = 0.9;
            floorCeiling.Material.Shininess = 300;
            floorCeiling.Material.Reflective = 0.1;

            // walls
            var walls = new Cube();
            walls.Transform = Transformation.Scaling(10, 10, 10);
            walls.Material = new Material();
            walls.Material.Pattern = new Checkers(new Color(0.4863, 0.3765, 0.2941), new Color(0.3725, 0.2902, 0.2275));
            walls.Material.Pattern.Transform = Transformation.Translation(1, -10, -1) * Transformation.Scaling(0.05, 20, 0.05);
            walls.Material.Ambient = 0.1;
            walls.Material.Diffuse = 0.7;
            walls.Material.Specular = 0.9;
            walls.Material.Shininess = 300;
            walls.Material.Reflective = 0.1;

            // table top
            var tableTop = new Cube();
            tableTop.Transform = Transformation.Translation(0, 3.1, 0) * Transformation.Scaling(3, 0.1, 2);
            tableTop.Material = new Material();
            tableTop.Material.Pattern = new Stripe(new Color(0.5529, 0.4235, 0.3255), new Color(0.6588, 0.5098, 0.4000));
            tableTop.Material.Pattern.Transform = Transformation.Translation(0, 0.1, 0) * Transformation.Scaling(0.05, 0.05, 0.05) * Transformation.Rotation_y(0.1);
            tableTop.Material.Ambient = 0.1;
            tableTop.Material.Diffuse = 0.7;
            tableTop.Material.Specular = 0.9;
            tableTop.Material.Shininess = 300;
            tableTop.Material.Reflective = 0.2;

            // leg #1
            var leg1 = new Cube();
            leg1.Transform = Transformation.Translation(2.7, 1.5, -1.7) * Transformation.Scaling(0.1, 1.5, 0.1);
            leg1.Material = new Material();
            leg1.Material.Color = new Color(0.5529, 0.4235, 0.3255);
            leg1.Material.Ambient = 0.2;
            leg1.Material.Diffuse = 0.7;

            // leg #2
            var leg2 = new Cube();
            leg2.Transform = Transformation.Translation(2.7, 1.5, 1.7) * Transformation.Scaling(0.1, 1.5, 0.1);
            leg2.Material = new Material();
            leg2.Material.Color = new Color(0.5529, 0.4235, 0.3255);
            leg2.Material.Ambient = 0.2;
            leg2.Material.Diffuse = 0.7;

            // leg #3
            var leg3 = new Cube();
            leg3.Transform = Transformation.Translation(-2.7, 1.5, -1.7) * Transformation.Scaling(0.1, 1.5, 0.1);
            leg3.Material = new Material();
            leg3.Material.Color = new Color(0.5529, 0.4235, 0.3255);
            leg3.Material.Ambient = 0.2;
            leg3.Material.Diffuse = 0.7;

            // leg #4
            var leg4 = new Cube();
            leg4.Transform = Transformation.Translation(-2.7, 1.5, 1.7) * Transformation.Scaling(0.1, 1.5, 0.1);
            leg4.Material = new Material();
            leg4.Material.Color = new Color(0.5529, 0.4235, 0.3255);
            leg4.Material.Ambient = 0.2;
            leg4.Material.Diffuse = 0.7;

            // glass cube
            var glassCube = new Cube();
            glassCube.Transform = Transformation.Translation(0, 3.45001, 0) * Transformation.Rotation_y(0.2) * Transformation.Scaling(0.25, 0.25, 0.25);
            glassCube.CastsShadow = false;
            glassCube.Material = new Material();
            glassCube.Material.Color = new Color(1, 1, 0.8);
            glassCube.Material.Ambient = 0;
            glassCube.Material.Diffuse = 0.3;
            glassCube.Material.Specular = 0.9;
            glassCube.Material.Shininess = 300;
            glassCube.Material.Reflective = 0.7;
            glassCube.Material.Transparency = 0.7;
            glassCube.Material.RefractiveIndex = 1.5;

            // little cube #1
            var littleCube1 = new Cube();
            littleCube1.Transform = Transformation.Translation(1, 3.35, -0.9) * Transformation.Rotation_y(-0.4) * Transformation.Scaling(0.15, 0.15, 0.15);
            littleCube1.Material = new Material();
            littleCube1.Material.Color = new Color(1, 0.5, 0.5);
            littleCube1.Material.Reflective = 0.6;
            littleCube1.Material.Diffuse = 0.4;

            // little cube #2
            var littleCube2 = new Cube();
            littleCube2.Transform = Transformation.Translation(-1.5, 3.27, 0.3) * Transformation.Rotation_y(0.4) * Transformation.Scaling(0.15, 0.07, 0.15);
            littleCube2.Material = new Material();
            littleCube2.Material.Color = new Color(1, 1, 0.5);

            // little cube #3
            var littleCube3 = new Cube();
            littleCube3.Transform = Transformation.Translation(0, 3.25, 1) * Transformation.Rotation_y(0.4) * Transformation.Scaling(0.2, 0.05, 0.05);
            littleCube3.Material = new Material();
            littleCube3.Material.Color = new Color(0.5, 1, 0.5);

            // little cube #4
            var littleCube4 = new Cube();
            littleCube4.Transform = Transformation.Translation(-0.6, 3.4, -1) * Transformation.Rotation_y(0.8) * Transformation.Scaling(0.05, 0.2, 0.05);
            littleCube4.Material = new Material();
            littleCube4.Material.Color = new Color(0.5, 0.5, 1);

            // little cube #5
            var littleCube5 = new Cube();
            littleCube5.Transform = Transformation.Translation(2, 3.4, 1) * Transformation.Rotation_y(0.8) * Transformation.Scaling(0.05, 0.2, 0.05);
            littleCube5.Material = new Material();
            littleCube5.Material.Color = new Color(0.5, 1, 1);

            // mirror
            var mirror = new Cube();
            mirror.Transform = Transformation.Translation(-2, 3.5, 9.95) * Transformation.Scaling(4.8, 1.4, 0.06);
            mirror.Material = new Material();
            mirror.Material.Color = new Color(0,0,0);
            mirror.Material.Diffuse = 0;
            mirror.Material.Ambient = 0;
            mirror.Material.Specular = 1;
            mirror.Material.Shininess = 300;
            mirror.Material.Reflective = 1;

            // frame #1
            var frame1 = new Cube();
            frame1.Transform = Transformation.Translation(-10, 4, 1) * Transformation.Scaling(0.05, 1, 1);
            frame1.Material = new Material();
            frame1.Material.Color = new Color(0.7098, 0.2471, 0.2196);
            frame1.Material.Diffuse = 0.6;

            // frame #2
            var frame2 = new Cube();
            frame2.Transform = Transformation.Translation(-10, 3.4, 2.7) * Transformation.Scaling(0.05, 0.4, 0.4);
            frame2.Material = new Material();
            frame2.Material.Color = new Color(0.2667, 0.2706, 0.6902);
            frame2.Material.Diffuse = 0.6;

            // frame #3
            var frame3 = new Cube();
            frame3.Transform = Transformation.Translation(-10, 4.6, 2.7) * Transformation.Scaling(0.05, 0.4, 0.4);
            frame3.Material = new Material();
            frame3.Material.Color = new Color(0.3098, 0.5961, 0.3098);
            frame3.Material.Diffuse = 0.6;

            // frame #4
            var frame4 = new Cube();
            frame4.Transform = Transformation.Translation(-2, 3.5, 9.95) * Transformation.Scaling(5, 1.5, 0.05);
            frame4.Material = new Material();
            frame4.Material.Color = new Color(0.3882, 0.2627, 0.1882);
            frame4.Material.Diffuse = 0.7;

            World world = new World();
            world.Shapes = new List<Shape> {walls, floorCeiling, tableTop, leg1, leg2, leg3, leg4, glassCube, mirror, frame1, frame2, frame3, frame4, littleCube1, littleCube2, littleCube3, littleCube4, littleCube5};
  
            // ======================================================
            // light sources
            // ======================================================

            world.Light = new PointLight(new Point(0, 6.9, -5), new Color(1, 1, 0.9));

            // ======================================================
            // the camera
            // ======================================================

            Camera camera = new Camera(400, 200, 0.785);
            camera.Transform = Transformation.ViewTransform(
                                new Point(8, 6, -8), // view from
                                new Point(0, 3, 0),// view to
                                new Vector(0, 1, 0));    // vector up

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/Cubes.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}