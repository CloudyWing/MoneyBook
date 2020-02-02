using System;
using System.Linq;
using System.Linq.Expressions;

namespace MoneyBook.Repositories {

    public interface IRepository<TEntity> : IDisposable where TEntity : class, new() {

        void Create(TEntity instance);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> predicate = null);

        void SaveChanges();
    }
}