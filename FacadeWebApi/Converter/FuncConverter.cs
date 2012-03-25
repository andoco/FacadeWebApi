using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacadeWebApi.Converter
{
    public class FuncConverter : IFacadeMessageConverter
    {
        private readonly Func<HttpRequestMessage, bool> matchFunc;
        private readonly Func<HttpRequestMessage, HttpRequestMessage[]> convertFunc;
        private readonly Func<IEnumerable<Task<HttpResponseMessage>>, Task<HttpResponseMessage>> completeFunc;

        public FuncConverter(
            Func<HttpRequestMessage, bool> matchFunc,
            Func<HttpRequestMessage, HttpRequestMessage[]> convertFunc,
            Func<IEnumerable<Task<HttpResponseMessage>>, Task<HttpResponseMessage>> completeFunc)
        {
            this.matchFunc = matchFunc;
            this.convertFunc = convertFunc;
            this.completeFunc = completeFunc;
        }

        public bool Match(HttpRequestMessage request)
        {
            return this.matchFunc(request);
        }

        public HttpRequestMessage[] Convert(HttpRequestMessage request)
        {
            return this.convertFunc(request);
        }

        public Task<HttpResponseMessage> Complete(IEnumerable<Task<HttpResponseMessage>> responseTasks)
        {
            return this.completeFunc(responseTasks);
        }
    }
}
