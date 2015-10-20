using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG
{
    /// <summary>
    /// Non-generic interface for metrics to implement.
    /// </summary>
    public interface IMetric { }

    /// <summary>
    /// Metric content.
    /// </summary>
    public struct Metric<T> : IMetric
    {
        public string Name;
        public DateTime TimeStamp;
        public string Type;
        public T Content;
    }
}
