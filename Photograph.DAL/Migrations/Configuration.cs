using System.Collections.Generic;
using System.Data.Entity;
using Photograph.DAL.Constants;
using Photograph.DAL.Entities;

namespace Photograph.DAL.Migrations
{
	using System;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Photograph.DAL.PhotographContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;

			ContextKey = "Photograph.DAL.PhotographContext";
		}

		protected override void Seed(PhotographContext context)
		{
            //BuildIds(context);
        }

		private static void BuildIds(PhotographContext context)
		{
			var photographScope = new Scope()
			{
				Id = Guid.NewGuid(),
				Name = "Photograph",
				IncludeAllClaimsForUser = true
			};
			context.Scopes.Add(photographScope);
			context.SaveChanges();

			var client = new Client()
			{
				Id = Guid.NewGuid(),
				ClientName = "Photograph",
				ClientId = "PhotographId",
				Flow = Flows.ResourceOwner,
				Enabled = true,
				ClientSecrets = "jm8B+rcdwALtH8VN8XYAvDQWBQbWGq78AYIYJgP81IQ=",
				AccessTokenLifetime = 86400,
				AllowedScopes = new List<Scope>() {photographScope}
			};
			context.Clients.Add(client);
			context.SaveChanges();

			var adminRole = new Role()
			{
				Id = Guid.NewGuid(),
				Name = "Admin"
			};
			var subscriberRole = new Role()
			{
				Id = Guid.NewGuid(),
				Name = "Subscriber"
			};
			var userRole = new Role()
			{
				Id = Guid.NewGuid(),
				Name = "User"
			};

			context.Roles.Add(adminRole);
			context.Roles.Add(subscriberRole);
			context.Roles.Add(userRole);
			context.SaveChanges();

			var adminRoleDb = context.Roles.FirstOrDefault(x => x.Name.Equals("Admin"));

			var adminUserTomislav = new User()
			{
				Id = Guid.NewGuid(),
				UserName = "Admin",
				Password = "UkL3UmTpZ9hyA81IYOLwTQSUDmaP0Qq+ZyZKuxD/xts=",
				Email = "admin@mailinator.com",
				FirstName = "Admin",
				LastName = "Admin",
				Roles = new List<Role>() {adminRole}
			};
			context.Users.Add(adminUserTomislav);
			context.SaveChanges();

			var subRoleDb = context.Roles.FirstOrDefault(x => x.Name.Equals("Subscriber"));

			var subUserTomislav = new User()
			{
				Id = Guid.NewGuid(),
				UserName = "Sub",
				Password = "UkL3UmTpZ9hyA81IYOLwTQSUDmaP0Qq+ZyZKuxD/xts=",
				Email = "sub@mailinator.com",
				FirstName = "Sub",
				LastName = "Sub",
				Roles = new List<Role>() {subRoleDb}
			};
			context.Users.Add(subUserTomislav);
			context.SaveChanges();

			var userRoleDb = context.Roles.FirstOrDefault(x => x.Name.Equals("User"));

			for (var i = 0; i < 100; i++)
			{
				var id = Guid.NewGuid();

				var userTomislav = new User()
				{
					Id = id,
					UserName = id.ToString(),
					Password = "UkL3UmTpZ9hyA81IYOLwTQSUDmaP0Qq+ZyZKuxD/xts=",
					Email = $"{id}@mailinator.com",
					FirstName = "Sub",
					LastName = "Sub",
					Roles = new List<Role>() {userRoleDb}
				};
				context.Users.Add(userTomislav);
			}
			context.SaveChanges();


			//var user = context.Users.FirstOrDefault(x => x.UserName.Equals("Admin"));

			//var subs = new Subscriber
			//{
			//	UserId = user.Id,
			//	User = user
			//};

			//context.Subscribers.Add(subs);
			//context.SaveChanges();

			//var user1 = context.Users
			//	.Include(x => x.Subscriber)
			//	.Include(x => x.Roles)
			//	.FirstOrDefault(x => x.UserName.Equals("Admin"));
			//var sub = context.Subscribers.Include(x => x.User).FirstOrDefault(x => x.UserId.Equals(user1.Id));

			//context.Subscribers.Remove(sub);

			//context.SaveChanges();
		}
	}
}