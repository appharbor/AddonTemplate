using System;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AddonTemplate.Web
{
	public class RequireBasicAuthenticationAttribute : AuthorizeAttribute
	{
		private readonly string _realm;

		public RequireBasicAuthenticationAttribute(string realm)
		{
			_realm = realm;
		}

		public string Realm
		{
			get
			{
				return _realm;
			}
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var httpAuthorizationHeader = httpContext.Request.Headers["Authorization"];
			if (string.IsNullOrEmpty(httpAuthorizationHeader))
			{
				return false;
			}

			string[] httpAuthorization = httpAuthorizationHeader.Split(' ');
			if (httpAuthorization.Length != 2)
			{
				return false;
			}

			if (httpAuthorization[0] != "Basic")
			{
				return false;
			}

			NetworkCredential credential = ParseCredential(httpAuthorization[1]);
			if (credential == null)
			{
				return false;
			}

			// TODO: Verify credentials
			if (credential.UserName != "foo" || credential.Password != "123456")
			{
				return false;
			}

			httpContext.User = new GenericPrincipal(new GenericIdentity("appharbor"), new string[0]);

			return true;
		}

		private static NetworkCredential ParseCredential(string value)
		{
			byte[] encodedBytes = Convert.FromBase64String(value);

			string unencoded = Encoding.GetEncoding("iso-8859-1").GetString(encodedBytes);
			if (unencoded.IndexOf(':') < 0)
			{
				return null;
			}

			string username = unencoded.Remove(unencoded.IndexOf(':'));
			string password = unencoded.Substring(unencoded.IndexOf(':') + 1);

			return new NetworkCredential(username, password);
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			var httpResponse = filterContext.HttpContext.Response;
			httpResponse.StatusCode = 401;
			httpResponse.AddHeader("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
			httpResponse.End();

			filterContext.Result = new EmptyResult();
		}
	}
}
