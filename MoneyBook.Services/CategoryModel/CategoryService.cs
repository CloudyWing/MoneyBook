using System;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using MoneyBook.Repositories;

namespace MoneyBook.Services.CategoryModel {

    public class CategoryService : ServiceBase, ICategoryService {
        private readonly IRepository<Category> categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository) => this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));

        public void Create(string userId, CategoryDto instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(CategoryDto));
            }

            DateTime now = DateTime.Now;

            Category category = Mapper.Map<Category>(instance);
            category.Id = Guid.NewGuid();
            category.UserId = userId;
            category.IsDisabled = false;
            category.SortNumber = GetNewSortNumber(userId);
            category.CreatedTime = now;
            category.ModifiedTime = now;
            categoryRepository.Create(category);

            categoryRepository.SaveChanges();
        }

        private int GetNewSortNumber(string userId)
            => categoryRepository.Read(x => x.UserId == userId).SetNonDeleted()
                .OrderByDescending(x => x.SortNumber)
                .Select(x => x.SortNumber)
                .FirstOrDefault() + 1;

        public void Update(string userId, Guid id, CategoryDto instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(CategoryDto));
            }

            Category category = Mapper.Map(instance, Get(userId, id));
            category.ModifiedTime = DateTime.Now;
            categoryRepository.Update(category);

            categoryRepository.SaveChanges();
        }

        public void Enable(string userId, Guid id) {
            ToggleDisableState(userId, id, false);
        }

        public void Disable(string userId, Guid id) {
            ToggleDisableState(userId, id, true);
        }

        private void ToggleDisableState(string userId, Guid id, bool isDisabled) {
            Category category = Get(userId, id);
            category.IsDisabled = isDisabled;
            categoryRepository.Update(category);

            categoryRepository.SaveChanges();
        }

        public void MoveUp(string userId, Guid id) {
            Category thisCategory = Get(userId, id);
            Category prevCategory = categoryRepository.Read(x => x.UserId == userId && x.SortNumber == (thisCategory.SortNumber - 1))
                .SetNonDeleted()
                .FirstOrDefault();

            thisCategory.SortNumber--;
            categoryRepository.Update(thisCategory);

            prevCategory.SortNumber++;
            categoryRepository.Update(prevCategory);

            categoryRepository.SaveChanges();
        }

        public void MoveDown(string userId, Guid id) {
            Category thisCategory = Get(userId, id);
            Category nextCategory = categoryRepository.Read(x => x.UserId == userId && x.SortNumber == (thisCategory.SortNumber + 1))
                .SetNonDeleted()
                .First();

            thisCategory.SortNumber++;
            categoryRepository.Update(thisCategory);

            nextCategory.SortNumber--;
            categoryRepository.Update(nextCategory);

            categoryRepository.SaveChanges();
        }

        public void Delete(string userId, Guid id) {
            using (TransactionScope ts = new TransactionScope()) {
                Category category = Get(userId, id);
                category.DeletedTime = DateTime.Now;
                categoryRepository.Update(category);

                categoryRepository.SaveChanges();

                RefreshSortNumber(userId);
                ts.Complete();
            }
        }

        private void RefreshSortNumber(string userId) {
            int i = 1;
            foreach (Category category in categoryRepository.Read(x => x.UserId == userId).SetNonDeleted()) {
                category.SortNumber = i++;
                categoryRepository.Update(category);
            }
            categoryRepository.SaveChanges();
        }

        public Category Get(string userId, Guid id) {
            return CreateSingleQueryable(userId, id).Single();
        }

        public IQueryable<Category> Read(Expression<Func<Category, bool>> predicate = null) {
            IQueryable<Category> categories = categoryRepository.Read();
            if (predicate != null) {
                categories = categories.Where(predicate);
            }
            return categories;
        }

        public bool VerifyPermission(string userId, Guid id)
            => CreateSingleQueryable(userId, id).Any();

        private IQueryable<Category> CreateSingleQueryable(string userId, Guid id) {
            return categoryRepository
                .Read()
                .SetNonDeleted()
                .Where(x => x.Id == id && x.UserId == userId);
        }
    }
}
