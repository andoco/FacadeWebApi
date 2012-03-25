using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using FacadeWebApi.Tests.Models;
using NUnit.Framework;
using FacadeWebApi.Converter;

namespace FacadeWebApi.Tests
{
    [TestFixture]
    public class FacadeWebApiTests
    {
        private HttpServer localServer;
        private HttpSelfHostServer forwardServer;

        [SetUp]
        public void Init()
        {
            // setup server to host the facade
            var localConfig = new HttpConfiguration();
            localConfig.MessageHandlers.Add(
                new FacadeHttpMessageHandler(
                    new UrlConverter("/facade-api/(.+)", "http://localhost:8080/api/$1")));
            
            // add routes to be handled by facade and forwarded to another server
            localConfig.Routes.MapHttpRoute("foo", "api/foo", new { controller="Facade", converter=new UrlConverter("/api/(.+)", "http://localhost:8080/api/$1")});
            localConfig.Routes.MapHttpRoute("bar", "api/bar", new { controller = "Facade", converter = new UrlConverter("/api/(.+)", "http://localhost:8080/api/$1") });

            // add a default route to test it works in conjunction with facade handler
            localConfig.Routes.MapHttpRoute("default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            this.localServer = new HttpServer(localConfig);

            // setup another server we forward requests to
            var realServerConfig = new HttpSelfHostConfiguration("http://localhost:8080");
            this.forwardServer = new HttpSelfHostServer(realServerConfig);
            realServerConfig.Routes.MapHttpRoute("default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            this.forwardServer.OpenAsync().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            this.forwardServer.CloseAsync().Wait();
        }

        [Test(Description="Request should be handled by the FacadeController")]
        public void FacadeControllerShouldProcessRequests()
        {
            var client = new HttpClient(this.localServer);
            
            var fooResp = client.GetAsync("http://localhost/api/foo").Result;
            Assert.AreEqual(HttpStatusCode.OK, fooResp.StatusCode);
            Assert.DoesNotThrow(() => { var foos = fooResp.Content.ReadAsAsync<IEnumerable<Foo>>().Result; });
            
            var barResp = client.GetAsync("http://localhost/api/bar").Result;
            Assert.AreEqual(HttpStatusCode.OK, barResp.StatusCode);
            Assert.DoesNotThrow(() => { var bars = barResp.Content.ReadAsAsync<IEnumerable<Bar>>().Result; });
        }

        [Test(Description="Request should be handled by a normal controller")]
        public void DefaultControllerShouldProcessRequest()
        {
            var client = new HttpClient(this.localServer);
            var resp = client.GetAsync("http://localhost/api/local").Result;
            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
            Assert.DoesNotThrow(() => { var foos = resp.Content.ReadAsAsync<string[]>().Result; });
        }

        [Test(Description="Request should handled by the FacadeHttpMessageHandler")]
        public void FacadeMessageHandlerShouldProcessRequest()
        {
            var client = new HttpClient(this.localServer);

            var resp = client.GetAsync("http://localhost/facade-api/foo").Result;
            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
            Assert.DoesNotThrow(() => { var foos = resp.Content.ReadAsAsync<IEnumerable<Foo>>().Result; });
        }
    }
}
