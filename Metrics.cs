using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG
{
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
            var metric = new {
                name = name,
                content = content
            };

            // Emit the entry using our emitter
            emitter.Emit(new Dictionary<string, string>(), new object[] { metric });
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
