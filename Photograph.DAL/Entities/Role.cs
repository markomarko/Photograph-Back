using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photograph.DAL.Entities
{
	public class Role
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public virtual List<User> Users { get; set; }
	}
}