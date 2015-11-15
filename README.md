# Metrics
This is a simple metrics system for collecting data.

## Requirements

To use the metrics system you will need to implement the IMetricsEmitter interface. 

### IMetricsEmitter

A metrics emitter emit function has three parameters:

- properties: This is a dictionary of all current properties for the current batch of metrics.
- metrics: An array of all current cached metrics.
- metricsIndex: The index of the last current valid metric (Note: it is the responsibility of the emitter to remove the invalid metrics). 

```
	public class ExampleEmitter : IMetricsEmitter
    {
        public Emit(IDictionary<string, object> properties, Metric[] metrics, int metricsIndex)
        {
            // Implement.
        }
    }
```

### Metrics

When initalizing the metrics system you will have to pass it an IMetricsEmitter and a int that determins the number of metrics a batch will contain. 

```
	---

    var metrics = new Metrics(new ExampleEmitter, 1);

    metrics.Entry("Entry", "Data");

    ---
```

### Metric

A metric is the struct that is used by all metrics.

- Name: A string used for the name given to the metric.
- TimeStamp: DateTime set to the time that the metric was emitted.
- Type: A string used for indicating the type of metric.
- Data: An object that is the data to be sent with the metric. 

## Usage

The metrics system has three different metrics that it can emit:

### Entry

The Entry function can send data as a string, int or a float.
```
	metrics.Entry("Entry", "Data");
    metrics.Entry("Entry", 1);
    metrics.Entry("Entry", 1.0f);
```

The metric type for an entry can be a 'string', 'int' or 'float'.

### Increment

The Increment function is used to emit a metric of type inc.

```
	metrics.Inc("Name");
```

Increment uses the type of 'inc'

### Event

Designed to be a used for recording when expected events occurs.

```
	metrics.Event("Name");
```

Event uses the type 'event'.

### Flush

Emitts any metrics that are currently in the queue.

```
    metrics.Flush();
```

# Examples
Within this repository there are several simple examples of using the metrics system with different emitters.

## Example 1

A basic example of the metric system with a simple implementation of the IMetricEmitter interface that writes metrics to a file.

## Example 2

This example implements a IMetricsEmitter that emits the metrics via http post.

## Example 3

Example 3 is an example of using the built in batching system in the metrics system.

## Unity Example

An example of running the metrics system under Unity3D using the Unity3D method of http post.
