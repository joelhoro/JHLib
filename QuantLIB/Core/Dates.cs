using System;
using System.Collections.Generic;

namespace JHLib.QuantLIB.Core
{
    public class Frequency
    {
        public double frequency;
        public Frequency(double frequency) { this.frequency = frequency; }
        public double ToDays() { return frequency;  }
    }

    public class Date
    {
        private DateTime _datetime;
        public Date(DateTime dateTime) { _datetime = dateTime;  }

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

        public string ToString()
        {
            return _datetime.ToShortDateString();
        }
    }
}
