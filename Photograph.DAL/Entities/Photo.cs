using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Photograph.DAL.Entities
{
	public class Photo
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		public Guid AlbumId { get; set; }
		public virtual Album Album { get; set; }
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public string Context { get; set; }
		public bool Selected { get; set; }
		public virtual List<User> SelectedByUsers { get; set; }	
	}
}