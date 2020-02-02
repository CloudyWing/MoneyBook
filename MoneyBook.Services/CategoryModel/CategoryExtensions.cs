using System.Linq;
using MoneyBook.Repositories;

namespace MoneyBook.Services.CategoryModel {
    public static class CategoryExtensions {
        public static IQueryable<Category> SetNonDeleted(this IQueryable<Category> soruce) {
            return soruce.Where(x => !x.DeletedTime.HasValue);
        }

        public static IQueryable<Category> SetEnabled(this IQueryable<Category> soruce) {
            return soruce.SetNonDeleted().Where(x => !x.IsDisabled);
        }
    }
}
