using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.DAL.Entities;

namespace Photograph.DAL.Repository.Adapters
{
	public class NotificationRepository : GenericRepository<Notification, Guid>
	{
		private readonly DbSet<User> _userEntities;
		public NotificationRepository(DbContext context) : base(context)
		{
			_userEntities = context.Set<User>();
		}

		public override void Add(Notification entity)
		{
			entity.User = _userEntities.Find(entity.UserId);

			base.Add(entity);
		}
	}
}