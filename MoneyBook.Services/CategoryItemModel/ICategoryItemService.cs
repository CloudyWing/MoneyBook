using System;
using System.Linq;
using System.Linq.Expressions;
using MoneyBook.Repositories;

namespace MoneyBook.Services.CategoryItemModel {
    public interface ICategoryItemService {
        void Create(string userId, CategoryItemDto instance);

        void Update(string userId, Guid id, CategoryItemDto instance);

        void Enable(string userId, Guid id);

        void Disable(string userId, Guid id);

        void MoveUp(string userId, Guid id);

        void MoveDown(string userId, Guid id);

        void Delete(string userId, Guid id);

        IQueryable<CategoryItem> Read(Expression<Func<CategoryItem, bool>> predicate = null);

        CategoryItem Get(string userId, Guid id);

        bool VerifyPermission(string userId, Guid id);
    }
}