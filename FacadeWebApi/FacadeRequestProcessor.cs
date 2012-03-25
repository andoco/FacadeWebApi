using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using FacadeWebApi.Converter;

namespace FacadeWebApi
{
    /// <summary>
    /// Processes the conversion and completion of a request
    /// </summary>
    public class FacadeRequestProcessor
    {
        public Task<HttpResponseMessage> Process(HttpRequestMessage request, IFacadeMessageConverter converter)
        {
            // convert the request into fulfilling requests
            var fulfillmentRequests = converter.Convert(request);

            var client = new HttpClient();

            // asynchronously fulfill the requests
            var fulfillmentTasks = fulfillmentRequests.Select(r => client.SendAsync(r)).ToArray();

            // return task for fulfilled responses
            return converter.Complete(fulfillmentTasks);
        }
    }
}
