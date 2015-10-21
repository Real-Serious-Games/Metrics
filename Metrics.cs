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

        private IDictionary<string, string> properties;

        private static readonly string stringTypeName = typeof(string).Name;
        private static readonly string intTypeName = typeof(int).Name;
        private static readonly string floatTypeName = typeof(float).Name;
        private static readonly string incTypeName = "inc";

        public Metrics(IMetricsEmitter emitter)
        {
            this.emitter = emitter;

            properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Add a metrics entry string.
        /// </summary>
        public void Entry(string name, string data)
        {
            // Set up the metric object
            var metric = new Metric();
            metric.Name = name;
            metric.Data = data;
            metric.Type = stringTypeName;
            metric.TimeStamp = DateTime.Now;

            // Emit the entry using our emitter
            emitter.Emit(properties, new Metric[] { metric });
        }

        /// <summary>
        /// Add a metrics entry that is an integer.
        /// </summary>
        public void Entry(string name, int data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a metrics entry that is a float.
        /// </summary>
        public void Entry(string name, float data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends an increment metric to the emitter.
        /// </summary>
        public void Inc(string name)
        {
            var metric = new Metric();
            metric.Name = name;
            metric.Type = incTypeName;

            emitter.Emit(properties, new Metric[] { metric });
        }

        /// <summary>
        /// Sends an event metric to the emitter.
        /// </summary>
        public void Event(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set a property for all subsequent messages.
        /// </summary>
        public void SetProperty(string name, string property)
        {
            if (properties.ContainsKey(name))
            {
                properties[name] = property;
            }
            else
            {
                properties.Add(name, property);
            }
        }

        /// <summary>
        /// Finds the specified property and stops including it in subsequent messages.
        /// </summary>
        public void RemoveProperty(string name)
        {
            if (properties.ContainsKey(name))
            {
                properties.Remove(name);
            }
            else
            {
                throw new ApplicationException("Tried to remove a non-existent property from metrics.");
            }
        }
    }
}
