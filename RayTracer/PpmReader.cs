using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class PpmReader
    {
        public static Canvas ReadCanvasFromPpm(TextReader reader)
        {
            var canvas = new Canvas(0,0);
            var colorValues = new List<int>();

            string line;
            var x = 0;
            var y = 0;
            var colorScale = 1.0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == string.Empty || line[0] == '#') continue;

                switch (y)
                {
                    case 0 :
                        if (line != "P3") return canvas;
                        y += 1;
                        break;

                    case 1 :
                        var dimensions = line.Split(' ');

                        canvas = new Canvas(
                            int.Parse(dimensions[0]), 
                            int.Parse(dimensions[1]));

                        y += 1;
                        break;

                    case 2 :
                        colorScale = double.Parse(line);
                        y += 1;
                        break;

                    default :
                        var ints = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < ints.Length; i++)
                            colorValues.Add(int.Parse(ints[i]));
                        break;
                }
            }

            y = 0;
            for (int i = 0; i < colorValues.Count; i += 3)
            {
                var pixel = new Color(
                    colorValues[i] / colorScale, 
                    colorValues[i+1] / colorScale, 
                    colorValues[i+2] / colorScale);

                canvas.SetPixel(x, y, pixel);

                x++;
                if (x % canvas.Width == 0)
                { 
                    x = 0;
                    y += 1;
                }
            }

            return canvas;
        }
    }
}