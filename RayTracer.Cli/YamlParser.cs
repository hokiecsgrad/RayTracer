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
        public Dictionary<string, Material> Materials { get; private set; }
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
            Materials = ParseMaterials();
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

        public Camera ParseCamera()
        {
            if (_yamlRoot is null) CreateMappingNode();
            if (!_yamlRoot.Children.ContainsKey("camera")) return null;

            YamlNode cameraData = _yamlRoot.Children[new YamlScalarNode("camera")];

            Point from = GetPointFromNode(cameraData["from"]);
            Point to = GetPointFromNode(cameraData["to"]);
            Vector up = GetVectorFromNode(cameraData["up"]);

            return new Camera()
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
            ValidateLights();

            List<ILight> lights = new();
            YamlSequenceNode lightsNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("lights")];

            foreach (YamlMappingNode light in lightsNode)
            {
                string type = light.Children[new YamlScalarNode("type")].ToString();
                Point position =
                    GetPointFromNode(light.Children[new YamlScalarNode("at")]);
                Color intensity =
                    GetColorFromNode(light.Children[new YamlScalarNode("intensity")]);

                lights.Add(CreateLight(type, position, intensity));
            }

            return lights;
        }

        private ILight CreateLight(string type, Point position, Color color)
            => type switch
            {
                "point" => new PointLight(position, color),
                _ => throw new Exception($"Unsupportd light type, {type}!"),
            };

        public Dictionary<string, Material> ParseMaterials()
        {
            if (_yamlRoot is null) CreateMappingNode();
            ValidateMaterials();

            if (!_yamlRoot.Children.ContainsKey("materials")) return null;

            Dictionary<string, Material> materials = new();
            YamlSequenceNode materialsNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("materials")];
            string name = String.Empty;

            foreach (YamlMappingNode material in materialsNode)
            {
                name = material.Children[new YamlScalarNode("name")].ToString();
                Material currMaterial = ParseMaterial(material);
                materials.Add(name, currMaterial);
            }

            return materials;
        }

        public List<Shape> ParseShapes()
        {
            if (_yamlRoot is null) CreateMappingNode();
            ValidateShapes();

            List<Shape> shapes = new();
            YamlSequenceNode shapesNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("shapes")];

            foreach (YamlMappingNode shape in shapesNode)
            {
                Matrix transformations = Matrix.Identity;
                Material material = null;

                string type = shape.Children[new YamlScalarNode("type")].ToString();

                if (shape.Children.ContainsKey("transform"))
                    transformations =
                        ParseTransformations((YamlMappingNode)shape.Children[new YamlScalarNode("transform")]);

                if (shape.Children.ContainsKey("material"))
                {
                    YamlMappingNode materialNode = (YamlMappingNode)shape.Children[new YamlScalarNode("material")];
                    if (materialNode.Children.ContainsKey("name"))
                    {
                        material = Materials[materialNode.Children[new YamlScalarNode("name")].ToString()];
                    }
                    else
                    {
                        material =
                            ParseMaterial((YamlMappingNode)shape.Children[new YamlScalarNode("material")]);
                    }
                }

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
                _ => throw (new Exception($"Unsupported shape, {type}!")),
            };

        private Matrix ParseTransformations(YamlMappingNode transformNode)
        {
            Matrix transformation = Matrix.Identity;

            if (transformNode.Children.ContainsKey("translate"))
                transformation *=
                    GetTranslationsTransform(
                        transformNode.Children[new YamlScalarNode("translate")]);

            if (transformNode.Children.ContainsKey("scale"))
                transformation *=
                    GetScaleTransform(
                        transformNode.Children[new YamlScalarNode("scale")]);

            if (transformNode.Children.ContainsKey("rotate-x"))
                transformation *= Transformation.Rotation_x(
                    GetDoubleFromNode(
                        transformNode.Children[new YamlScalarNode("rotate-x")])
                );

            if (transformNode.Children.ContainsKey("rotate-y"))
                transformation *= Transformation.Rotation_y(
                    GetDoubleFromNode(
                        transformNode.Children[new YamlScalarNode("rotate-y")])
                );

            if (transformNode.Children.ContainsKey("rotate-z"))
                transformation *= Transformation.Rotation_z(
                    GetDoubleFromNode(
                        transformNode.Children[new YamlScalarNode("rotate-z")])
                );

            return transformation;
        }

        private Material ParseMaterial(YamlMappingNode materialNode)
        {
            Material material = new Material();
            Pattern pattern = null;
            Color color = material.Color;
            double ambient = material.Ambient;
            double diffuse = material.Diffuse;
            double specular = material.Specular;
            double shiny = material.Shininess;
            double reflective = material.Reflective;
            double transparency = material.Transparency;
            double refractive = material.RefractiveIndex;

            if (materialNode.Children.ContainsKey("pattern"))
                pattern = ParsePatternFromNode((YamlMappingNode)materialNode.Children[new YamlScalarNode("pattern")]);
            if (materialNode.Children.ContainsKey("color"))
                color = GetColorFromNode(materialNode.Children[new YamlScalarNode("color")]);
            if (materialNode.Children.ContainsKey("ambient"))
                ambient = GetDoubleFromNode(materialNode.Children[new YamlScalarNode("ambient")]);
            if (materialNode.Children.ContainsKey("diffuse"))
                diffuse = GetDoubleFromNode(materialNode.Children[new YamlScalarNode("diffuse")]);
            if (materialNode.Children.ContainsKey("specular"))
                specular = GetDoubleFromNode(materialNode.Children[new YamlScalarNode("specular")]);
            if (materialNode.Children.ContainsKey("shininess"))
                shiny = GetIntFromNode(materialNode.Children[new YamlScalarNode("shininess")]);
            if (materialNode.Children.ContainsKey("reflective"))
                reflective = GetDoubleFromNode(materialNode.Children[new YamlScalarNode("reflective")]);
            if (materialNode.Children.ContainsKey("transparency"))
                transparency = GetDoubleFromNode(materialNode.Children[new YamlScalarNode("transparency")]);
            if (materialNode.Children.ContainsKey("refractive-index"))
                refractive = GetDoubleFromNode(materialNode.Children[new YamlScalarNode("refractive-index")]);

            material = new Material()
            {
                Color = color,
                Ambient = ambient,
                Diffuse = diffuse,
                Specular = specular,
                Shininess = shiny,
                Reflective = reflective,
                Transparency = transparency,
                RefractiveIndex = refractive,
            };

            if (pattern != null)
                material.Pattern = pattern;

            return material;
        }

        private Pattern ParsePatternFromNode(YamlMappingNode patternNode)
        {
            Matrix transformations = Matrix.Identity;
            string type = patternNode.Children[new YamlScalarNode("type")].ToString();

            Color colorOne = GetColorFromNode(patternNode.Children[new YamlScalarNode("colors")][0]);
            Color colorTwo = GetColorFromNode(patternNode.Children[new YamlScalarNode("colors")][1]);

            if (patternNode.Children.ContainsKey("transform"))
                transformations =
                    ParseTransformations((YamlMappingNode)patternNode.Children[new YamlScalarNode("transform")]);

            Pattern pattern = type switch
            {
                "gradient" => new Gradient(colorOne, colorTwo) { Transform = transformations },
                "stripes" => new Stripe(colorOne, colorTwo) { Transform = transformations },
                "ring" => new Ring(colorOne, colorTwo) { Transform = transformations },
                "checkers" => new Checkers(colorOne, colorTwo) { Transform = transformations },
                _ => throw (new Exception($"Unsupported pattern, {type}!")),
            };

            return pattern;
        }

        private void ValidateSceneElements()
        {
            if (!_yamlRoot.Children.ContainsKey("camera") &&
                    _yamlRoot.Children.Count != 6)
                throw new ArgumentException("Scene must contain a valid camera configuration.");

            if (!_yamlRoot.Children.ContainsKey("lights"))
                throw new ArgumentException("Scene must contain a section for lights.");

            if (!_yamlRoot.Children.ContainsKey("shapes"))
                throw new ArgumentException("Scene must contain a section for shapes.");
        }

        private void ValidateMaterials()
        {
            if (!_yamlRoot.Children.ContainsKey("materials")) return;

            YamlSequenceNode materialsNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("materials")];

            foreach (YamlMappingNode shape in materialsNode)
            {
                if (!shape.Children.ContainsKey("name"))
                    throw new ArgumentException("Materials defintion is missing a required name.");
            }
        }

        private void ValidateShapes()
        {
            if (_yamlRoot.Children.Count < 1)
                throw new ArgumentException("Scene must contain at least one shape.");

            YamlSequenceNode shapesNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("shapes")];

            foreach (YamlMappingNode shape in shapesNode)
            {
                if (!shape.Children.ContainsKey("type"))
                    throw new ArgumentException("Shape definition is missing a type.");

                string type = shape.Children[new YamlScalarNode("type")].ToString();
            }
        }

        private void ValidateLights()
        {
            if (_yamlRoot.Children.Count < 1)
                throw new ArgumentException("Scene must contain at least one light source.");

            YamlSequenceNode lightsNode =
                (YamlSequenceNode)_yamlRoot.Children[new YamlScalarNode("lights")];

            foreach (YamlMappingNode light in lightsNode)
            {
                if (!light.Children.ContainsKey("type"))
                    throw new ArgumentException("Light definition is missing a type.");

                string type = light.Children[new YamlScalarNode("type")].ToString();

                if (!light.Children.ContainsKey("at"))
                    throw new ArgumentException($"Light, {type}, is missing a position.");

                if (!light.Children.ContainsKey("intensity"))
                    throw new ArgumentException($"Light, {type}, is missing a color definition.");
            }
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