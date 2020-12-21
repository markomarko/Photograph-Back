using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photograph.DAL.Entities
{
	public class Subscriber
	{
		[Key, ForeignKey("User")]
		public Guid UserId { get; set; }

		[Required]
		[InverseProperty("Subscriber")]
		public virtual User User { get; set; }

		public bool IsSuspended { get; set; }
		public bool IsTrial { get; set; }
		public string StripeId { get; set; }
		public string SubscriptionPlan { get; set; }

		[InverseProperty("DependsOnAdmin")]
		public virtual List<User> DependentUsers { get; set; }

		[InverseProperty("Owner")]
		public virtual List<Album> OwnAlbums { get; set; }
	}
}