using Photograph.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;
using System.Linq.Expressions;

namespace Photograph.DAL.Repository.Adapters
{
    public class AlbumRepository : GenericRepository<Album, Guid>, IAlbumRepository
    {

        private readonly DbSet<User> _userEntities;

        public AlbumRepository(DbContext context) : base(context)
        {
            _userEntities = context.Set<User>();
        }

        public void AddAlbum(Album entity, List<Guid> clientList)
        {
         
            entity.Id = Guid.NewGuid();

            entity.UsersWithAccess = new List<User>();

            entity.Photos = new List<Photo>();

            entity.Owner =  _userEntities.Include(x => x.Subscriber).SingleOrDefault(x => x.Id.Equals(entity.OwnerId)).Subscriber;

            entity.UsersWithAccess = new List<User>();

            foreach (Guid client in clientList)
            {
                var temp = _userEntities.SingleOrDefault(x => x.Id.Equals(client));

                if (temp != null)
                    entity.UsersWithAccess.Add(temp);
            }

            entity.Photos = new List<Photo>();
            Add(entity);
           
        }

        public override IEnumerable<Album> Find(Expression<Func<Album, bool>> predicate)
        {
            return _entities.Include(a => a.UsersWithAccess).Where(predicate);
        }
    }
}
