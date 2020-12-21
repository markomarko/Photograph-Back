using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;

namespace Photograph.DAL.Repository.Adapters
{
	public class UserRepository : GenericRepository<User, Guid>, IUserRepository
	{
		private readonly DbSet<Role> _roleEntities;
		private readonly DbSet<Subscriber> _subscriberEntities;

		public UserRepository(DbContext context) : base(context)
		{
			_roleEntities = context.Set<Role>();
			_subscriberEntities = context.Set<Subscriber>();
		}

		public User GetUserById(Guid id)
		{
			return _entities
				.Include(u => u.Roles)
				.Include(u => u.Subscriber)
				.Include(u => u.DependsOnAdmin)
				.SingleOrDefault(x => x.Id.Equals(id));
		}

		public Task<User> GetByIdAsync(Guid id)
		{
			return _entities
				.Include(u => u.Roles)
				.Include(u => u.Subscriber)
				.Include(u => u.DependsOnAdmin).FirstOrDefaultAsync(x => x.Id.Equals(id));
		}

		public IEnumerable<User> GetAllUsers()
		{
			return _entities
				.Include(u => u.Roles)
				.Include(u => u.Subscriber)
				.Include(u => u.DependsOnAdmin).ToList();
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return await _entities.Include(u => u.Roles).ToListAsync();
		}

		public override IEnumerable<User> GetRange(int start, int count, Expression<Func<User, Guid>> orderByPredicate)
		{
			return _entities.Include(x => x.Roles).OrderBy(orderByPredicate).Skip(start).Take(count).ToList();
		}

		public void AddUser(User user)
		{
			user.Password = user.Password.Sha256();

			var role = _roleEntities.Find(user.Roles[0].Id);

			user.Roles = new List<Role>() {role};
            user.SelectedPhotos = new List<Photo>();
			if (user.DependsOnAdmin != null)
			{
				var sub = _subscriberEntities.Find(user.DependsOnAdmin.First().UserId);
				user.DependsOnAdmin = new List<Subscriber>() {sub};
			}

			Add(user);
		}

		public void UpdateUser(User user)
		{
			using (var context = new PhotographContext())
			{
				var dbUser = _entities.Include("Roles").FirstOrDefault(x => x.Id.Equals(user.Id));

				if (dbUser != null)
				{
					if (user.Password != null && user.Password.Length > 0)
					{
						dbUser.Password = user.Password.Sha256();
					}
					dbUser.UserName = user.UserName;
					dbUser.ValidUntil = user.ValidUntil;
					dbUser.FirstName = user.FirstName;
					dbUser.LastName = user.LastName;

					if (dbUser.Roles.FirstOrDefault(x => x.Id.Equals(user.Roles[0].Id)) == null)
					{
						dbUser.Roles.RemoveAll(x => true);
						context.SaveChanges();

						if (context.Entry(user.Roles[0]).State == EntityState.Detached)
							context.Roles.Attach(user.Roles[0]);

						dbUser.Roles.Add(user.Roles[0]);
					}
				}
				context.SaveChanges();
			}
		}

		public void DeleteUser(Guid id)
		{
			var user = GetUserById(id);

			Remove(user);
		}
	}
}