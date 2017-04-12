using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Example2
{
    /// <summary>
    /// Simple implementation of IHttpService supporting posting data to a URL.
    /// </summary>
    class SimpleHttpService : IHttpService
    {

        public void Post(string url, string text)
        {
            // Convert to byte array
            var data = Encoding.Default.GetBytes(text);

            // Create request
            var request = WebRequest.Create(url);

            request.Method = "POST";
            request.ContentLength = data.Length;
            request.ContentType = "application/json";

            var dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();

            // Send the request
            var response = (HttpWebResponse)request.GetResponse();
            response.Close();

            // Notify if we were unsucessful
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException("POST to " + url + " returned response code " + 
                    (int)response.StatusCode + " " + response.StatusCode.ToString());
            }
        }
    }
}
