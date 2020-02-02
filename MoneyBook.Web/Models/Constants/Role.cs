using System;
using System.Collections.Generic;
using System.Linq;
using CloudyWing.Constants;

namespace MoneyBook.Web.Models {
    [Serializable]
    public sealed class Role : Constant<string> {
        private static readonly Lazy<Role> administrator = new Lazy<Role>(() => new Role(nameof(Administrator), "管理者"));
        private static readonly Lazy<Role> member = new Lazy<Role>(() => new Role(nameof(Member), "會員"));

        private Role(string value, string text)
            : base(value, text) {
        }

        public static Role Administrator => administrator.Value;

        public static Role Member => member.Value;

        public static explicit operator Role(string value) {
            return GetItems().Where(x => x.Value == value).SingleOrDefault()
                ?? throw new InvalidCastException();
        }

        public static IEnumerable<Role> GetItems() {
            yield return Administrator;
            yield return Member;
        }
    }
}
