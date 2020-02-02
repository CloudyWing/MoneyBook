using System;

namespace MoneyBook.Web.Models {

    public struct IntegerRange {

        public IntegerRange(int? startDateTime, int? endDateTime) : this() {
            Start = startDateTime;
            End = endDateTime;
        }

        public static bool operator ==(IntegerRange a, IntegerRange b) {
            return a.Equals(b);
        }

        public static bool operator !=(IntegerRange a, IntegerRange b) {
            return !(a == b);
        }

        public int? Start { get; private set; }

        public int? End { get; private set; }

        public bool HasStart => Start.HasValue;

        public bool HasEnd => End.HasValue;

        public int StartOrMin => Start ?? int.MinValue;

        public int EndOrMax => End ?? int.MaxValue;

        /// <summary>
        /// Start == End
        /// </summary>
        public bool IsEmpty => Start == End;

        /// <summary>
        /// int.MinValue ~ int.MaxValue
        /// </summary>
        public static IntegerRange Infinite() => new IntegerRange(int.MinValue, int.MaxValue);

        public override bool Equals(object obj) {
            if (obj != null && obj is IntegerRange) {
                Equals((IntegerRange)obj);
            }
            return false;
        }

        public bool Equals(IntegerRange range) {
            return Start == range.Start && End == range.End;
        }

        public override int GetHashCode() {
            return Start.GetHashCode() ^ End.GetHashCode();
        }

        public override string ToString() {
            return Start == null && End == null ? "" : $"{Start},{End}";
        }

        public static IntegerRange Parse(string range) {
            string[] integers = range.Split(',');

            if (integers.Length == 0) {
                return new IntegerRange();
            }

            if (integers.Length != 2 ||
                !int.TryParse(integers[0].Trim(), out int start) || !int.TryParse(integers[1].Trim(), out int end)
            ) {
                throw new FormatException("Invalid point expression.");
            }

            return new IntegerRange(start, end);
        }
    }
}