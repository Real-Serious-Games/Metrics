using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example3
{
    /// <summary>
    /// Interface for sending data to an HTTP service.
    /// </summary>
    interface IHttpService
    {
        /// <summary>
        /// Post the specified data to a URL.
        /// </summary>
        void Post(string url, string data);
    }
}
