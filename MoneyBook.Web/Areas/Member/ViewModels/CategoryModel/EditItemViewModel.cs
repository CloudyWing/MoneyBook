using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MoneyBook.Web.Areas.Member.ViewModels.CategoryModel {

    public class EditItemViewModel {
        public CategoryItemEditor Editor { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }

    public class CategoryItemEditor {
        public Guid? Id { get; set; }

        [Display(Name = "類別細項名稱")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "類別編號")]
        [Required]
        public string CategoryId { get; set; }
    }
}