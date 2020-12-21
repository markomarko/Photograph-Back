using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;

namespace Photograph.DAL.Repository.Adapters
{
	public class PhotoRepository : GenericRepository<Photo, Guid>, IPhotoRepository
	{

        private readonly DbSet<Album> _albumContext;
        private readonly DbSet<User> _userContext;

		public PhotoRepository(DbContext context) : base(context)
		{
            _albumContext = context.Set<Album>();
            _userContext = context.Set<User>();
		}

        public override void Add(Photo entities)
        {
            entities.Id = Guid.NewGuid();

            entities.Album = _albumContext.SingleOrDefault(x => x.Id.Equals(entities.AlbumId));

            entities.SelectedByUsers = new List<User>();

            base.Add(entities);
        }

        public override Photo Get(Guid id)
        {
            return _entities
                .Include(p => p.SelectedByUsers)
                .SingleOrDefault(p => p.Id.Equals(id));
                
        }

        public int Count(Guid id)
        {
            using (var db = new PhotographContext())
            {
                return db.Photos.Count(x => x.AlbumId == id);
            }
        }

        public override void Update(Photo entity)
        {
            using (var context = new PhotographContext())
            {
                var dbPhoto = context.Photos.Include(x => x.SelectedByUsers).SingleOrDefault(x => x.Id.Equals(entity.Id));

                foreach (var user in entity.SelectedByUsers)
                {
                    dbPhoto.SelectedByUsers.Add(context.Users.SingleOrDefault(u => u.Id.Equals(user.Id)));
                }
                
                context.SaveChanges();
            }
        }

        public override IEnumerable<Photo> GetRange(int start, int count, Expression<Func<Photo, bool>> predicate, Expression<Func<Photo, Guid>> orderByPredicate)
        {
            return _entities.Include(p => p.SelectedByUsers).Where(predicate).OrderBy(orderByPredicate).Skip(start).Take(count).ToList();
        }
    }
}