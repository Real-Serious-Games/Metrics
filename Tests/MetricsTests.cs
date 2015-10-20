using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.MetricsTests
{
    public class MetricsTests
    {
        Metrics testObject;

        Mock<IMetricsEmitter> mockMetricsEmitter;

        void Init()
        {
            mockMetricsEmitter = new Mock<IMetricsEmitter>();

            testObject = new Metrics(mockMetricsEmitter.Object);
        }

        [Fact]
        public void entry_with_string_emits_message()
        {
            Init();

            testObject.Entry("TestEntry", "Testing");

            mockMetricsEmitter.Verify(m => m.Emit(It.IsAny<Dictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
        }
    }
}
