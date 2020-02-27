using System.Threading.Tasks;
using System.Web.Http;
using Owin;
using Serilog;

namespace LegacyOwin
{
    public class Startup
    {
        static Startup()
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.RollingFile(@"C:\Logs\legacyOwin.log")
                            .CreateLogger();
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { controller = nameof(HomeController).Replace("Controller",""), id = RouteParameter.Optional }
            //);
            config.MapHttpAttributeRoutes();
            app.Use((ctx, next) =>
            {
                Log.Logger.Information($"{ctx.Request.Uri} - before webapi");
                return next();
            });
            app.UseWebApi(config);
            app.Run((ctx) =>
            {
                Log.Logger.Information($"{ctx.Request.Uri} - after webapi");
                ctx.Response.StatusCode = 404;
                ctx.Response.ReasonPhrase = "Dead End";
                ctx.Response.Body.Close();
                return Task.CompletedTask;
            });
        }
    }
}