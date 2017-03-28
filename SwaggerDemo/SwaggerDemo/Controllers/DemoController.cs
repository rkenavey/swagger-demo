using System.Net;
using System.Web.Http;

namespace SwaggerDemo.Controllers
{
    public abstract class DemoController : ApiController
    {
        protected IHttpActionResult NotFound(string message)
        {
            var formatter = ControllerContext.Configuration.Formatters.JsonFormatter;
            var notFoundMessage = $"{(int)HttpStatusCode.NotFound} ({HttpStatusCode.NotFound}): {message}";
            return Content(HttpStatusCode.NotFound, notFoundMessage, formatter);
        }
    }
}
