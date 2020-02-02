using System.Linq;
using MoneyBook.Repositories;

namespace MoneyBook.Services.CategoryItemModel {
    public static class CategoryItemExtensions {
        public static IQueryable<CategoryItem> SetNonDeleted(this IQueryable<CategoryItem> soruce)
            => soruce.Where(x => !x.DeletedTime.HasValue && !x.Category.DeletedTime.HasValue);

        public static IQueryable<CategoryItem> SetEnabled(this IQueryable<CategoryItem> soruce)
            => soruce.SetNonDeleted().Where(x => !x.IsDisabled && !x.Category.IsDisabled);
    }
}
