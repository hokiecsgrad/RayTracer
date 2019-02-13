using System;

namespace RayTracer
{
    public class Color
    {
        private const double EPSILON = 0.0001;
        public double Red, Green, Blue;

        public Color(double red, double green, double blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public static Color operator*(Color a, Color b)
        {
            return new Color(a.Red * b.Red, a.Green * b.Green, a.Blue * b.Blue);
        }

        public static Color operator*(Color a, double multiplier)
        {
            return new Color(a.Red * multiplier, 
                             a.Green * multiplier, 
                             a.Blue * multiplier);
        }

        public static Color operator+(Color a, Color b)
        {
            return new Color(a.Red + b.Red, 
                             a.Green + b.Green, 
                             a.Blue + b.Blue);
        }

        public static Color operator-(Color a, Color b)
        {
            return new Color(a.Red - b.Red, 
                              a.Green - b.Green, 
                              a.Blue - b.Blue);
        }

        public override bool Equals(Object other)
        {
            Color objTuple = other as Color;

            if (objTuple == null) {
                return false;
            }
 
            return (Math.Abs(objTuple.Red - this.Red) < EPSILON) && 
                    (Math.Abs(objTuple.Green - this.Green) < EPSILON) && 
                    (Math.Abs(objTuple.Blue - this.Blue) < EPSILON);
        }
   }
}