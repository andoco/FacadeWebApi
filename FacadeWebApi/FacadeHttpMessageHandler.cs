using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FacadeWebApi.Converter;

namespace FacadeWebApi
{
    /// <summary>
    /// Message handler for converting a <see cref="HttpRequestMessage"/> into one or more new requests
    /// and processing the responses back into a single <see cref="HttpResponseMessage"/>
    /// </summary>
    /// <remarks>
    /// The first <see cref="IFacadeMessageConverter"/> that matches the request will be used for
    /// handling the request, and the message will proceed no further along the message pipeline.
    /// 
    /// If no matching <see cref="IFacadeMessageConverter"/> is found, the request will continue
    /// along the message pipeline.
    /// </remarks>
    public class FacadeHttpMessageHandler : DelegatingHandler
    {
        private readonly IList<IFacadeMessageConverter> converters;

        public FacadeHttpMessageHandler(params IFacadeMessageConverter[] converters)
        {
            this.converters = new List<IFacadeMessageConverter>(converters);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // find matching converter
            var converter = this.converters.FirstOrDefault(c => c.Match(request));

            if (converter == null)
            {
                // continue normal message processing
                return base.SendAsync(request, cancellationToken);
            }

            var processor = new FacadeRequestProcessor();

            return processor.Process(request, converter);
        }
    }
}
