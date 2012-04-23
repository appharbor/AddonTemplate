using AddonTemplate.Web.Controllers;
using RestfulRouting;

namespace AddonTemplate.Web
{
	public class Routes : RouteSet
	{
		public Routes()
		{
			Resource<HerokuController>(() =>
			{
				Resources<ResourceController>(() =>
				{
					As("resources");
				});
			});
		}
	}
}
