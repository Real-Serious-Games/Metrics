using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RSG;

namespace Example3
{
    /// <summary>
    /// An example using the metrics system to send metrics via JSON to an HTTP service.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var httpService = new SimpleHttpService();
            var postUrl = "http://localhost:3000/emit";

            // Create the metrics with batching.
            var metrics = new Metrics(new HttpJsonPostEmitter(postUrl, httpService), 5);

            // Create some dummy metrics data
            metrics.SetProperty("Metrics entry", "test");
            for (var i = 0; i < 10; i++)
            {
                metrics.Entry("Test int entry", i);
            }
            metrics.SetProperty("Second property", "true");
            metrics.Event("An event occurred");
            metrics.Entry("Test string entry", "foo");

            // Flush the metrics before in case there are still queued metrics.
            metrics.Flush();

            Console.WriteLine("Sent metrics data to " + postUrl);
        }
    }
}
