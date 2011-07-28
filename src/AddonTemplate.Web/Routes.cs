using AddonTemplate.Web.Controllers;
using RestfulRouting;

namespace AddonTemplate.Web
{
	public class Routes : RouteSet
	{
		public Routes()
		{
			Resource<ProviderController>(() =>
			{
				Resources<ResourceController>(() =>
				{
					As("resources");
				});
			});
		}
	}
}
