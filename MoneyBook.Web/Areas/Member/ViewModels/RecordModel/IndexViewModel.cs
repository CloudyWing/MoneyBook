using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MoneyBook.Services.CategoryModel;
using MoneyBook.Web.Models;
using PagedList;

namespace MoneyBook.Web.Areas.Member.ViewModels.RecordModel {
    public class IndexViewModel {
        public RecordCondition Condition { get; set; }

        public IPagedList<RecordDataListItem> DataList { get; set; }

        public int PageNumber { get; set; }

        public IEnumerable<SelectListItem> PayTypes { get; set; }
    }

    public class RecordCondition {
        [Display(Name = "收入/支出")]
        public byte? PayType { get; set; }

        [Display(Name = "類別")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "類別細項")]
        public Guid? CategoryItemId { get; set; }

        [Display(Name = "交易日期")]
        public DateTimeRange TradeDateRange { get; set; }

        [Display(Name = "交易日期(起)")]
        [DataType(DataType.Date)]
        public DateTime? TradeDateStartRange { get; set; }

        [Display(Name = "交易日期(迄)")]
        [DataType(DataType.Date)]
        public DateTime? TradeDateEndRange { get; set; }

        [Display(Name = "金額")]
        public IntegerRange MoneyRange { get; set; }

        [Display(Name = "金額(起)")]
        [Range(0, int.MaxValue)]
        public int? MoneyStartRange { get; set; }

        [Display(Name = "金額(迄)")]
        [Range(0, int.MaxValue)]
        public int? MoneyEndRange { get; set; }

        [Display(Name = "備註")]
        public string Note { get; set; }
    }

    public class RecordDataListItem {
        public Guid Id { get; set; }

        [Display(Name = "交易日期")]
        [DataType(DataType.Date)]
        public DateTime TradeDate { get; set; }

        [Display(Name = "交易金額")]
        public int Money { get; set; }

        [Display(Name = "備註")]
        public string Note { get; set; }

        public CategoryItemDataListItem CategoryItem { get; set; }
    }

    public class CategoryItemDataListItem {
        [Display(Name = "類別細項")]
        public string Name { get; set; }

        public CategoryDataListItem Category { get; set; }
    }

    public class CategoryDataListItem {
        [Display(Name = "類別")]
        public string Name { get; set; }

        [Display(Name = "收支出類型")]
        public byte PayType { get; set; }

        public PayType PayTypeConstant => (PayType)PayType;
    }
}