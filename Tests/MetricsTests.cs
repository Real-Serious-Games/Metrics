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
            Init();

            testObject.Entry("TestEntry", "Testing");

            mockMetricsEmitter
                .Verify(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()), Times.Once());
        }

        [Fact]
        public void entry_with_string_contains_entry_name()
        {
            Init();

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
            Init();

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
            Init();

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
            Init();

            var timeStamp = DateTime.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTime.Now;

            testObject.Entry("TestEntry", "data");

            var timeAfter = DateTime.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }

        [Fact]
        public void set_property_includes_property_on_first_message()
        {
            Init();

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
            Init();

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
            Init();

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
            Init();

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
            Init();

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
            Init();

            Assert.Throws<ApplicationException>(() => testObject.RemoveProperty("undefined property"));
        }

        [Fact]
        public void inc_emits_metric()
        {
            Init();

            testObject.Inc("counter");

            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()),
                    Times.Once());
        }

        [Fact]
        public void inc_contains_all_properties()
        {
            Init();

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
            Init();

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
            Init();

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
            Init();

            var timeStamp = DateTime.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTime.Now;

            testObject.Inc("counter");

            var timeAfter = DateTime.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }

        [Fact]
        public void event_emits_metric()
        {
            Init();

            testObject.Inc("counter");

            mockMetricsEmitter
                .Verify(m => m.Emit(
                    It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()),
                    Times.Once());
        }

        [Fact]
        public void event_contains_all_properties()
        {
            Init();

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
            Init();

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
            Init();

            const string type = "event";

            var emittedType = String.Empty;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    emittedType = entry.Type;
                });

            testObject.Event("TestEvent");

            Assert.Equal(type, emittedType);
        }
        
        [Fact]
        public void event_emits_correct_timestamp()
        {
            Init();

            var timeStamp = DateTime.MinValue;

            mockMetricsEmitter
                .Setup(m => m.Emit(It.IsAny<IDictionary<string, string>>(), It.IsAny<Metric[]>()))
                .Callback<IDictionary<string, string>, Metric[]>((properties, metrics) =>
                {
                    var entry = metrics[0];
                    timeStamp = entry.TimeStamp;
                });

            var timeBefore = DateTime.Now;

            testObject.Event("TestEvent");

            var timeAfter = DateTime.Now;

            Assert.InRange(timeStamp, timeBefore, timeAfter);
        }
    }
}
