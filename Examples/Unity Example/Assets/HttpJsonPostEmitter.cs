using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RSG;
using Newtonsoft.Json;

public class HttpJsonPostEmitter : IMetricsEmitter
{
	private string postUrl;
	private IHttpService httpService;
	
	public HttpJsonPostEmitter(string postUrl, IHttpService httpService)
	{
		this.postUrl = postUrl;
		this.httpService = httpService;
	}

	public void Emit(IDictionary<string, string> properties, Metric[] metrics)
	{
		// Wrap up properties and metrics that we want to send.
		var objectToSend = new
		{
			Properties = properties,
			Metrics = metrics
		};
		
		// Convert to JSON.
		var json = JsonConvert.SerializeObject(objectToSend);
		
		// Send!
		httpService.Post(postUrl, json);
	}
}
