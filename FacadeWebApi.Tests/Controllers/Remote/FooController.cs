using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using FacadeWebApi.Tests.Models;
using System.Threading.Tasks;

namespace FacadeWebApi.Tests.Controllers.Remote
{
    public class FooController : ApiController
    {
        public IEnumerable<Foo> GetAll()
        {
            return new[] { new Foo() { Id = "1" }, new Foo() { Id = "2" } };
        }

        public Foo Get(string id)
        {
            return new Foo() { Id = id };
        }
    }
}
