using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RSG;

namespace Example2
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

            // Create some dummy metrics data
            var metrics = new Metrics(new HttpJsonPostEmitter(postUrl, httpService));
            metrics.SetProperty("Metrics entry", "test");
            for (var i = 0; i < 10; i++)
            {
                metrics.Entry("Test int entry", i);
            }
            metrics.SetProperty("Second property", "true");
            metrics.Event("An event occurred");
            metrics.Entry("Test string entry", "foo");

            Console.WriteLine("Sent metrics data to " + postUrl);
        }
    }
}
