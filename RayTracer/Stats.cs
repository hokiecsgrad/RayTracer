/*
    Code originally authored by @basp on Github
    https://github.com/basp/pixie.net/blob/master/src/Pixie.Core/Stats.cs
*/

using System;

namespace RayTracer
{
    public class Stats
    {
        public static long Tests = 0;

        public static long PrimaryRays = 0;

        public static long SecondaryRays = 0;

        public static long ShadowRays = 0;

        public static void Reset()
        {
            Tests = 0;
            PrimaryRays = 0;
            SecondaryRays = 0;
            ShadowRays = 0;
        }
    }
}