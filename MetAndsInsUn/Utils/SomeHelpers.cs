using MetAndsInsUn.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MetAndsInsUn.Utils
{
    public class SomeHelpers
    {
        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static List<WeeksCal> GenerateWeeks(int year)
        {
            List<WeeksCal> dates = new List<WeeksCal>();        
                        
            for (int i = 1; i < 53; i++)
            {
                DateTime dtIni = FirstDateOfWeekISO8601(year, i);
                DateTime dtFin = FirstDateOfWeekISO8601(year, i + 1).AddDays(-1);
                dates.Add(new WeeksCal()
                {
                    fecha = "Semana " + i + ". Desde " + dtIni.ToString("dd-MMM-yy") + " hasta " + dtFin.ToString("dd-MMM-yy"),
                    week = i,
                    year = year
                });
            }            

            return dates;
        }

        public static string GetDateString()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString()
                 + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString()
                 + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }
    }
}
