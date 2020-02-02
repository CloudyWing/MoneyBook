using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Microsoft.AspNet.Identity;
using MoneyBook.Repositories;
using MoneyBook.Services;
using MoneyBook.Services.CategoryItemModel;
using MoneyBook.Services.CategoryModel;
using MoneyBook.Services.RecordModel;
using MoneyBook.Web.Areas.Member.ViewModels.RecordModel;
using MoneyBook.Web.Controllers;
using MoneyBook.Web.Models.SelectLists;
using PagedList;

namespace MoneyBook.Web.Areas.Member.Controllers {
    public class RecordController : BasicController {
        private const string ConditionKey = nameof(RecordController) + "Condition";
        private const string PageNumberKey = nameof(RecordController) + "PageNumber";

        private readonly IRecordService recordService;
        private readonly ICategoryService categoryService;
        private readonly ICategoryItemService categoryItemService;
        private readonly IMapper mapper;

        public RecordController(IRecordService recordService, ICategoryService categoryService, ICategoryItemService categoryItemService, IMapper mapper) {
            this.recordService = recordService ?? throw new ArgumentNullException(nameof(recordService));
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.categoryItemService = categoryItemService ?? throw new ArgumentNullException(nameof(categoryItemService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public RecordCondition TempCondition {
            get {
                if (TempData.ContainsKey(ConditionKey) && TempData[ConditionKey] != null) {
                    return (TempData[ConditionKey] as RecordCondition) ?? new RecordCondition();
                }
                return new RecordCondition();
            }
            set => TempData[ConditionKey] = value;
        }

        public int TempPageNumber {
            get => TempData.ContainsKey(PageNumberKey) && TempData[PageNumberKey] != null ?
                (int)TempData[PageNumberKey] : 1;
            set => TempData[PageNumberKey] = value;
        }

        public ActionResult Index(int pageNumber = 1) {
            RecordCondition condition = TempCondition;

            IndexViewModel model = new IndexViewModel() {
                Condition = condition,
                DataList = QueryDataListItems(pageNumber, condition),
                PageNumber = pageNumber,
                PayTypes = SelectListUtils.CreatePayTypes()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model) {
            int pageNumber = model.PageNumber < 1 ? 1 : model.PageNumber;

            IndexViewModel result = new IndexViewModel() {
                Condition = model.Condition,
                DataList = QueryDataListItems(pageNumber, model.Condition),
                PageNumber = pageNumber,
                PayTypes = SelectListUtils.CreatePayTypes()
            };

            return View(result);
        }

        [HttpPost]
        public ActionResult AjaxIndex(int pageNumber, RecordCondition condition) {

            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            IndexViewModel result = new IndexViewModel() {
                Condition = condition,
                DataList = QueryDataListItems(pageNumber, condition ?? new RecordCondition()),
                PageNumber = pageNumber
            };

            return PartialView("_List", result);
        }

        private IPagedList<RecordDataListItem> QueryDataListItems(int pageNumber, RecordCondition condition) {
            TempPageNumber = pageNumber;
            TempCondition = condition;
            // Linq Where()裡面不能呼叫GetUserId();
            string userId = User.Identity.GetUserId();

            IQueryable<Record> query = recordService.Read()
                .Include(x => x.CategoryItem)
                    .Include(x => x.CategoryItem.Category)
                .Where(x => x.CategoryItem.Category.UserId == userId)
                .SetNonDeleted()
                .SetTradeDateRange(condition.TradeDateStartRange, condition.TradeDateEndRange)
                .SetMoneyRange(condition.MoneyStartRange, condition.MoneyEndRange);

            if (condition.PayType != null) {
                query = query.Where(x => x.CategoryItem.Category.PayType == condition.PayType);
            }

            if (condition.CategoryId != null) {
                query = query.Where(x => x.CategoryItem.CategoryId == condition.CategoryId);
            }

            if (condition.CategoryItemId != null) {
                query = query.Where(x => x.CategoryItemId == condition.CategoryItemId);
            }

            if (condition.Note != null) {
                query = query.Where(x => x.Note.Contains(condition.Note));
            }

            IPagedList<Record> pagedList = query.OrderByDescending(x => x.TradeDate).ToPagedList(pageNumber, PageSize);

            return new StaticPagedList<RecordDataListItem>(
                mapper.Map<IEnumerable<RecordDataListItem>>(pagedList), pagedList.GetMetaData()
            );
        }

        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public ActionResult GetPayTypes() {
            return Json(SelectListUtils.CreatePayTypes());
        }

        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
        public ActionResult GetCategories(byte prevValue) {
            return Json(SelectListUtils.CreateCategories(categoryService, User.Identity.GetUserId(), (PayType)prevValue));

        }

        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
        public ActionResult GetCategoryItems(Guid prevValue) {
            return Json(SelectListUtils.CreateCategoryItems(categoryItemService, User.Identity.GetUserId(), prevValue));
        }

        public ActionResult Create() {
            return PartialView("_Edit", new EditViewModel() {
                Editor = new RecordEditor() {
                    PayType = 0,
                    TradeDate = DateTime.Today
                }
            });
        }

        public ActionResult CreateNext(RecordEditor editor) {
            ViewBag.Message = "執行成功...";
            MapperConfiguration config = new MapperConfiguration(
                cfg => cfg.CreateMap<RecordEditor, RecordEditor>()
                    .ForMember(x => x.Money, y => y.Ignore())
                    .ForMember(x => x.Note, y => y.Ignore())
            );

            return PartialView("_Edit", new EditViewModel() {
                Editor = config.CreateMapper().Map<RecordEditor>(editor)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditViewModel model) {
            if (!ModelState.IsValid) {
                return PartialView("_Edit", model);
            }

            CreateInternal(model);
            return Json(Result.Success());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNext(EditViewModel model) {
            if (!ModelState.IsValid) {
                return PartialView("_Edit", model);
            }

            CreateInternal(model);
            return RedirectToAction("CreateNext", model.Editor);
        }

        public void CreateInternal(EditViewModel model) {
            RecordDto instance = mapper.Map<RecordDto>(model.Editor);
            recordService.Create(User.Identity.GetUserId(), instance);
        }

        public ActionResult Edit(Guid id) {
            if (!recordService.VerifyPermission(User.Identity.GetUserId(), id)) {
                return HttpForbidden();
            }

            EditViewModel model = new EditViewModel() {
                Editor = mapper.Map<RecordEditor>(recordService.Get(User.Identity.GetUserId(), id))
            };

            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model) {
            if (!ModelState.IsValid) {
                return PartialView("_Edit", model);
            }
            RecordDto instance = mapper.Map<RecordDto>(model.Editor);
            recordService.Update(User.Identity.GetUserId(), (Guid)model.Editor.Id, instance);

            return Json(Result.Success());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id) {
            recordService.Delete(User.Identity.GetUserId(), id);
            return AjaxIndex(TempPageNumber, TempCondition);
        }

        public class RecordProfile : Profile {
            public RecordProfile() {
                CreateMap<RecordEditor, RecordDto>();

                CreateMap<Record, RecordEditor>()
                    .ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.CategoryItem.CategoryId))
                    .ForMember(x => x.PayType, opt => opt.MapFrom(x => x.CategoryItem.Category.PayType));

                CreateMap<Record, RecordDataListItem>();
                CreateMap<CategoryItem, CategoryItemDataListItem>();
                CreateMap<Category, CategoryDataListItem>();
            }
        }
    }
}
