using System;
using System.IO;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using RayTracer;

namespace RayTracer.Cli
{
    public class YamlParser
    {
        private string YamlString = String.Empty;
        private YamlMappingNode _yamlRoot = null;

        public Camera Camera { get; private set; }
        public List<ILight> Lights { get; private set; }
        public List<Shape> Shapes { get; private set; }

        public YamlParser(string yaml)
        {
            if (String.IsNullOrEmpty(yaml))
                throw new ArgumentException("YAML string cannot be empty!");

            YamlString = yaml;
        }

        public void Parse()
        {
            CreateMappingNode();

            ValidateSceneElements();

            Camera = ParseCamera();
            Lights = ParseLights();
            Shapes = ParseShapes();
        }

        private void CreateMappingNode()
        {
            try
            {
                YamlStream yaml = new YamlStream();
                yaml.Load(new StringReader(YamlString));
                _yamlRoot = (YamlMappingNode)yaml.Documents[0].RootNode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Invalid YAML file provided. Error: {0}", ex.Message);
            }
        }

        private void ValidateSceneElements()
        {
            if (!_yamlRoot.Children.ContainsKey("camera") &&
                    _yamlRoot.Children.Count != 6)
                throw new ArgumentException("Scene must contain a valid camera configuration.");

            if (!_yamlRoot.Children.ContainsKey("lights") &&
                    _yamlRoot.Children.Count < 1)
                throw new ArgumentException("Scene must contain at least one light source.");

            if (!_yamlRoot.Children.ContainsKey("shapes") &&
                    _yamlRoot.Children.Count < 1)
                throw new ArgumentException("Scene must contain at least one valid shape.");
        }

        public Camera ParseCamera()
        {
            if (_yamlRoot is null) CreateMappingNode();
            if (!_yamlRoot.Children.ContainsKey("camera")) return null;

            YamlNode cameraData = _yamlRoot.Children[new YamlScalarNode("camera")];

            int height = GetIntFromNode(cameraData["height"]);
            int width = GetIntFromNode(cameraData["width"]);
            double fov = GetDoubleFromNode(cameraData["field-of-view"]);
            Point from = GetPointFromNode(cameraData["from"]);
            Point to = GetPointFromNode(cameraData["to"]);
            Vector up = GetVectorFromNode(cameraData["up"]);

            return new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                                from,
                                                to,
                                                up)
            };
        }

        public List<ILight> ParseLights()
        {
            if (_yamlRoot is null) CreateMappingNode();
            if (!_yamlRoot.Children.ContainsKey("lights")) return null;

            List<ILight> lights = new();
            YamlSequenceNode lightsNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("lights")];

            foreach (YamlMappingNode light in lightsNode)
            {
                Point position =
                    GetPointFromNode(light.Children[new YamlScalarNode("at")]);
                Color intensity =
                    GetColorFromNode(light.Children[new YamlScalarNode("intensity")]);

                lights.Add(new PointLight(position, intensity));
            }

            return lights;
        }

        public List<Shape> ParseShapes()
        {
            if (_yamlRoot is null) CreateMappingNode();
            if (!_yamlRoot.Children.ContainsKey("shapes")) return null;

            List<Shape> shapes = new();
            YamlSequenceNode shapesNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("shapes")];

            foreach (YamlMappingNode shape in shapesNode)
            {
                string type = shape.Children[new YamlScalarNode("type")].ToString();
                Matrix transformations =
                    ParseTransformations((YamlMappingNode)shape.Children[new YamlScalarNode("transform")]);
                Material material =
                    ParseMaterial((YamlMappingNode)shape.Children[new YamlScalarNode("material")]);
                shapes.Add(CreateShape(type, transformations, material));
            }

            return shapes;
        }

        private Shape CreateShape(string type, Matrix transformation, Material material)
            => type switch
            {
                "sphere" => new Sphere() { Transform = transformation, Material = material },
                "plane" => new Plane() { Transform = transformation, Material = material },
                "cube" => new Cube() { Transform = transformation, Material = material },
                "cylinder" => new Cylinder() { Transform = transformation, Material = material },
                "cone" => new Cone() { Transform = transformation, Material = material },
                _ => throw (new Exception("Invalid shape!!")),
            };

        private Matrix ParseTransformations(YamlMappingNode transformNode)
        {
            Matrix transformation = Matrix.Identity;

            if (transformNode.Children.ContainsKey("scale"))
                transformation *=
                    GetTranslationsTransform(
                        transformNode.Children[new YamlScalarNode("scale")]);

            if (transformNode.Children.ContainsKey("translate"))
                transformation *=
                    GetScaleTransform(
                        transformNode.Children[new YamlScalarNode("scale")]);

            return transformation;
        }

        private Material ParseMaterial(YamlMappingNode materialNode)
        {
            Color color = Color.Black;
            int shiny = 0;
            if (materialNode.Children.ContainsKey("color"))
                color = GetColorFromNode(materialNode.Children[new YamlScalarNode("color")]);
            if (materialNode.Children.ContainsKey("shininess"))
                shiny = GetIntFromNode(materialNode.Children[new YamlScalarNode("shininess")]);
            return new Material()
            {
                Color = color,
                Shininess = shiny,
            };
        }

        private Matrix GetTranslationsTransform(YamlNode node)
            => Transformation.Translation(
                    double.Parse(node[0].ToString()),
                    double.Parse(node[1].ToString()),
                    double.Parse(node[2].ToString())
                    );

        private Matrix GetScaleTransform(YamlNode node)
            => Transformation.Scaling(
                    double.Parse(node[0].ToString()),
                    double.Parse(node[1].ToString()),
                    double.Parse(node[2].ToString())
                    );

        private int GetIntFromNode(YamlNode node)
            => int.Parse(node.ToString());

        private double GetDoubleFromNode(YamlNode node)
            => double.Parse(node.ToString());

        private Point GetPointFromNode(YamlNode node)
            => new Point(
                    double.Parse(node[0].ToString()),
                    double.Parse(node[1].ToString()),
                    double.Parse(node[2].ToString())
                    );

        private Vector GetVectorFromNode(YamlNode node)
            => new Vector(
                    double.Parse(node[0].ToString()),
                    double.Parse(node[1].ToString()),
                    double.Parse(node[2].ToString())
                    );

        private Color GetColorFromNode(YamlNode node)
            => new Color(
                    double.Parse(node[0].ToString()),
                    double.Parse(node[1].ToString()),
                    double.Parse(node[2].ToString())
                    );
    }
}