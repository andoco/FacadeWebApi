using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using FacadeWebApi.Converter;

namespace FacadeWebApi
{
    /// <summary>
    /// Controller for converting a <see cref="HttpRequestMessage"/> into one or more new requests
    /// and processing the responses back into a single <see cref="HttpResponseMessage"/>
    /// </summary>
    /// <remarks>
    /// The <see cref="IFacadeMessageConverter"/> to be used is supplied in the request RouteData using
    /// a property name of "converter". If the converter is not supplied an <see cref="InvalidOperationException"/>
    /// is thrown.
    /// </remarks>
    public class FacadeController : IHttpController
    {
        public Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            // get the converter to use from the RouteData
            var converter = controllerContext.RouteData.Values["converter"] as IFacadeMessageConverter;

            if (converter == null)
                throw new InvalidOperationException("Requests mapped to FacadeController must supply a converter in RouteData");

            var processor = new FacadeRequestProcessor();

            return processor.Process(controllerContext.Request, converter);
        }
    }
}
