using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RSG;

namespace Example1
{
    /// <summary>
    /// A simple example program demonstrating using the metrics system to log metrics
    /// out to a text file.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Initialise metrics with a new TextLogEmitter
            var metrics = new Metrics(new TextLogEmitter("output.csv"));

            metrics.SetProperty("My property", "test");

            // Create some dummy metrics data
            for (var i = 0; i < 10; i++)
            {
                metrics.Entry("Test int entry", i);
            }

            metrics.SetProperty("Second property", "true");

            metrics.Event("An event occurred");

            metrics.Entry("Test string entry", "foo");

            Console.WriteLine("Metrics output written to otuput.csv");
        }
    }
}
