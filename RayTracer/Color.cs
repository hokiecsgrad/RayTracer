using System;
using System.Collections.Generic;

namespace RayTracer
{
    public struct Color
    {
        public readonly double Red;
        public readonly double Green; 
        public readonly double Blue;

        public static Color White =>
            new Color(1, 1, 1);

        public static Color Black =>
            new Color(0, 0, 0);

        public Color(double red, double green, double blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public static Color operator +(Color a, Color b) =>
            new Color(
                a.Red + b.Red, 
                a.Green + b.Green, 
                a.Blue + b.Blue);

        public static Color operator -(Color a, Color b) =>
            new Color(
                a.Red - b.Red, 
                a.Green - b.Green, 
                a.Blue - b.Blue);

        public static Color operator *(Color a, Color b) =>
            new Color(
                a.Red * b.Red,
                a.Green * b.Green,
                a.Blue * b.Blue);

        public static Color operator *(Color a, double multiplier) =>
            new Color(
                a.Red * multiplier, 
                a.Green * multiplier, 
                a.Blue * multiplier);

        public static Color operator *(double multiplier, Color a) => a * multiplier;

        public static Color operator /(Color a, double divisor) =>
            new Color(
                a.Red / divisor, 
                a.Green / divisor, 
                a.Blue / divisor);

        public static Color operator /(double divisor, Color a) => a / divisor;

        public static bool operator <(Color a, Color b) =>
            a.Red < b.Red && 
            a.Green < b.Green && 
            a.Blue < b.Blue;

        public static bool operator >(Color a, Color b) =>
            a.Red > b.Red && 
            a.Green > b.Green && 
            a.Blue > b.Blue;

        public static IEqualityComparer<Color> GetEqualityComparer(double epsilon = 0.0) =>
            new ApproxColorEqualityComparer(epsilon);

        public override string ToString() =>
            $"({this.Red}, {this.Green}, {this.Blue})";
   }

    internal class ApproxColorEqualityComparer : ApproxEqualityComparer<Color>
    {
        public ApproxColorEqualityComparer(double epsilon = 0.0)
            : base(epsilon)
        {
        }

        public override bool Equals(Color x, Color y) =>
            this.ApproxEqual(x.Red, y.Red) &&
            this.ApproxEqual(x.Green, y.Green) &&
            this.ApproxEqual(x.Blue, y.Blue);

        public override int GetHashCode(Color obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + obj.Red.GetHashCode();
                hash = hash * 23 + obj.Green.GetHashCode();
                hash = hash * 23 + obj.Blue.GetHashCode();
                return hash;
            }
        }        
    }    
}