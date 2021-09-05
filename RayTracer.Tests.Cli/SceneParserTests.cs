using System;
using System.IO;
using YamlDotNet.RepresentationModel;
using RayTracer;
using RayTracer.Cli;
using Xunit;

namespace RayTracer.Tests.Cli
{
    public class SceneParserTests
    {
        [Fact]
        public void Parse_WithCamera_ShouldReturnCameraString()
        {
            string yamlString = @"camera:
  width: 400
  height: 400
  field-of-view: 1.152
  from: [-5.0, 1.5, 0.0]
  to: [0.0, 0.0, 0.0]
  up: [0, 1, 0]";

            YamlStream yaml = new YamlStream();
            yaml.Load(new StringReader(yamlString));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var entry = mapping.Children[0];

            Assert.Equal("camera", entry.Key);
        }

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
            yamlParser.Parse();

            Assert.Equal(400, yamlParser.Camera.HSize);
            Assert.Equal(400, yamlParser.Camera.VSize);
            Assert.Equal(1.152, yamlParser.Camera.FieldOfView);
        }

        [Fact]
        public void Parse_WithSingleLight_ShouldCreateSingleLight()
        {
            string yamlString = @"lights:
  - at: [-4.9, 4.9, -1]
    intensity: [1, 1, 1]";

            YamlParser yamlParser = new YamlParser(yamlString);
            yamlParser.Parse();

            Assert.Single(yamlParser.Lights);
            Assert.Equal(new Color(1, 1, 1), yamlParser.Lights[0].Color);
        }

        [Fact]
        public void Parse_WithMultipleLights_ShouldCreateTwoLights()
        {
            string yamlString = @"lights:
  - at: [-4.9, 4.9, -1]
    intensity: [1, 1, 1]
  
  - at: [0, 0, 0]
    intensity: [0.5, 0.5, 0.5]";

            YamlParser yamlParser = new YamlParser(yamlString);
            yamlParser.Parse();

            Assert.Equal(2, yamlParser.Lights.Count);
            Assert.Equal(new Color(1, 1, 1), yamlParser.Lights[0].Color);
            Assert.Equal(new Color(0.5, 0.5, 0.5), yamlParser.Lights[1].Color);
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
            yamlParser.Parse();

            Assert.Single(yamlParser.Shapes);
            Assert.True(yamlParser.Shapes[0] is Sphere);
            Assert.Equal(new Color(0.8, 0.5, 0.3), yamlParser.Shapes[0].Material.Color);
        }

        [Fact]
        public void Parse_WholeValidSimpleScene_ShouldCreateOneCameraOneLightOneSphere()
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
  - at: [-4.9, 4.9, -1]
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
    }
}
