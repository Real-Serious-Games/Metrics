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
                .Verify(m => m.Emit(It.IsAny<Dictionary<string, string>>(), It.IsAny<object[]>()), Times.Once());
        }

        [Fact]
        public void entry_with_string_contains_entry_name()
        {
            Init();

            var name = "TestEntry";

            /*mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<Dictionary<string, string>>(), It.IsAny<object[]>()))
                .Callback<Dictionary<string, string>, object[]>((properties, metrics) => {

                })*/
            throw new NotImplementedException();
        }

        [Fact]
        public void entry_with_string_contains_string()
        {
            throw new NotImplementedException();
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
