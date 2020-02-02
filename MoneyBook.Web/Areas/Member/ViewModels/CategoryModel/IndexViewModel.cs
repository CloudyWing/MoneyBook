using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MoneyBook.Repositories;
using MoneyBook.Services.CategoryModel;
using MoneyBook.Web.Models.Constants;

namespace MoneyBook.Web.Areas.Member.ViewModels.CategoryModel {
    public class IndexViewModel {
        public CategoryCondition Condition { get; set; }

        public IEnumerable<CategoryDataListItem> DataList { get; set; }

        public IEnumerable<SelectListItem> PayTypes { get; set; }

        public IEnumerable<SelectListItem> OptionStates { get; set; }
    }

    public class CategoryCondition {
        [Display(Name = "類別名稱")]
        public string Name { get; set; }

        [Display(Name = "是否包含類別細項")]
        public bool IsContainsItem { get; set; }

        [Display(Name = "收支出類型")]
        public IEnumerable<byte> PayTypes { get; set; }

        [Display(Name = "類別狀態")]
        public IEnumerable<bool> OptionStates { get; set; }
    }

    public class CategoryDataListItem {
        [Display(Name = "類別編號")]
        public Guid Id { get; set; }

        [Display(Name = "類別名稱")]
        public string Name { get; set; }

        [Display(Name = "收支出類型")]
        public byte PayType { get; set; }

        public PayType PayTypeConstant => ((PayType)PayType);

        [Display(Name = "是否停用類別")]
        public bool IsDisabled { get; set; }

        public OptionState OptionState => !IsDisabled;

        public IEnumerable<CategoryItemDataListItem> CategoryItems { get; set; }
    }

    public class ComplexCategory {
        public Category Category { get; set; }

        public CategoryItem CategoryItem { get; set; }
    }

    public class CategoryItemDataListItem {
        [Display(Name = "類別細項編號")]
        public Guid Id { get; set; }

        [Display(Name = "類別細項名稱")]
        public string Name { get; set; }

        [Display(Name = "是否停用類別")]
        public bool IsDisabled { get; set; }

        public OptionState OptionState => !IsDisabled;

        public bool HasRecords { get; set; }
    }
}