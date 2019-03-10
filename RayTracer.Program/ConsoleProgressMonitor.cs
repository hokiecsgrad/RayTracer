/*
    Code originally authored by @basp on Github
    https://github.com/basp/pixie.net/blob/master/src/Pixie.Cmd/ConsoleProgressMonitor.cs
*/

using System;
using System.Diagnostics;
using RayTracer;

namespace RayTracer.Program
{
    public class ConsoleProgressMonitor : ProgressMonitor
    {
        private readonly int rows;
        private readonly Stopwatch sw = new Stopwatch();

        public ConsoleProgressMonitor(int rows)
        {
            this.rows = rows;
        }

        public override void OnRowStarted(int row)
        {
            this.sw.Start();
        }

        public override void OnRowFinished(int row)
        {
            this.sw.Stop();
            Console.WriteLine($"{row++}/{rows} ({sw.Elapsed})");
            this.sw.Reset();
        }
    }
}