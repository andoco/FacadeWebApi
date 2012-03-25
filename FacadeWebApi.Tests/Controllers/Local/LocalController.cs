using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace FacadeWebApi.Tests.Controllers.Local
{
    /// <summary>
    /// Normal controller that handles requests in the default fashion
    /// </summary>
    public class LocalController : ApiController
    {
        public virtual IEnumerable<string> Get()
        {
            return new[] { "one", "two", "three" };
        }
    }
}
