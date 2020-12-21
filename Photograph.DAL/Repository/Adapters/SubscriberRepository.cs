using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.DAL.Entities;

namespace Photograph.DAL.Repository.Adapters
{
	public class SubscriberRepository : GenericRepository<Subscriber, Guid>
	{
		private readonly DbSet<User> _userEntities;

		public SubscriberRepository(DbContext context) : base(context)
		{
			_userEntities = context.Set<User>();
		}

		public override Subscriber Get(Guid id)
		{
			return _entities
				.Include(x => x.DependentUsers)
				.Include(x => x.User)
				.FirstOrDefault(x => x.UserId.Equals(id));
		}

		public override void Add(Subscriber entity)
		{
			var user = _userEntities.FirstOrDefault(usr => usr.Id.Equals(entity.UserId));
			entity.User = user;
			base.Add(entity);
		}
	}
}