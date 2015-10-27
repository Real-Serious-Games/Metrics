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

        public static readonly string stringTypeName = typeof(string).Name;
        public static readonly string intTypeName = typeof(int).Name;
        public static readonly string floatTypeName = typeof(float).Name;
        public static readonly string incTypeName = "inc";
        public static readonly string eventTypeName = "event";

        public Metrics(IMetricsEmitter emitter, int batchSize = 1)
        {
            if (emitter == null)
            {
                throw new ArgumentNullException();
            }
            if (batchSize < 1)
            {
                throw new ArgumentException("Batch size must be at least 1");
            }
            
            this.emitter = emitter;

            properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Add a metrics entry string.
        /// </summary>
        public void Entry(string name, string data)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == String.Empty)
            {
                throw new ArgumentException();
            }
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            // Set up the metric object
            var metric = new Metric()
            {
                Name = name,
                Data = data,
                Type = stringTypeName,
                TimeStamp = DateTime.Now
            };

            // Emit the entry using our emitter
            emitter.Emit(properties, new Metric[] { metric });
        }

        /// <summary>
        /// Add a metrics entry that is an integer.
        /// </summary>
        public void Entry(string name, int data)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == String.Empty)
            {
                throw new ArgumentException();
            }

            var metric = new Metric()
            {
                Name = name,
                Data = data.ToString(),
                Type = intTypeName,
                TimeStamp = DateTime.Now
            };

            emitter.Emit(properties, new Metric[] { metric });
        }

        /// <summary>
        /// Add a metrics entry that is a float.
        /// </summary>
        public void Entry(string name, float data)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == String.Empty)
            {
                throw new ArgumentException();
            }

            var metric = new Metric()
            {
                Name = name,
                Data = data.ToString(),
                Type = floatTypeName,
                TimeStamp = DateTime.Now
            };

            emitter.Emit(properties, new Metric[] { metric });
        }

        /// <summary>
        /// Sends an increment metric to the emitter.
        /// </summary>
        public void Inc(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == String.Empty)
            {
                throw new ArgumentException();
            }

            var metric = new Metric()
            {
                Name = name,
                Type = incTypeName,
                TimeStamp = DateTime.Now
            };

            emitter.Emit(properties, new Metric[] { metric });
        }

        /// <summary>
        /// Sends an event metric to the emitter.
        /// </summary>
        public void Event(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == String.Empty)
            {
                throw new ArgumentException();
            }

            var metric = new Metric()
            {
                Name = name,
                Type = eventTypeName,
                TimeStamp = DateTime.Now
            };

            emitter.Emit(properties, new Metric[] { metric });
        }

        /// <summary>
        /// Set a property for all subsequent messages.
        /// </summary>
        public void SetProperty(string name, string property)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == String.Empty)
            {
                throw new ArgumentException();
            }
            if (property == null)
            {
                throw new ArgumentNullException();
            }
            if (property == String.Empty)
            {
                throw new ArgumentException();
            }
            
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
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == String.Empty)
            {
                throw new ArgumentException();
            }

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
