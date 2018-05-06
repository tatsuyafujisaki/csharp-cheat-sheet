using System;

namespace CheatSheet.DateTime1
{
    sealed class HourMinute : IEquatable<HourMinute>
    {
        internal readonly int Hour;
        internal readonly int Minute;

        internal HourMinute(int hour, int minute)
        {
            if (hour < 0 || 23 < hour)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), hour, "Must be between 0 and 23.");
            }

            if (minute < 0 || 59 < minute)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), minute, "Must be between 0 and 59.");
            }

            Hour = hour;
            Minute = minute;
        }

        internal HourMinute(TimeSpan ts)
        {
            Hour = ts.Hours;
            Minute = ts.Minutes;
        }

        public override int GetHashCode()
        {
            return Hour ^ Minute;
        }

        public override bool Equals(object o)
        {
            var hm = o as HourMinute;

            if (hm == null)
            {
                return false;
            }

            return Hour == hm.Hour && Minute == hm.Minute;
        }

        public bool Equals(HourMinute hm)
        {
            if (hm == null)
            {
                return false;
            }

            return Hour == hm.Hour && Minute == hm.Minute;
        }

        public static bool operator ==(HourMinute hm1, HourMinute hm2)
        {
            // Both are null or the same object.
            if (ReferenceEquals(hm1, hm2))
            {
                return true;
            }

            // Call the == operator of object.
            if ((object)hm1 == (object)hm2)
            {
                return false;
            }

            return hm1.Equals(hm2);
        }

        public static bool operator !=(HourMinute hm1, HourMinute hm2)
        {
            if (hm1 == null)
            {
                return !(hm2 == null);
            }

            return !hm1.Equals(hm2);
        }

        public static bool operator <(HourMinute hm1, HourMinute hm2)
        {
            if (hm1 == null || hm2 == null)
            {
                throw new InvalidOperationException();
            }

            return hm1.Hour == hm2.Hour ? hm1.Minute < hm2.Minute : hm1.Hour < hm2.Hour;
        }

        public static bool operator >(HourMinute hm1, HourMinute hm2)
        {
            if (hm1 == null || hm2 == null)
            {
                throw new InvalidOperationException();
            }

            return hm1.Hour == hm2.Hour ? hm1.Minute > hm2.Minute : hm1.Hour > hm2.Hour;
        }

        public static bool operator <=(HourMinute hm1, HourMinute hm2)
        {
            if (hm1 == null || hm2 == null)
            {
                throw new InvalidOperationException();
            }

            return hm1.Hour == hm2.Hour ? hm1.Minute <= hm2.Minute : hm1.Hour <= hm2.Hour;
        }

        public static bool operator >=(HourMinute hm1, HourMinute hm2)
        {
            if (hm1 == null || hm2 == null)
            {
                throw new InvalidOperationException();
            }

            return hm1.Hour == hm2.Hour ? hm1.Minute >= hm2.Minute : hm1.Hour >= hm2.Hour;
        }
    }
}