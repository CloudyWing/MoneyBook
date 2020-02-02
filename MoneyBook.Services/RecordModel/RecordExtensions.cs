using System;
using System.Linq;
using MoneyBook.Repositories;

namespace MoneyBook.Services.RecordModel {
    public static class RecordExtensions {
        public static IQueryable<Record> SetNonDeleted(
            this IQueryable<Record> soruce
        ) {
            return soruce.Where(x => !x.DeletedTime.HasValue);
        }

        public static IQueryable<Record> SetTradeDateRange(
            this IQueryable<Record> soruce, DateTime? tradeDateStartRange, DateTime? tradeDateEndRange
        ) {
            if (tradeDateStartRange.HasValue) {
                soruce = soruce.Where(x => x.TradeDate >= tradeDateStartRange);
            }

            if (tradeDateEndRange.HasValue) {
                soruce = soruce.Where(x => x.TradeDate <= tradeDateEndRange);
            }
            return soruce;
        }

        public static IQueryable<Record> SetMoneyRange(
            this IQueryable<Record> soruce, int? moneyStartRange, int? moneyEndRange
        ) {
            if (moneyStartRange.HasValue) {
                soruce = soruce.Where(x => x.Money >= moneyStartRange);
            }

            if (moneyEndRange.HasValue) {
                soruce = soruce.Where(x => x.Money <= moneyEndRange);
            }
            return soruce;
        }
    }
}
