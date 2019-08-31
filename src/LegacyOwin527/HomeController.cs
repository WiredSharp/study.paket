using System.Web.Http;

namespace LegacyOwin
{
    //[RoutePrefix("Home")]
    public class HomeController: ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(new {Message = "Hello World"});
        }
    }
}