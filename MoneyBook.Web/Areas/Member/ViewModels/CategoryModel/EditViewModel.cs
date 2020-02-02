using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MoneyBook.Web.Areas.Member.ViewModels.CategoryModel {
    public class EditViewModel {
        public CategoryEditor Editor { get; set; }

        public IEnumerable<SelectListItem> PayTypes { get; set; }
    }

    public class CategoryEditor {
        public Guid? Id { get; set; }

        [Display(Name = "類別名稱")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "類別狀態")]
        [Required]
        public byte PayType { get; set; }
    }
}