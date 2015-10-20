using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG
{
    public interface IMetricsEmitter
    {
        void Emit(Dictionary<string, string> properties, IMetric[] metrics); 
    }
}
