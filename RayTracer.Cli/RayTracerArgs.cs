using PowerArgs;

namespace RayTracer.Cli
{
    public class RayTracerArgs
    {
        [ArgDefaultValue(400)]
        [ArgDescription("The width of the output image")]
        public int width { get; set; }

        [ArgDefaultValue(300)]
        [ArgDescription("The height of the output image")]
        public int height { get; set; }

        [ArgDefaultValue(0.8)]
        [ArgDescription("The field of view for the scene")]
        public double fov { get; set; }

        [ArgDefaultValue(1)]
        [ArgDescription("The number of samples to take for each pixel")]
        public int n { get; set; }

        [ArgDefaultValue("Cover")]
        [ArgDescription("The name of the scene file to load")]
        public string scene { get; set; }

        [ArgDefaultValue("output.ppm")]
        [ArgDescription("The filename of the output file")]
        public string filename { get; set; }
    }
}