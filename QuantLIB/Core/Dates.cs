using System;
using System.Collections.Generic;

namespace JHLib.QuantLIB.Core
{
    public class Frequency
    {
        public double frequency;
        public Frequency(double frequency) { this.frequency = frequency; }
        public double ToDays() { return frequency;  }
        public static Frequency d1 { get { return new Frequency(1); } }
        public static Frequency d2 { get { return new Frequency(2); } }
        public static Frequency w { get { return new Frequency(7); } }
    }

    public class Date
    {
        private DateTime _datetime;
        public Date(DateTime dateTime) { _datetime = dateTime.Date;  }

        public static Date Today { get { return new Date(DateTime.Now); } }
        public static Date Tomorrow { get { return Today + Frequency.d1; } }
        public static IEnumerable<Date> GenerateSchedule(Date startDate, Frequency frequency, int numberOfDates )
        {
            Date date = startDate;
            for(int i = 0; i < numberOfDates; i++)
            {
                yield return date;
                date += frequency;
            }
        }

        public static Date operator +(Date startDate, Frequency frequency)
        {
            return new Date(startDate._datetime.AddDays(frequency.ToDays()));
        }

        public static Date operator -(Date startDate, Frequency frequency)
        {
            return new Date(startDate._datetime.AddDays(-frequency.ToDays()));
        }

        public static TimeSpan operator -(Date date1, Date date2)
        {
            return date1._datetime.Subtract(date2._datetime);
        }

        public static bool operator <(Date date1, Date date2)
        {
            return date1._datetime < date2._datetime;
        }
        public static bool operator >(Date date1, Date date2)
        {
            return date1._datetime > date2._datetime;
        }

        public override string ToString()
        {
            return _datetime.ToShortDateString();
        }

        public override bool Equals(object obj)
        {
            return (obj is Date) && (_datetime == (obj as Date)._datetime);
        }
    }
}
