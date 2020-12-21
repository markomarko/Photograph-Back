using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photograph.DAL.Entities
{
	public class Scope
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public bool IncludeAllClaimsForUser { get; set; }

		public virtual List<Client> Clients { get; set; }
	}
}