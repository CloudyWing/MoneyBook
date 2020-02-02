using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LinqKit;
using Microsoft.AspNet.Identity;
using MoneyBook.Repositories;
using MoneyBook.Services;
using MoneyBook.Services.CategoryItemModel;
using MoneyBook.Services.CategoryModel;
using MoneyBook.Web.Areas.Member.ViewModels.CategoryModel;
using MoneyBook.Web.Controllers;
using MoneyBook.Web.Models.SelectLists;

namespace MoneyBook.Web.Areas.Member.Controllers {
    public class CategoryController : BasicController {
        private const string ConditionKey = nameof(CategoryController) + "Condition";
        private const string PageNumberKey = nameof(CategoryController) + "PageNumber";
        private readonly ICategoryService categoryService;
        private readonly ICategoryItemService categoryItemService;
        private readonly IMapper mapper;

        public CategoryController(ICategoryService categoryService, ICategoryItemService categoryItemService, IMapper mapper) {
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.categoryItemService = categoryItemService ?? throw new ArgumentNullException(nameof(categoryItemService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public CategoryCondition TempCategoryCondition {
            get {
                if (TempData.ContainsKey(ConditionKey) && TempData[ConditionKey] != null) {
                    return (TempData[ConditionKey] as CategoryCondition) ?? new CategoryCondition();
                }
                return new CategoryCondition();
            }
            set => TempData[ConditionKey] = value;
        }

        public int TempPageNumber {
            get => TempData.ContainsKey(PageNumberKey) && TempData[PageNumberKey] != null ?
                (int)TempData[PageNumberKey] : 1;
            set => TempData[PageNumberKey] = value;
        }

        public ActionResult Index() {
            CategoryCondition condition = TempCategoryCondition;

            IndexViewModel model = new IndexViewModel() {
                Condition = condition,
                DataList = QueryDataListItems(condition),
                PayTypes = SelectListUtils.CreatePayTypes(),
                OptionStates = SelectListUtils.CreateOptionStates()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model) {
            IndexViewModel result = new IndexViewModel() {
                Condition = model.Condition,
                DataList = QueryDataListItems(model.Condition),
                PayTypes = SelectListUtils.CreatePayTypes(),
                OptionStates = SelectListUtils.CreateOptionStates()
            };

            return View(result);
        }

        private ActionResult AjaxIndex() {
            CategoryCondition condition = TempCategoryCondition;

            IndexViewModel result = new IndexViewModel() {
                Condition = condition,
                DataList = QueryDataListItems(condition ?? new CategoryCondition())
            };

            return PartialView("_List", result);
        }

        [HttpPost]
        public ActionResult AjaxIndex(CategoryCondition condition) {
            IndexViewModel result = new IndexViewModel() {
                Condition = condition,
                DataList = QueryDataListItems(condition ?? new CategoryCondition())
            };

            return PartialView("_List", result);
        }

        private IEnumerable<CategoryDataListItem> QueryDataListItems(CategoryCondition condition) {
            TempCategoryCondition = condition;

            string userId = User.Identity.GetUserId();

            var query = categoryService.Read().GroupJoin(
                categoryItemService.Read(),
                x => new { x.Id, HasValue = false },
                x => new { Id = x.CategoryId, x.DeletedTime.HasValue },
                (x, items) => new { x, items }
            ).SelectMany(temp => temp.items.DefaultIfEmpty(), (temp, item) => new ComplexCategory() {
                Category = temp.x,
                CategoryItem = item
            }).Where(x => x.Category.UserId == userId && !x.Category.DeletedTime.HasValue);


            if (!string.IsNullOrWhiteSpace(condition.Name)) {
                var pred = PredicateBuilder.New<ComplexCategory>();
                pred = pred.Or(x => x.Category.Name.Contains(condition.Name));
                if (condition.IsContainsItem) {
                    pred = pred.Or(x => x.CategoryItem.Name.Contains(condition.Name));
                }
                query = query.Where(pred);
            }

            if (condition.PayTypes != null && condition.PayTypes.Any()) {
                var pred = PredicateBuilder.New<ComplexCategory>();
                foreach (byte payType in condition.PayTypes) {
                    pred = pred.Or(x => x.Category.PayType == payType);
                }
                query = query.Where(pred);
            }

            if (condition.OptionStates?.Count() == 1) {
                bool optionState = condition.OptionStates.First();
                query = query.Where(x => x.Category.IsDisabled != optionState);
            }

            return query.Select(x => new { x.Category, x.CategoryItem, HasRecords = x.CategoryItem.Records.Any() })
                .OrderBy(x => x.Category.SortNumber).ThenBy(x => x.CategoryItem.SortNumber)
                .AsEnumerable()
                .GroupBy(x => x.Category).Select(x => new CategoryDataListItem() {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    PayType = x.Key.PayType,
                    IsDisabled = x.Key.IsDisabled,
                    CategoryItems = x.Count(y => y.CategoryItem == null) > 0 ? new List<CategoryItemDataListItem>() : x.Select(y => new CategoryItemDataListItem() {
                        Id = y.CategoryItem.Id,
                        Name = y.CategoryItem.Name,
                        IsDisabled = y.CategoryItem.IsDisabled,
                        HasRecords = y.HasRecords
                    })
                });
        }

        public ActionResult Create() {
            return PartialView("_Edit", new EditViewModel() {
                Editor = new CategoryEditor(),
                PayTypes = SelectListUtils.CreatePayTypes()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditViewModel model) {
            model.PayTypes = SelectListUtils.CreatePayTypes();

            if (!ModelState.IsValid) {
                return PartialView("_Edit", model);
            }

            CategoryDto instance = mapper.Map<CategoryDto>(model.Editor);
            categoryService.Create(User.Identity.GetUserId(), instance);

            return Json(Result.Success());
        }

        public ActionResult Edit(Guid id) {
            if (!categoryService.VerifyPermission(User.Identity.GetUserId(), id)) {
                return HttpForbidden();
            }

            EditViewModel model = new EditViewModel() {
                Editor = mapper.Map<CategoryEditor>(categoryService.Get(User.Identity.GetUserId(), id)),
                PayTypes = SelectListUtils.CreatePayTypes()
            };

            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model) {
            model.PayTypes = SelectListUtils.CreatePayTypes();

            if (!ModelState.IsValid) {
                return PartialView("_Edit", model);
            }
            CategoryDto instance = mapper.Map<CategoryDto>(model.Editor);
            categoryService.Update(User.Identity.GetUserId(), (Guid)model.Editor.Id, instance);

            return Json(Result.Success());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id) {
            categoryService.Delete(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enable(Guid id) {
            categoryService.Enable(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disable(Guid id) {
            categoryService.Disable(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveUp(Guid id) {
            categoryService.MoveUp(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveDown(Guid id) {
            categoryService.MoveDown(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        public ActionResult CreateItem() {
            return PartialView("_EditItem", new EditItemViewModel() {
                Editor = new CategoryItemEditor(),
                Categories = SelectListUtils.CreateCategories(categoryService, User.Identity.GetUserId())
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem(EditItemViewModel model) {
            model.Categories = SelectListUtils.CreateCategories(categoryService, User.Identity.GetUserId());

            if (!ModelState.IsValid) {
                return PartialView("_EditItem", model);
            }

            CategoryItemDto instance = mapper.Map<CategoryItemDto>(model.Editor);
            categoryItemService.Create(User.Identity.GetUserId(), instance);

            return Json(Result.Success());
        }

        public ActionResult EditItem(Guid id) {
            if (!categoryItemService.VerifyPermission(User.Identity.GetUserId(), id)) {
                return HttpForbidden();
            }

            EditItemViewModel model = new EditItemViewModel() {
                Editor = mapper.Map<CategoryItemEditor>(categoryItemService.Get(User.Identity.GetUserId(), id)),
                Categories = SelectListUtils.CreateCategories(categoryService, User.Identity.GetUserId())
            };

            return View("_EditItem", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(EditItemViewModel model) {
            model.Categories = SelectListUtils.CreateCategories(categoryService, User.Identity.GetUserId());

            if (!ModelState.IsValid) {
                return PartialView("_Edit", model);
            }
            CategoryItemDto instance = mapper.Map<CategoryItemDto>(model.Editor);

            categoryItemService.Update(User.Identity.GetUserId(), (Guid)model.Editor.Id, instance);
            return Json(Result.Success());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(Guid id) {
            categoryItemService.Delete(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnableItem(Guid id) {
            categoryItemService.Enable(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisableItem(Guid id) {
            categoryItemService.Disable(User.Identity.GetUserId(), id);
            return AjaxIndex(TempCategoryCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveUpItem(Guid id) {
            categoryService.MoveUp(User.Identity.GetUserId(), id);
            return AjaxIndex();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveDownItem(Guid id) {
            categoryItemService.MoveDown(User.Identity.GetUserId(), id);
            return AjaxIndex();
        }

        public class CategoryProfile : Profile {
            public CategoryProfile() {
                CreateMap<CategoryEditor, CategoryDto>()
                    .ForMember(x => x.PayType, opt => opt.MapFrom(x => (PayType)x.PayType));

                CreateMap<Category, CategoryEditor>();

                CreateMap<CategoryItemEditor, CategoryItemDto>();

                CreateMap<CategoryItem, CategoryItemEditor>();
            }
        }
    }
}
