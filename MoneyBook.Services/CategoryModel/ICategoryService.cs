using System;
using System.Linq;
using System.Linq.Expressions;
using MoneyBook.Repositories;

namespace MoneyBook.Services.CategoryModel {
    public interface ICategoryService {
        void Create(string userId, CategoryDto instance);

        void Update(string userId, Guid id, CategoryDto instance);

        void Enable(string userId, Guid id);

        void Disable(string userId, Guid id);

        void MoveUp(string userId, Guid id);

        void MoveDown(string userId, Guid id);

        void Delete(string userId, Guid id);

        IQueryable<Category> Read(Expression<Func<Category, bool>> predicate = null);

        Category Get(string userId, Guid id);

        bool VerifyPermission(string userId, Guid id);
    }
}