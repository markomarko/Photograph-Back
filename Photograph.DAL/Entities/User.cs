using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photograph.DAL.Entities
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		[StringLength(255)]
		[Index(IsUnique = true)]
		public string UserName { get; set; }
		public string Password { get; set; }

		[EmailAddress]
		[StringLength(255)]
		[Index(IsUnique = true)]
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		[InverseProperty("DependentUsers")]
		public virtual List<Subscriber> DependsOnAdmin { get; set; }

		public double ValidUntil { get; set; }
		public bool IsSuspended { get; set; }
		public virtual List<Role> Roles { get; set; }
		public virtual Subscriber Subscriber { get; set; }
		public virtual List<Album> AccessibleAlbums { get; set; }
		public virtual List<Photo> SelectedPhotos { get; set; }

		public virtual List<Notification> Notifications { get; set; }
	}
}