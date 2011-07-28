using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RestfulRouting;

namespace AddonTemplate.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);
		}

		private static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoutes<Routes>();
		}
	}
}
