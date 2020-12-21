using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.DAL.Repository
{
	public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
	{
		protected DbSet<TEntity> _entities;
		private DbContext _context;

		protected GenericRepository(DbContext context)
		{
			_context = context;
			_entities = context.Set<TEntity>();
		}


		public virtual TEntity Get(TKey id)
		{
			return _entities.Find(id);
		}

		public virtual IEnumerable<TEntity> GetAll()
		{
			return _entities.ToList();
		}

		public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
		{
			return _entities.Where(predicate);
		}

		public virtual IEnumerable<TEntity> GetRange(int start, int count, Expression<Func<TEntity, TKey>> orderByPredicate)
		{
			return _entities.OrderBy(orderByPredicate).Skip(start).Take(count).ToList();
		}

		public virtual IEnumerable<TEntity> GetRange(int start, int count, Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TKey>> orderByPredicate)
		{
			return _entities.Where(predicate).OrderBy(orderByPredicate).Skip(start).Take(count).ToList();
		}

		public virtual int Count()
		{
			return _entities.Count();
		}

		public virtual void Add(TEntity entity)
		{
            _entities.Add(entity);
            _context.SaveChanges();
        }

		public virtual void AddRange(IEnumerable<TEntity> entities)
		{
			_entities.AddRange(entities);
		}

		public virtual void Remove(TEntity entity)
		{
			_entities.Remove(entity);
			_context.SaveChanges();
		}

		public virtual void Update(TEntity entity)
		{
            _context.Entry(entity).State = EntityState.Added;
            _entities.Attach(entity);
			_context.SaveChanges();
		}
	}
}