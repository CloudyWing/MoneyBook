using System;
using System.Collections.Generic;
using CloudyWing.Constants;

namespace MoneyBook.Web.Models.Constants {
    /// <summary>
    /// Enable / Disable的常數物件
    /// </summary>
    [Serializable]
    public sealed class OptionState : Constant<bool> {
        private static readonly Lazy<OptionState> enable = new Lazy<OptionState>(() => new OptionState(true, "啟用", nameof(Enable)));
        private static readonly Lazy<OptionState> disable = new Lazy<OptionState>(() => new OptionState(false, "停用", nameof(Disable)));

        private OptionState(bool value, string text, string name)
            : base(value, text) {
            Name = name;
        }

        private static OptionState Enable => enable.Value;

        private static OptionState Disable => disable.Value;

        // 物件名稱
        public string Name { get; set; }

        public static implicit operator OptionState(bool value) {
            return value ? Enable : Disable;
        }

        public static IEnumerable<OptionState> GetItems() {
            yield return Enable;
            yield return Disable;
        }
    }
}
