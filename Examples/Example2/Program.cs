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

            var metrics = new Metrics(new HttpJsonPostEmitter(postUrl, httpService));

            metrics.Entry("Metric entry", "foo");
        }
    }
}
