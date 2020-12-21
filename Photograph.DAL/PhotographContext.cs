using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.DAL.Entities;

namespace Photograph.DAL
{
	public class PhotographContext : DbContext
	{
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Album> Albums { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Client> Clients { get; set; }
		public DbSet<Scope> Scopes { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Subscriber> Subscribers { get; set; }

		public PhotographContext()
		{
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

			modelBuilder.Entity<User>()
				.HasOptional(u => u.Subscriber)
				.WithRequired(s => s.User)
				.WillCascadeOnDelete(true);

			base.OnModelCreating(modelBuilder);
		}
	}
}