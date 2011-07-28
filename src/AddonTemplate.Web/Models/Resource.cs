using System;

namespace AddonTemplate.Web.Models
{
	public class Resource
	{
		public virtual string CreatedBy
		{
			get;
			set;
		}

		public virtual Guid Id
		{
			get;
			set;
		}

		public virtual Plan Plan
		{
			get;
			set;
		}

		public virtual string ProviderId
		{
			get;
			set;
		}

		public virtual ProvisionStatus ProvisionStatus
		{
			get;
			set;
		}
	}
}
