using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Photograph.WebApi.Models
{
	public class SubscriberViewModel
	{
		public string Id { get; set; }
		public string TokenId { get; set; }

		[Required]
		public string UserName { get; set; }

		public string Password { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		public int DependsOnAdminId { get; set; }

		public double ValidUntil { get; set; }

		[Required]
		public List<string> Roles { get; set; }

		public bool IsSuspended { get; set; }

		public bool IsTrial { get; set; }

		public string SubscriptionPlan { get; set; }
	}
}