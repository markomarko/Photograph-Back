using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.DAL.Repository
{
	public interface IGenericRepository<TEntity, TKey> where TEntity : class
	{
		TEntity Get(TKey id);

		IEnumerable<TEntity> GetAll();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

		IEnumerable<TEntity> GetRange(int start, int count, Expression<Func<TEntity, TKey>> orderByPredicate);
		IEnumerable<TEntity> GetRange(int start, int count, Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderByPredicate);

		int Count();
        void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);
		void Remove(TEntity entity);
		void Update(TEntity entity);
	}
}