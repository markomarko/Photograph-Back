using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Photograph.WebApi.Models
{
	public class UserViewModel
	{
		public Guid Id { get; set; }

		[Required]
		public string UserName { get; set; }

		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		public string SubscriberEmail { get; set; }
		public double ValidUntil { get; set; }
		public bool IsSuspended { get; set; }

		[Required]
		public List<string> Roles { get; set; }
	}
}