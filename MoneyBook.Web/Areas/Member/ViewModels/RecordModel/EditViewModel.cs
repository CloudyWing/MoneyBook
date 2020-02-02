using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyBook.Web.Areas.Member.ViewModels.RecordModel {
    public class EditViewModel {
        public RecordEditor Editor { get; set; }
    }

    public class RecordEditor {
        public Guid? Id { get; set; }

        [Display(Name = "收入/支出")]
        public byte PayType { get; set; }

        [Display(Name = "類別")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "類別細項")]
        [Required]
        public Guid? CategoryItemId { get; set; }

        [Display(Name = "交易日期")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? TradeDate { get; set; }

        [Display(Name = "交易金額")]
        [Required]
        [Range(0, int.MaxValue)]
        public int Money { get; set; }

        [Display(Name = "備註")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Note { get; set; }
    }
}