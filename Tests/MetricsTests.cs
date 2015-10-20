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

            mockMetricsEmitter
                .Verify(m => m.Emit(It.IsAny<Dictionary<string, string>>(), It.IsAny<IMetric[]>()), Times.Once());
        }

        [Fact]
        public void entry_with_string_contains_entry_name()
        {
            Init();

            const string name = "TestEntry";

            string emittedEntryName = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<Dictionary<string, string>>(), It.IsAny<IMetric[]>()))
                .Callback<Dictionary<string, string>, IMetric[]>((properties, metrics) => {
                    var entry = (Metric<string>) metrics[0];
                    emittedEntryName = entry.Name;
                });

            testObject.Entry(name, "Test content");

            Assert.Equal(name, emittedEntryName);
        }

        [Fact]
        public void entry_with_string_contains_string()
        {
            Init();

            const string content = "Test content";

            string emittedContent = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<Dictionary<string, string>>(), It.IsAny<IMetric[]>()))
                .Callback<Dictionary<string, string>, IMetric[]>((properties, metrics) => {
                    var entry = (Metric<string>) metrics[0];
                    emittedContent = entry.Content;
                });

            testObject.Entry("TestEntry", content);

            Assert.Equal(content, emittedContent);
        }

        [Fact]
        public void set_property_includes_property_on_first_message()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void set_property_sets_property_string()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void set_property_sets_property_name()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void set_property_includes_property_on_subsequent_messages()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void set_property_updates_existing_property()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void remove_property_stops_including_properties_on_messages()
        {
            throw new NotImplementedException();
        }
    }
}
