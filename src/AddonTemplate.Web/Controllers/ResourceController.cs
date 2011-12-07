using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using AddonTemplate.Web.Models;
using AddonTemplate.Web.ViewModels;
using RestSharp;
using HttpCookie = System.Web.HttpCookie;

namespace AddonTemplate.Web.Controllers
{
	public class ResourceController : Controller
	{
		[RequireBasicAuthentication("AppHarbor")]
		public ActionResult Create(ProvisioningRequest provisionRequest)
		{
			Plan plan;
			if (!Enum.TryParse<Plan>(provisionRequest.plan, true, out plan))
			{
				throw new ArgumentException(string.Format("Plan \"{0}\" is not a valid plan", provisionRequest.plan));
			}

			var resource = new Resource
			{
				CreatedBy = string.Format("{0};{1}", Request.GetForwardedHostAddress(), User.Identity.Name),
				Id = Guid.NewGuid(),
				Plan = plan,
				ProviderId = provisionRequest.heroku_id,
				ProvisionStatus = ProvisionStatus.Provisioning,
			};
			
			// TODO: Persist the resource

			// TODO: Provision the resource

			resource.ProvisionStatus = ProvisionStatus.Provisioned;
			
			// TODO: Persist the status change

			var output = new
			{
				id = resource.Id,
				config = new
				{
					CONFIG_VAR = "CONFIGURATION_VALUE",
				}
			};

			return Json(output);
		}

		[RequireBasicAuthentication("AppHarbor")]
		public ActionResult Destroy(Guid id)
		{
			// TODO: Fetch the resource from persistance store
			var resource = new Resource();

			resource.ProvisionStatus = ProvisionStatus.Deprovisioning;

			// TODO: Persist the status change

			// TODO: De-provision the resource

			resource.ProvisionStatus = ProvisionStatus.Deprovisioned;

			// TODO: Persist the status change

			return Json("ok");
		}

		public ActionResult Show(Guid id, string token, string timeStamp)
		{
			AuthenticateToken(id, token, timeStamp);

			SetAddonCookie();

			var headerClient = new RestClient("http://appharbor.com/");
			var headerRequest = new RestRequest("header", Method.GET);
			var headerResponse = headerClient.Execute(headerRequest);

			// TODO: Fetch the resource from persistance store
			var resource = new Resource();

			// TODO: Populate the view model with the resource data
			var viewModel = new ResourceViewModel
			{
				Header = headerResponse.Content,
			};

			return View(viewModel);
		}

		[RequireBasicAuthentication("Heroku")]
		public ActionResult Update(Guid id, PlatformRequest planUpdateRequest)
		{
			Plan plan;
			if (!Enum.TryParse<Plan>(planUpdateRequest.plan, true, out plan))
			{
				throw new ArgumentException(string.Format("Plan \"{0}\" is not a valid plan", planUpdateRequest.plan));
			}

			// TODO: Fetch the resource from persistance store
			var resource = new Resource();

			resource.Plan = plan;

			// TODO: Update resource to reflect new plan

			// TODO: Persist the resource change

			var output = new
			{
				id = resource.Id,
				config = new
				{
					CONFIG_VAR = "CONFIGURATION_VALUE",
				}
			};

			return Json(output);
		}

		private void SetAddonCookie()
		{
			var navData = Request.QueryString["nav-data"];
			var cookie = new HttpCookie("appharbor-nav-data", navData);

			Response.SetCookie(cookie);
		}

		private void AuthenticateToken(Guid id, string token, string timeStamp)
		{
			var validToken = string.Join(":", id.ToString(), "MANIFEST_SSO_SALT", timeStamp);
			var hash = validToken.ToHash<SHA1Managed>();
			if (token != hash)
			{
				throw new HttpException(403, "Authentication failed");
			}

			var validTime = (DateTime.UtcNow.AddMinutes(-5) - new DateTime(1970, 1, 1)).TotalSeconds;
			if (Convert.ToInt32(timeStamp) < validTime)
			{
				throw new HttpException(403, "Timestamp too old");
			}
		}
	}
}
