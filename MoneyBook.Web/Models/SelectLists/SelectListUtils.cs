using System;
using System.Collections;
using System.Linq;
using MoneyBook.Repositories;
using MoneyBook.Services.CategoryItemModel;
using MoneyBook.Services.CategoryModel;
using MoneyBook.Web.Models.Constants;

namespace MoneyBook.Web.Models.SelectLists {
    public static class SelectListUtils {
        public static GenericSelectList CreatePayTypes(IEnumerable disabledValues = null) {
            return new GenericSelectList(PayType.GetItems(), "Value", "Text", disabledValues);
        }

        public static GenericSelectList CreateOptionStates(IEnumerable disabledValues = null) {
            return new GenericSelectList(OptionState.GetItems(), "Value", "Text", disabledValues);
        }

        public static GenericSelectList CreateIncomeCategories(ICategoryService service, string userId) {
            return CreateCategories(service, userId, PayType.Income);
        }

        public static GenericSelectList CreateExpenseCategories(ICategoryService service, string userId) {
            return CreateCategories(service, userId, PayType.Expense);
        }

        public static GenericSelectList CreateCategories(ICategoryService service, string userId, PayType payType = null) {
            IQueryable<Category> query = service.Read(x => x.UserId == userId).SetNonDeleted().SetEnabled();
            if (payType != null) {
                query = query.Where(x => x.PayType == payType.Value);
            }
            return new GenericSelectList(query, "Id", "Name");
        }

        public static GenericSelectList CreateIncomeCategoryItems(ICategoryItemService service, string userId, Guid? categoryId = null) {
            return CreateCategoryItems(service, userId, categoryId, PayType.Income);
        }

        public static GenericSelectList CreateExpenseCategoryItems(ICategoryItemService service, string userId, Guid? categoryId = null) {
            return CreateCategoryItems(service, userId, categoryId, PayType.Expense);
        }

        public static GenericSelectList CreateCategoryItems(ICategoryItemService service, string userId, Guid? categoryId = null, PayType payType = null) {
            IQueryable<CategoryItem> query = service
                .Read(x => x.Category.UserId == userId)
                .SetNonDeleted()
                .SetEnabled();

            if (categoryId.HasValue) {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (payType != null) {
                query = query.Where(x => x.Category.PayType == payType.Value);
            }

            return new GenericSelectList(query, "Id", "Name");
        }
    }
}