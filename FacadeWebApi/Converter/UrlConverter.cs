using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacadeWebApi.Converter
{
    /// <summary>
    /// Converts messages by mapping from one Uri to another
    /// </summary>
    public class UrlConverter : IFacadeMessageConverter
    {
        private readonly string pattern;
        private readonly string replace;

        public UrlConverter(string pattern, string replace)
        {
            this.pattern = pattern;
            this.replace = replace;
        }

        public bool Match(HttpRequestMessage request)
        {
            return Regex.IsMatch(request.RequestUri.AbsolutePath, this.pattern);
        }

        public HttpRequestMessage[] Convert(HttpRequestMessage request)
        {
            var newUrl = new Uri(Regex.Replace(request.RequestUri.AbsolutePath, this.pattern, this.replace));

            var newRequest = new HttpRequestMessage(request.Method, newUrl)
            {
                Content = request.Content,
            };

            return new [] { newRequest };
        }

        public Task<HttpResponseMessage> Complete(IEnumerable<Task<HttpResponseMessage>> responseTasks)
        {
            // check the expected response tasks to process
            if (responseTasks.Count() != 1)
                throw new InvalidOperationException("Exactly 1 response message should be expected");

            // get the task that is receiving the response
            var task = responseTasks.Single();

            // return a continuation that returns the received response
            return task.ContinueWith(t => { return t.Result; });            
        }
    }
}
