using RSG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Example1
{
    /// <summary>
    /// A metrics emitter that writes metrics to a specified text (csv) file. 
    /// </summary>
    class TextLogEmitter : IMetricsEmitter
    {
        private string logFilePath;

        /// <summary>
        /// Create the metrics emitter, specifying the path of the file to write to.
        /// 
        /// If this file doesn't exist it will be created, otherwise new entries
        /// will just be appended to the end of the file.
        /// </summary>
        public TextLogEmitter(string logFilePath)
        {
            this.logFilePath = logFilePath;

            // Only create the file if it doesn't already exist, otherwise we can just
            // skip this and append to the existing file.
            if (!File.Exists(logFilePath))
            {
                using (var streamWriter = File.CreateText(logFilePath))
                {
                    // Write header with column names
                    streamWriter.WriteLine("Name, Data, Timestamp, Type, Properties");
                }
            }
        }

        /// <summary>
        /// Emit an array of metrics entries.
        /// </summary>
        public void Emit(IDictionary<string, string> properties, Metric[] metrics)
        {
            using (var streamWriter = File.AppendText(logFilePath))
            {
                foreach (var metric in metrics)
                {
                    streamWriter.WriteLine(MetricToString(properties, metric));
                }
            }
        }

        /// <summary>
        /// Get a single line string to write out to our file for a given metric.
        /// </summary>
        private string MetricToString(IDictionary<string, string> properties, Metric metric)
        {
            return metric.Name + ", " +
                metric.Data + ", " + 
                metric.TimeStamp.ToString() + ", " + 
                metric.Type + ", " + 
                "{" + String.Join("; ", properties
                    .Select(property => property.Key + ": " + property.Value)
                    .ToArray()) + "}";

        }
    }
}
