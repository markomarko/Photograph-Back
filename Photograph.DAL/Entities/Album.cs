using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photograph.DAL.Entities
{
	public class Album
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime DateTime { get; set; }

		public Guid OwnerId { get; set; }	

		public Subscriber Owner { get; set; }

		[InverseProperty("AccessibleAlbums")]
		public virtual List<User> UsersWithAccess { get; set; }

		public virtual List<Photo> Photos { get; set; }	
	}
}