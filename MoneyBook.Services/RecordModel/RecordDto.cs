using System;

namespace MoneyBook.Services.RecordModel {
    public class RecordDto {
        public Guid CategoryItemId { get; set; }

        public DateTime TradeDate { get; set; }

        public int Money { get; set; }

        public string Note { get; set; }
    }
}
