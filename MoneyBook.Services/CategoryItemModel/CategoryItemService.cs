using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using MoneyBook.Repositories;

namespace MoneyBook.Services.CategoryItemModel {
    public class CategoryItemService : ServiceBase, ICategoryItemService {
        private readonly IRepository<CategoryItem> categoryItemRepository;

        public CategoryItemService(IRepository<CategoryItem> categoryItemRepository) => this.categoryItemRepository = categoryItemRepository ?? throw new ArgumentNullException(nameof(categoryItemRepository));

        public void Create(string userId, CategoryItemDto instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(CategoryItemDto));
            }

            DateTime now = DateTime.Now;

            CategoryItem categoryItem = Mapper.Map<CategoryItem>(instance);
            categoryItem.Id = Guid.NewGuid();
            categoryItem.SortNumber = GetNewSortNumber(userId, categoryItem.CategoryId);
            categoryItem.IsDisabled = false;
            categoryItem.CreatedTime = now;
            categoryItem.ModifiedTime = now;
            categoryItemRepository.Create(categoryItem);

            categoryItemRepository.SaveChanges();
        }

        private int GetNewSortNumber(string userId, Guid categoryId) {
            return categoryItemRepository.Read(x => x.Category.UserId == userId && x.CategoryId == categoryId)
                .SetNonDeleted()
                .OrderByDescending(x => x.SortNumber)
                .Select(x => x.SortNumber)
                .FirstOrDefault() + 1;
        }

        public void Update(string userId, Guid id, CategoryItemDto instance) {
            using (TransactionScope ts = new TransactionScope()) {
                CategoryItem categoryItem = Get(userId, id);
                Guid oldCategoryItemId = categoryItem.CategoryId;

                Mapper.Map(instance, categoryItem);
                categoryItem.ModifiedTime = DateTime.Now;
                categoryItemRepository.Update(categoryItem);

                categoryItemRepository.SaveChanges();

                if (oldCategoryItemId != categoryItem.CategoryId) {
                    RefreshSortNumber(oldCategoryItemId);
                    RefreshSortNumber(categoryItem.CategoryId);
                }
                ts.Complete();
            }
        }

        private void RefreshSortNumber(Guid categoryId) {
            int i = 1;
            IQueryable<CategoryItem> query = categoryItemRepository
                .Read()
                .Where(x => x.CategoryId == categoryId)
                .SetNonDeleted();

            foreach (CategoryItem categoryItem in query) {
                categoryItem.SortNumber = i++;
                categoryItemRepository.Update(categoryItem);
            }
            categoryItemRepository.SaveChanges();
        }

        public void Enable(string userId, Guid id) {
            ToggleDisableState(userId, id, false);
        }

        public void Disable(string userId, Guid id) {
            ToggleDisableState(userId, id, true);
        }

        private void ToggleDisableState(string userId, Guid id, bool isDisabled) {
            CategoryItem categoryItem = Get(userId, id);
            categoryItem.IsDisabled = isDisabled;
            categoryItemRepository.Update(categoryItem);

            categoryItemRepository.SaveChanges();
        }

        public void MoveUp(string userId, Guid id) {
            CategoryItem thisCategoryItem = Get(userId, id);
            CategoryItem prevCategoryItem = categoryItemRepository.Read(
                    x => x.Category.UserId == userId &&
                        x.CategoryId == thisCategoryItem.CategoryId &&
                        x.SortNumber == (thisCategoryItem.SortNumber - 1)
                )
                .SetNonDeleted()
                .FirstOrDefault();

            thisCategoryItem.SortNumber--;
            categoryItemRepository.Update(thisCategoryItem);

            prevCategoryItem.SortNumber++;
            categoryItemRepository.Update(prevCategoryItem);

            categoryItemRepository.SaveChanges();
        }

        public void MoveDown(string userId, Guid id) {
            CategoryItem thisCategoryItem = Get(userId, id);
            CategoryItem nextCategoryItem = categoryItemRepository.Read(
                    x => x.Category.UserId == userId &&
                        x.CategoryId == thisCategoryItem.CategoryId &&
                        x.SortNumber == (thisCategoryItem.SortNumber + 1)
                )
                .SetNonDeleted()
                .FirstOrDefault();

            thisCategoryItem.SortNumber++;
            categoryItemRepository.Update(thisCategoryItem);

            nextCategoryItem.SortNumber--;
            categoryItemRepository.Update(nextCategoryItem);

            categoryItemRepository.SaveChanges();
        }

        public void Delete(string userId, Guid id) {
            CategoryItem categoryItem = Get(userId, id);
            categoryItem.DeletedTime = DateTime.Now;
            categoryItemRepository.Update(categoryItem);

            categoryItemRepository.SaveChanges();

            RefreshSortNumber(categoryItem.CategoryId);
        }

        public CategoryItem Get(string userId, Guid id) {
            return CreateSingleQueryable(userId, id).Single();
        }

        public IQueryable<CategoryItem> Read(Expression<Func<CategoryItem, bool>> predicate = null) {
            IQueryable<CategoryItem> categoryItems = categoryItemRepository.Read();
            if (predicate != null) {
                categoryItems = categoryItems.Where(predicate);
            }
            return categoryItems;
        }

        public bool VerifyPermission(string userId, Guid id)
            => CreateSingleQueryable(userId, id).Any();

        private IQueryable<CategoryItem> CreateSingleQueryable(string userId, Guid id) {
            return categoryItemRepository
                .Read()
                .Include(x => x.Category)
                .SetNonDeleted()
                .Where(x => x.Category.UserId == userId && x.Id == id);
        }
    }
}
