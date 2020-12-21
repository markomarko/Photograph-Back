using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Photograph.DAL.Constants;

namespace Photograph.DAL.Entities
{
	public class Client
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		public string ClientName { get; set; }

		public bool Enabled { get; set; }

		public string ClientId { get; set; }

		public string ClientSecrets { get; set; }

		public Flows Flow { get; set; }

		public int AccessTokenLifetime { get; set; }

		public virtual List<Scope> AllowedScopes { get; set; }
	}
}