using System.Web;

namespace AddonTemplate.Web
{
	internal static class HttpRequestExtensions
	{
		public static string GetForwardedHostAddress(this HttpRequestBase httpRequest)
		{
			const string forwardedForHeader = "HTTP_X_FORWARDED_FOR";

			var forwardedFor = httpRequest.ServerVariables[forwardedForHeader];
			if (forwardedFor != null)
			{
				return forwardedFor;
			}

			return httpRequest.UserHostAddress;
		}
	}
}
