using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using FacadeWebApi.Tests.Models;

namespace FacadeWebApi.Tests.Controllers.Remote
{
    public class BarController : ApiController
    {
        public IEnumerable<Bar> GetAll()
        {
            return new[] { new Bar() { Id = "1" }, new Bar() { Id = "2" } };
        }

        public Bar Get(string id)
        {
            return new Bar() { Id = id };
        }
    }
}
