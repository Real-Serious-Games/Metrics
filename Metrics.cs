using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG
{
    /// <summary>
    /// Class for collecting and emitting metrics messages through an IMetricsEmitter.
    /// </summary>
    public class Metrics
    {
        private IMetricsEmitter emitter; 

        public Metrics(IMetricsEmitter emitter)
        {
            this.emitter = emitter;
        }

        /// <summary>
        /// Add a metrics entry string.
        /// </summary>
        public void Entry(string name, string content)
        {
            // Set up the metric object
            var metric = new Metric();
            metric.Name = name;
            metric.Data = content;

            // Emit the entry using our emitter
            emitter.Emit(new Dictionary<string, string>(), new Metric[] { metric });
        }

        /// <summary>
        /// Set a property for all subsequent messages.
        /// </summary>
        public void SetProperty(string name, string property)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the specified property and stops including it in subsequent messages.
        /// </summary>
        public void RemoveProperty(string name)
        {
            throw new NotImplementedException();
        }
    }
}
