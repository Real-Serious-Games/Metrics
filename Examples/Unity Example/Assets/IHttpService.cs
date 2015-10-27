using System;

/// <summary>
/// Interface for sending data to an HTTP service.
/// </summary>
public interface IHttpService
{
	/// <summary>
	/// Post the specified data to a URL.
	/// </summary>
	void Post(string url, string data);
}



