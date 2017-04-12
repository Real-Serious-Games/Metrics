using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG
{
    /// <summary>
    /// Interface for classes that process metrics to implement.
    /// </summary>
    public interface IMetricsEmitter
    {
        void Emit(IDictionary<string, string> properties, Metric[] metrics); 
    }
}
