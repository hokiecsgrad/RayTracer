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
  from: [-5.0, 1.5, 0.0]
  to: [0.0, 0.0, 0.0]
  up: [0, 1, 0]";

            YamlParser yamlParser = new YamlParser(yamlString);
            Camera camera = yamlParser.ParseCamera();

            Assert.IsType<Camera>(camera);
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
      color: [ 0.8, 0.5, 0.3 ]
      shininess: 50";

            YamlParser yamlParser = new YamlParser(yamlString);
            List<Shape> shapes = yamlParser.ParseShapes();

            Assert.Single(shapes);
            Assert.True(shapes[0] is Sphere);
            Assert.Equal(new Color(0.8, 0.5, 0.3), shapes[0].Material.Color);
        }

        [Fact]
        public void Parse_MaterialWithPattern_ShouldReturnPattern()
        {
            string yamlString = @"materials:
  - name: wall-material
    pattern:
      type: stripes
      colors:
        - [0.45, 0.45, 0.45]
        - [0.55, 0.55, 0.55]
    transform:
      scale: [ 0.25, 0.25, 0.25 ]
      rotate-y: [ 1.5708 ]
    ambient: 0
    diffuse: 0.4
    specular: 0
    reflective: 0.3";

            YamlParser yamlParser = new YamlParser(yamlString);
            Dictionary<string, Material> materials = yamlParser.ParseMaterials();

            Assert.Equal(new Color(0.45, 0.45, 0.45), ((Stripe)materials["wall-material"].Pattern).a);
            Assert.Equal(new Color(0.55, 0.55, 0.55), ((Stripe)materials["wall-material"].Pattern).b);
        }

        [Fact]
        public void Parse_WholeValidSimpleSceneFromString_ShouldCreateOneCameraOneLightOneSphere()
        {
            string yamlString = @"
camera:
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
      pattern:
        type: stripes
        colors:
          - [1, 1, 1]
          - [0, 0, 0]
      ambient: 0.2
      diffuse: 0.4
      specular: 0.9
      shininess: 50";

            YamlParser yamlParser = new YamlParser(yamlString);
            yamlParser.Parse();

            Assert.Single(yamlParser.Lights);
            Assert.Single(yamlParser.Shapes);
            Assert.True(yamlParser.Shapes[0] is Sphere);
            Assert.IsType<Stripe>(yamlParser.Shapes[0].Material.Pattern);
        }

        [Fact]
        public void Parse_WholeValidSimpleSceneFromFile_ShouldCreateOneCameraOneLightOneSphere()
        {
            string yamlString = File.ReadAllText("../../../Sphere.yaml");
            YamlParser yamlParser = new YamlParser(yamlString);
            yamlParser.Parse();

            Assert.Single(yamlParser.Lights);
            Assert.Single(yamlParser.Shapes);
            Assert.True(yamlParser.Shapes[0] is Sphere);
            Assert.IsType<Stripe>(yamlParser.Shapes[0].Material.Pattern);
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
