using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG
{
    /// <summary>
    /// Interface for
    /// </summary>
    public interface IMetricsEmitter
    {
        void Emit(Dictionary<string, string> properties, Metric[] metrics); 
    }
}
