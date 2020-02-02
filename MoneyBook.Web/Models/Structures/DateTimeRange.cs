using System;

namespace MoneyBook.Web.Models {

    public struct DateTimeRange {

        public DateTimeRange(DateTime? startDateTime, DateTime? endDateTime) : this() {
            Start = startDateTime;
            End = endDateTime;
        }

        public static bool operator ==(DateTimeRange a, DateTimeRange b) {
            return a.Equals(b);
        }

        public static bool operator !=(DateTimeRange a, DateTimeRange b) {
            return !(a == b);
        }

        public DateTime? Start { get; private set; }

        public DateTime? End { get; private set; }

        public bool HasStart => Start.HasValue;

        public bool HasEnd => End.HasValue;

        public DateTime StartOrMin => Start ?? DateTime.MinValue;

        public DateTime EndOrMax => End ?? DateTime.MaxValue;

        /// <summary>
        /// Start == End
        /// </summary>
        public bool IsEmpty => Start == End;

        /// <summary>
        /// DateTime.MinValue ~ DateTime.MaxValue
        /// </summary>
        public static DateTimeRange Infinite() => new DateTimeRange(DateTime.MinValue, DateTime.MaxValue);

        public override bool Equals(object obj) {
            if (obj != null && obj is DateTimeRange) {
                Equals((DateTimeRange)obj);
            }
            return false;
        }

        public bool Equals(DateTimeRange range) {
            return Start == range.Start && End == range.End;
        }

        public override int GetHashCode() {
            return Start.GetHashCode() ^ End.GetHashCode();
        }

        public override string ToString() {
            return Start == null && End == null ? "" : $"{Start},{End}";
        }

        public static DateTimeRange Parse(string range) {
            string[] times = range.Split(',');

            if (times.Length == 0) {
                return new DateTimeRange();
            }

            if (times.Length != 2 ||
                !DateTime.TryParse(times[0].Trim(), out DateTime start) || !DateTime.TryParse(times[1].Trim(), out DateTime end)
            ) {
                throw new FormatException("Invalid point expression.");
            }

            return new DateTimeRange(start, end);
        }
    }
}