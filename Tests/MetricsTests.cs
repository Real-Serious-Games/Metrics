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

        void InitWithoutBatching()
        {
            mockMetricsEmitter = new Mock<IMetricsEmitter>();

            testObject = new Metrics(mockMetricsEmitter.Object, 1);
        }

        void InitWithBatchSize(int batchSize)
        {
            mockMetricsEmitter = new Mock<IMetricsEmitter>();

            testObject = new Metrics(mockMetricsEmitter.Object, batchSize);
        }

        /// <summary>
        /// Helper function to check if two dictionaries are the same.
        /// </summary>
        bool DictionaryEquals<TKey, TValue>(IDictionary<TKey, TValue> expected, IDictionary<TKey, TValue> actual)
        {
            // Check that the two dictionaries are the same length.
            if (expected.Count != actual.Count)
            {
                return false;
            }

            // Check that each element is the same.
            foreach (var pair in expected)
            {
                TValue value;
                if (actual.TryGetValue(pair.Key, out value))
                {
                    // Check that each value is the same
                    if (!value.Equals(pair.Value))
                    {
                        return false;
                    }
                }
                else // Require key to be present.
                {
                    return false;
                }
            }

            return true;
        }

        [Fact]
        public void entry_with_string_emits_message()
        {
            InitWithoutBatching();

            testObject.Entry("TestEntry", "Testing");

            mockMetricsEmitter
                .Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
        }

        [Fact]
        public void entry_with_string_contains_entry_name()
        {
            InitWithoutBatching();

            const string name = "TestEntry";

            string emittedEntryName = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedEntryName = entry.Name;
                });

            testObject.Entry(name, "Test content");

            Assert.Equal(name, emittedEntryName);
        }

        [Fact]
        public void entry_with_string_contains_string()
        {
            InitWithoutBatching();

            const string content = "Test content";

            string emittedData = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedData = entry.Data;
                });

            testObject.Entry("TestEntry", content);

            Assert.Equal(content, emittedData);
        }

        [Fact]
        public void entry_with_string_has_correct_type()
        {
            InitWithoutBatching();

            string emittedType = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedType = entry.Type;
                });

            testObject.Entry("TestEntry", "data");

            Assert.Equal(Metrics.stringTypeName, emittedType);
        }

        [Fact]
        public void entry_with_string_has_correct_timestamp()
        {
            InitWithoutBatching();

            var timeStamp = DateTimeOffset.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTimeOffset.Now;

            testObject.Entry("TestEntry", "data");

            var timeAfter = DateTimeOffset.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }

        [Fact]
        public void entry_with_int_emits_message()
        {
            InitWithoutBatching();

            testObject.Entry("TestEntry", 99);

            mockMetricsEmitter
                .Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
        }

        [Fact]
        public void entry_with_int_contains_entry_name()
        {
            InitWithoutBatching();

            const string name = "TestEntry";

            string emittedEntryName = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedEntryName = entry.Name;
                });

            testObject.Entry(name, 99);

            Assert.Equal(name, emittedEntryName);
        }

        [Fact]
        public void entry_with_int_contains_int()
        {
            InitWithoutBatching();

            const int content = 512;

            string emittedData = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedData = entry.Data;
                });

            testObject.Entry("TestEntry", content);

            Assert.Equal(content.ToString(), emittedData);
        }

        [Fact]
        public void entry_with_int_has_correct_type()
        {
            InitWithoutBatching();

            string emittedType = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedType = entry.Type;
                });

            testObject.Entry("TestEntry", 64);

            Assert.Equal(Metrics.intTypeName, emittedType);
        }

        [Fact]
        public void entry_with_int_has_correct_timestamp()
        {
            InitWithoutBatching();

            var timeStamp = DateTimeOffset.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTimeOffset.Now;

            testObject.Entry("TestEntry", 64);

            var timeAfter = DateTimeOffset.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }

        [Fact]
        public void entry_with_float_emits_message()
        {
            InitWithoutBatching();

            testObject.Entry("TestEntry", 99.5f);

            mockMetricsEmitter
                .Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
        }

        [Fact]
        public void entry_with_float_contains_entry_name()
        {
            InitWithoutBatching();

            const string name = "TestEntry";

            string emittedEntryName = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedEntryName = entry.Name;
                });

            testObject.Entry(name, 99.1f);

            Assert.Equal(name, emittedEntryName);
        }

        [Fact]
        public void entry_with_float_contains_int()
        {
            InitWithoutBatching();

            const float content = 12.41f;

            string emittedData = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedData = entry.Data;
                });

            testObject.Entry("TestEntry", content);

            Assert.Equal(content.ToString(), emittedData);
        }

        [Fact]
        public void entry_with_float_has_correct_type()
        {
            InitWithoutBatching();

            string emittedType = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedType = entry.Type;
                });

            testObject.Entry("TestEntry", 64.1235f);

            Assert.Equal(Metrics.floatTypeName, emittedType);
        }

        [Fact]
        public void entry_with_float_has_correct_timestamp()
        {
            InitWithoutBatching();

            var timeStamp = DateTimeOffset.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTimeOffset.Now;

            testObject.Entry("TestEntry", 64.123f);

            var timeAfter = DateTimeOffset.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }

        [Fact]
        public void no_properties_are_included_by_default()
        {
            InitWithoutBatching();

            testObject.Entry("TestEvent", "data");

            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(properties => properties.Count == 0),
                    It.IsAny<Metric[]>()));
        }

        [Fact]
        public void set_property_includes_property_on_first_message()
        {
            InitWithoutBatching();

            const string propertyKey = "key";
            const string propertyValue = "value";

            testObject.SetProperty(propertyKey, propertyValue);

            testObject.Entry("TestEntry", "data");

            var expectedProperties = new Dictionary<string, string>();
            expectedProperties.Add(propertyKey, propertyValue);
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()),
                    Times.Once());
        }

        [Fact]
        public void set_property_includes_property_on_subsequent_messages()
        {
            InitWithoutBatching();

            const string propertyKey = "key";
            const string propertyValue = "value";

            testObject.SetProperty(propertyKey, propertyValue);

            testObject.Entry("TestEntry1", "data");
            testObject.Entry("TestEntry2", "data");
            testObject.Entry("TestEntry3", "data");

            var expectedProperties = new Dictionary<string, string>();
            expectedProperties.Add(propertyKey, propertyValue);
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()),
                    Times.Exactly(3));
        }

        [Fact]
        public void set_property_updates_existing_property()
        {
            InitWithoutBatching();

            const string propertyKey = "key";
            const string propertyValue1 = "value";
            const string propertyValue2 = "something else";

            testObject.SetProperty(propertyKey, propertyValue1);

            testObject.Entry("TestEntry1", "data");

            var expectedProperties = new Dictionary<string, string>();
            expectedProperties.Add(propertyKey, propertyValue1);
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()),
                    Times.Once());

            testObject.SetProperty(propertyKey, propertyValue2);

            testObject.Entry("TestEntry2", "data");

            expectedProperties[propertyKey] = propertyValue2;
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()));
        }

        [Fact]
        public void set_property_appends_to_included_properties()
        {
            InitWithoutBatching();

            const string propertyKey1 = "firstKey";
            const string propertyKey2 = "secondKey";
            const string propertyValue1 = "value";
            const string propertyValue2 = "something else";

            testObject.SetProperty(propertyKey1, propertyValue1);

            testObject.Entry("TestEntry1", "data");

            var expectedProperties = new Dictionary<string, string>();
            expectedProperties.Add(propertyKey1, propertyValue1);
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()),
                    Times.Once());

            testObject.SetProperty(propertyKey2, propertyValue2);

            testObject.Entry("TestEntry2", "data");

            expectedProperties.Add(propertyKey2, propertyValue2);
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()));
        }

        [Fact]
        public void remove_property_stops_including_properties_on_messages()
        {
            InitWithoutBatching();

            testObject.SetProperty("key1", "value");
            testObject.SetProperty("key2", "value");

            testObject.RemoveProperty("key1");

            testObject.Entry("TestEntry", "data");

            var expectedProperties = new Dictionary<string, string>();
            expectedProperties.Add("key2", "value");
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()));
        }

        [Fact]
        public void remove_property_on_invalid_property_throws_exception()
        {
            InitWithoutBatching();

            Assert.Throws<ApplicationException>(() => testObject.RemoveProperty("undefined property"));
        }

        [Fact]
        public void inc_emits_metric()
        {
            InitWithoutBatching();

            testObject.Inc("counter");

            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()),
                    Times.Once());
        }

        [Fact]
        public void inc_contains_all_properties()
        {
            InitWithoutBatching();

            testObject.SetProperty("Property1", "foo");
            testObject.SetProperty("Property2", "bar");

            testObject.Inc("counter");

            var expectedProperties = new Dictionary<string, string>();
            expectedProperties.Add("Property1", "foo");
            expectedProperties.Add("Property2", "bar");
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()),
                    Times.Once());
        }

        [Fact]
        public void inc_emits_name()
        {
            InitWithoutBatching();

            const string name = "Increment";

            var emittedName = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedName = entry.Name;
                });

            testObject.Inc(name);

            Assert.Equal(name, emittedName);
        }

        [Fact]
        public void inc_emits_correct_type()
        {
            InitWithoutBatching();

            var emittedType = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedType = entry.Type;
                });

            testObject.Inc("counter");

            Assert.Equal(Metrics.incTypeName, emittedType);
        }

        [Fact]
        public void inc_emits_correct_timestamp()
        {
            InitWithoutBatching();

            var timeStamp = DateTimeOffset.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTimeOffset.Now;

            testObject.Inc("counter");

            var timeAfter = DateTimeOffset.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }

        [Fact]
        public void event_emits_metric()
        {
            InitWithoutBatching();

            testObject.Inc("counter");

            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()),
                    Times.Once());
        }

        [Fact]
        public void event_contains_all_properties()
        {
            InitWithoutBatching();

            testObject.SetProperty("Property1", "foo");
            testObject.SetProperty("Property2", "bar");

            testObject.Inc("counter");

            var expectedProperties = new Dictionary<string, string>();
            expectedProperties.Add("Property1", "foo");
            expectedProperties.Add("Property2", "bar");
            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.Is<IDictionary<string, string>>(p => DictionaryEquals<string, string>(expectedProperties, p)),
                    It.IsAny<Metric[]>()),
                    Times.Once());
        }

        [Fact]
        public void event_emits_name()
        {
            InitWithoutBatching();

            const string name = "event";

            var emittedName = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedName = entry.Name;
                });

            testObject.Event(name);

            Assert.Equal(name, emittedName);
        }

        [Fact]
        public void event_emits_correct_type()
        {
            InitWithoutBatching();

            var emittedType = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedType = entry.Type;
                });

            testObject.Event("TestEvent");

            Assert.Equal(Metrics.eventTypeName, emittedType);
        }

        [Fact]
        public void event_emits_correct_timestamp()
        {
            InitWithoutBatching();

            var timeStamp = DateTimeOffset.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTimeOffset.Now;

            testObject.Event("TestEvent");

            var timeAfter = DateTimeOffset.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }

        [Fact]
        public void metrics_are_batched_into_correct_sizes()
        {
            var batchSize = 5;

            InitWithBatchSize(batchSize);

            var metricsBatchLength = 0;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    metricsBatchLength = metrics.Length;
                });

            for (var i = 0; i < batchSize + 1; i++)
            {
                testObject.Event(i.ToString());
            }

            mockMetricsEmitter.Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
            Assert.Equal(batchSize, metricsBatchLength);
        }

        [Fact]
        public void metrics_types_are_batched_togeather()
        {
            var batchSize = 5;

            InitWithBatchSize(batchSize);

            var metricsBatchLength = 0;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    metricsBatchLength = metrics.Length;
                });

            for (var i = 0; i < 2; i++)
            {
                testObject.Event(i.ToString());
                testObject.Entry("test", i);
                testObject.Inc(i.ToString());
            }

            mockMetricsEmitter.Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
            Assert.Equal(batchSize, metricsBatchLength);
        }

        [Fact]
        public void no_metrics_are_emited_when_queue_is_empty()
        {
            InitWithoutBatching();

            testObject.Flush();

            mockMetricsEmitter.Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Never());
        }

        [Fact]
        public void metrics_are_flushed_when_property_is_set()
        {
            var batchSize = 5;

            InitWithBatchSize(batchSize);

            var metricsBatchLength = 0;
            var propertiesLength = 0;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    metricsBatchLength = metrics.Length;
                    propertiesLength = properties.Count;
                });

            testObject.Event("Test");

            testObject.SetProperty("test", "property");

            mockMetricsEmitter.Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
            Assert.Equal(1, metricsBatchLength);
            Assert.Equal(0, propertiesLength);
        }

        [Fact]
        public void metrics_are_flushed_when_property_is_removed()
        {
            var batchSize = 5;

            InitWithBatchSize(batchSize);

            var metricsBatchLength = 0;
            var propertiesLength = 0;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    metricsBatchLength = metrics.Length;
                    propertiesLength = properties.Count;
                });

            testObject.SetProperty("test", "property");

            testObject.Event("Test");

            testObject.RemoveProperty("test");

            mockMetricsEmitter.Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
            Assert.Equal(1, metricsBatchLength);
            Assert.Equal(1, propertiesLength);
        }
    }
}
