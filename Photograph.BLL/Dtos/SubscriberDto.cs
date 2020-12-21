using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.DAL.Entities;

namespace Photograph.BLL.Dtos
{
	public class SubscriberDto
	{
		public int Id { get; set; }
		public UserDto UserDto { get; set; }
		public string StripeId { get; set; }
		public bool IsSuspended { get; set; }
		public bool IsTrial { get; set; }
		public SubscriptionPlan SubscriptionPlan { get; set; }
	}
}