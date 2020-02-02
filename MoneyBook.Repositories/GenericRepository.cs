using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace MoneyBook.Repositories {

    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, new() {

        private bool isDisposed;

        public GenericRepository(DbContext context) {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.Database.Log = (log) => Debug.WriteLine(log);
        }

        public DbContext Context {
            get;
            private set;
        }

        public DbSet<TEntity> DatabaseSet => Context.Set<TEntity>();

        public void Create(TEntity instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(TEntity));
            }
            DatabaseSet.Add(instance);
        }

        public IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> predicate = null) {
            IQueryable<TEntity> query = DatabaseSet.AsExpandable();
            if (predicate != null) {
                query = query.Where(predicate);
            }
            return query;
        }

        public void Update(TEntity instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(TEntity));
            }
            Context.Entry(instance).State = EntityState.Modified;
        }

        public void Delete(TEntity instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(TEntity));
            }
            Context.Entry(instance).State = EntityState.Deleted;
        }

        public void SaveChanges() {
            Context.SaveChanges();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing) {

            if (isDisposed) {
                return;
            }

            if (isDisposing && Context != null) {
                Context.Dispose();
                Context = null;
            }

            isDisposed = true;
        }

        ~GenericRepository() {
            Dispose(false);
        }
    }
}
