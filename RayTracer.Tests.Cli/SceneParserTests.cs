using System;
using System.IO;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using RayTracer;
using RayTracer.Cli;
using Xunit;

namespace RayTracer.Tests.Cli
{
    public class SceneParserTests
    {
        [Fact]
        public void Parse_WithCamera_ShouldCreateCamera()
        {
            string yamlString = @"camera:
  width: 400
  height: 400
  field-of-view: 1.152
  from: [-5.0, 1.5, 0.0]
  to: [0.0, 0.0, 0.0]
  up: [0, 1, 0]";

            YamlParser yamlParser = new YamlParser(yamlString);
            Camera camera = yamlParser.ParseCamera();

            Assert.Equal(400, camera.HSize);
            Assert.Equal(400, camera.VSize);
            Assert.Equal(1.152, camera.FieldOfView);
        }

        [Fact]
        public void Parse_WithSingleLight_ShouldCreateSingleLight()
        {
            string yamlString = @"lights:
  - type: point
    at: [-4.9, 4.9, -1]
    intensity: [1, 1, 1]";

            const double epsilon = 0.0001;
            IEqualityComparer<Point> PointComparer =
                Point.GetEqualityComparer(epsilon);

            YamlParser yamlParser = new YamlParser(yamlString);
            List<ILight> lights = yamlParser.ParseLights();

            Assert.Single(lights);
            Assert.Equal(new Color(1, 1, 1), lights[0].Color);
            Assert.Equal(new Point(-4.9, 4.9, -1), ((PointLight)lights[0]).Position, PointComparer);
        }

        [Fact]
        public void Parse_WithMultipleLights_ShouldCreateTwoLights()
        {
            string yamlString = @"lights:
  - type: point
    at: [-4.9, 4.9, -1]
    intensity: [1, 1, 1]
  
  - type: point
    at: [0, 0, 0]
    intensity: [0.5, 0.5, 0.5]";

            YamlParser yamlParser = new YamlParser(yamlString);
            List<ILight> lights = yamlParser.ParseLights();

            Assert.Equal(2, lights.Count);
            Assert.Equal(new Color(1, 1, 1), lights[0].Color);
            Assert.Equal(new Color(0.5, 0.5, 0.5), lights[1].Color);
        }

        [Fact]
        public void Parse_WithOneSphere_ShouldCreateASphere()
        {
            string yamlString = @"shapes:
  - type: sphere
    transform:
      scale: [ 1.0, 1.0, 1.0 ]
      translate: [ 0.0, 0.0, 0.0 ]
    material:
      color: [0.8, 0.5, 0.3]
      shininess: 50";

            YamlParser yamlParser = new YamlParser(yamlString);
            List<Shape> shapes = yamlParser.ParseShapes();

            Assert.Single(shapes);
            Assert.True(shapes[0] is Sphere);
            Assert.Equal(new Color(0.8, 0.5, 0.3), shapes[0].Material.Color);
        }

        [Fact]
        public void Parse_WholeValidSimpleSceneFromString_ShouldCreateOneCameraOneLightOneSphere()
        {
            string yamlString = @"
camera:
  width: 400
  height: 400
  field-of-view: 1.152
  from: [-5.0, 1.5, 0.0]
  to: [0.0, 0.0, 0.0]
  up: [0, 1, 0]

lights:
  - type: point
    at: [-4.9, 4.9, -1]
    intensity: [1, 1, 1]

shapes:
  - type: sphere
    transform:
      scale: [ 1.0, 1.0, 1.0 ]
      translate: [ 0.0, 0.0, 0.0 ]
    material:
      color: [0.8, 0.5, 0.3]
      shininess: 50";

            YamlParser yamlParser = new YamlParser(yamlString);
            yamlParser.Parse();

            Assert.Equal(400, yamlParser.Camera.HSize);
            Assert.Equal(400, yamlParser.Camera.VSize);
            Assert.Equal(1.152, yamlParser.Camera.FieldOfView);
            Assert.Single(yamlParser.Lights);
            Assert.Equal(new Color(1, 1, 1), yamlParser.Lights[0].Color);
            Assert.Single(yamlParser.Shapes);
            Assert.True(yamlParser.Shapes[0] is Sphere);
            Assert.Equal(new Color(0.8, 0.5, 0.3), yamlParser.Shapes[0].Material.Color);
        }

        [Fact]
        public void Parse_WholeValidSimpleSceneFromFile_ShouldCreateOneCameraOneLightOneSphere()
        {
            string yamlString = File.ReadAllText("../../../Sphere.yaml");
            YamlParser yamlParser = new YamlParser(yamlString);
            yamlParser.Parse();

            Assert.Equal(400, yamlParser.Camera.HSize);
            Assert.Equal(300, yamlParser.Camera.VSize);
            Assert.Equal(1.152, yamlParser.Camera.FieldOfView);
            Assert.Single(yamlParser.Lights);
            Assert.Equal(new Color(1, 1, 1), yamlParser.Lights[0].Color);
            Assert.Single(yamlParser.Shapes);
            Assert.True(yamlParser.Shapes[0] is Sphere);
            Assert.Equal(new Color(0.8, 0.5, 0.3), yamlParser.Shapes[0].Material.Color);
        }

        [Fact]
        public void Parse_SphereWithTranslation_ShouldMoveToCorrectPosition()
        {
            string yamlString = @"shapes:
  - type: sphere
    transform:
      translate: [ 5, -3, 2 ]
    material:
      color: [0.8, 0.5, 0.3]
      shininess: 50";

            const double epsilon = 0.001;
            IEqualityComparer<Matrix> MatrixComparer =
                Matrix.GetEqualityComparer(epsilon);

            YamlParser yamlParser = new YamlParser(yamlString);
            List<Shape> shapes = yamlParser.ParseShapes();

            Sphere sphere = new Sphere() { Transform = Transformation.Translation(5, -3, 2) };
            Matrix expected = sphere.Transform;

            Assert.Equal(expected, shapes[0].Transform, MatrixComparer);
        }

        [Fact]
        public void Parse_SphereWithTranslationAndScale_ShouldMoveToCorrectPositionAndScale()
        {
            string yamlString = @"shapes:
  - type: sphere
    transform:
      scale: [2, 2, 2]
      translate: [ 5, -3, 2 ]
    material:
      color: [0.8, 0.5, 0.3]
      shininess: 50";

            const double epsilon = 0.001;
            IEqualityComparer<Matrix> MatrixComparer =
                Matrix.GetEqualityComparer(epsilon);

            YamlParser yamlParser = new YamlParser(yamlString);
            List<Shape> shapes = yamlParser.ParseShapes();

            Sphere sphere = new Sphere()
            {
                Transform = Transformation.Translation(5, -3, 2) *
                                Transformation.Scaling(2, 2, 2)
            };
            Matrix expected = sphere.Transform;

            Assert.Equal(expected, shapes[0].Transform, MatrixComparer);
        }
    }
}
