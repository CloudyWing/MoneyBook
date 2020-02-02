using System;
using System.Collections.Generic;
using System.Linq;
using CloudyWing.Constants;

namespace MoneyBook.Services.CategoryModel {

    [Serializable]
    public sealed class PayType : Constant<byte> {
        private static readonly Lazy<PayType> income = new Lazy<PayType>(() => new PayType(0, "收入"));
        private static readonly Lazy<PayType> expense = new Lazy<PayType>(() => new PayType(1, "支出"));

        private PayType(byte value, string text) : base(value, text) { }

        public static explicit operator PayType(byte value) {
            return GetItems().Where(x => x.Value == value).SingleOrDefault()
                ?? throw new InvalidCastException();
        }

        /// <summary>
        /// 收入(0)
        /// </summary>
        public static PayType Income => income.Value;

        /// <summary>
        /// 支出(1)
        /// </summary>
        public static PayType Expense => expense.Value;


        public static IEnumerable<PayType> GetItems() {
            yield return Income;
            yield return Expense;
        }
    }
}
