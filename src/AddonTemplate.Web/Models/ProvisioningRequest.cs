namespace AddonTemplate.Web.Models
{
	public class ProvisioningRequest
	{
		public string heroku_id
		{
			get;
			set;
		}

		public string plan
		{
			get;
			set;
		}

		public string callback_url
		{
			get;
			set;
		}
	}
}
