using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacadeWebApi.Converter
{
    /// <summary>
    /// Converts a request URL into another URL which is used for forwarding the request
    /// </summary>
    public interface IFacadeMessageConverter
    {
        /// <summary>
        /// Checks if the request can be handled by this converter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool Match(HttpRequestMessage request);

        /// <summary>
        /// Converts the source request into new requests to be processed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        HttpRequestMessage[] Convert(HttpRequestMessage request);

        /// <summary>
        /// Processes the received responses and returns a single new response
        /// </summary>
        /// <param name="responseTasks"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> Complete(IEnumerable<Task<HttpResponseMessage>> responseTasks);
    }
}
