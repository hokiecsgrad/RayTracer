using System;
using System.IO;

namespace RayTracer
{
    public class PpmWriter
    {
        private const string PPM_FILE_TYPE = "P3";
        private const int MAX_COLOR_VALUE = 255;

        public static void WriteCanvasToPpm(string filename, Canvas canvas)
        {
            using (var s = File.OpenWrite(filename))
            using (var writer = new StreamWriter(s))
            {            
                writer.WriteLine(PPM_FILE_TYPE);
                writer.WriteLine($"{canvas.Width} {canvas.Height}");
                writer.WriteLine(MAX_COLOR_VALUE);

                for (int h = 0; h < canvas.Height; h++)
                {
                    int lineLength = 0;
                    for (int w = 0; w < canvas.Width; w++)
                    {
                        Color color = canvas.GetPixel(w, h);
                        string red = PpmWriter.Clamped(color.Red).ToString();
                        string green = PpmWriter.Clamped(color.Green).ToString();
                        string blue = PpmWriter.Clamped(color.Blue).ToString();

                        lineLength += red.Length + 1;
                        if (lineLength > 70) { lineLength = red.Length; writer.WriteLine(); }
                        writer.Write($"{red} ");
                        lineLength += green.Length + 1;
                        if (lineLength > 70) { lineLength = green.Length; writer.WriteLine(); }
                        writer.Write($"{green} ");
                        lineLength += blue.Length + 1;
                        if (lineLength > 70) { lineLength = blue.Length; writer.WriteLine(); }
                        writer.Write($"{blue} ");
                    }
                    writer.WriteLine();
                }
            }
        }

        public static int Clamped(double value)
        {
            if (value < 0.0) return 0;
            if (value > 1.0) return MAX_COLOR_VALUE;
            return (int)Math.Round(MAX_COLOR_VALUE * value);
        }
    }
}