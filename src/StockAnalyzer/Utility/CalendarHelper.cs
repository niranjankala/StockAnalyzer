using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Utility
{
    public class CalendarHelper
    {
        //public int[,] GetMonthStructureAjustedForWeekend(int month, int year)
        //{
        //    int[,] monthStructure = new int[6, 7];

        //    //////////////////////////////////////////////
        //    DateTime date = new DateTime(year, month, 1);
        //    int emptyCells = ((int)date.DayOfWeek + 7 - (int)DayOfWeek.Sunday) % 7;

        //    var tt = Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
        //            .Select(day => new DateTime(year, month, day)) // Map each day to a date
        //            .ToList(); // Load dates into a list

        //    int rowIndex = 0, colIndex = 0, i = 1;
        //    if (date.DayOfWeek != DayOfWeek.Sunday)
        //    {
        //        colIndex = emptyCells;
        //        i = emptyCells;
        //    }
        //    foreach (DateTime rDate in tt)
        //    {
        //        int WeekNumber = GetWeekOfMonth(rDate);
        //        if (rDate.DayOfWeek == DayOfWeek.Sunday && rowIndex == 0)
        //        {
        //            if (rowIndex != 0 && monthStructure[rowIndex - 1, 0] == 0)
        //            {
        //                monthStructure[rowIndex - 1, 0] = rDate.Day;
        //                colIndex = 0;
        //                i = 0;

        //            }
        //            else
        //            {
        //                monthStructure[rowIndex, 0] = rDate.Day;
        //                rowIndex++;
        //                i = 0;
        //            }

        //        }
        //        else
        //        {
        //            if (rDate.DayOfWeek == DayOfWeek.Sunday)
        //            {
        //                if (rowIndex != 0 && monthStructure[rowIndex - 1, 0] == 0)
        //                {
        //                    monthStructure[rowIndex - 1, 0] = rDate.Day;
        //                }
        //                else
        //                {
        //                    monthStructure[rowIndex, 0] = rDate.Day;
        //                }
        //            }
        //            else
        //            {
        //                if (colIndex == 0)
        //                {
        //                    colIndex++;
        //                }
        //                monthStructure[rowIndex, colIndex] = rDate.Day;
        //            }

        //            colIndex++;
        //            i++;
        //            if (i % 7 == 0 && i != 0)
        //            {
        //                colIndex = 0;
        //                rowIndex++;
        //                i = 0;
        //            }


        //        }
        //    }
        //    /////////////////////////////////////////








        //    // throw new NotImplementedException();
        //    //DateTime date = new DateTime(year, month, 1);
        //    //int emptyCells = ((int)date.DayOfWeek + 7 - (int)DayOfWeek.Sunday) % 7;
        //    //int days = DateTime.DaysInMonth(year, month);

        //    //int rowIndex = 0, colIndex = 0;
        //    //for (int i = 0; i != 42; i++)
        //    //{
        //    //    if (DayOfWeek.Sunday==DayOfWeek.Sunday && rowIndex==0)
        //    //    {
        //    //        monthStructure[0, 0] = 0;
        //    //        i = 7;
        //    //    }

        //    //    if (i % 7 == 0)
        //    //    {
        //    //        if (i > 0)
        //    //        {
        //    //            colIndex = 0;
        //    //            rowIndex = rowIndex + 1;
        //    //            //Console.Write(Environment.NewLine);
        //    //        }
        //    //    }
        //    //    if (i < emptyCells || i >= emptyCells + days)
        //    //    {
        //    //        //Console.Write("- ");
        //    //        monthStructure[rowIndex, colIndex] = 0;
        //    //    }
        //    //    else
        //    //    {
        //    //        //Console.Write(date.Day + " ");
        //    //        if (date.DayOfWeek == DayOfWeek.Sunday)
        //    //        {
        //    //            monthStructure[rowIndex, 0] = date.Day;
        //    //        }
        //    //        else
        //    //        {
        //    //            if (colIndex == 0)
        //    //            {
        //    //                colIndex++;
        //    //            }
        //    //            monthStructure[rowIndex, colIndex] = date.Day;
        //    //        }

        //    //        date = date.AddDays(1);
        //    //    }
        //    //    colIndex++;
        //    //}

        //    return monthStructure;
        //}

        public int[,] GetMonthStructure(int month, int year, DayOfWeek firstDayOfWeek)
        {
            int[,] monthStructure = new int[6, 7];

            //////////////////////////////////////////////
            DateTime date = new DateTime(year, month, 1);
            int emptyCells = ((int)date.DayOfWeek + 7 - (int)firstDayOfWeek) % 7;

            var tt = Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                    .Select(day => new DateTime(year, month, day)) // Map each day to a date
                    .ToList(); // Load dates into a list

            int rowIndex = 0, colIndex = 0;
            if (date.DayOfWeek != firstDayOfWeek)
            {
                colIndex = emptyCells;
            }

            foreach (DateTime rDate in tt)
            {
                int WeekNumber = GetWeekOfMonth(rDate, firstDayOfWeek);
                monthStructure[rowIndex, colIndex] = rDate.Day;
                colIndex++;

                if (colIndex % 7 == 0 && colIndex != 0)
                {
                    colIndex = 0;
                    rowIndex++;
                }
            }
            return monthStructure;
        }

        public int[,] GetCalendarMonthStructure(int month, int year, DayOfWeek firstDayOfWeek)
        {
            int[,] monthStructure = new int[6, 7];

            //////////////////////////////////////////////
            DateTime date = new DateTime(year, month, 1);
            int emptyCells = ((int)date.DayOfWeek + 7 - (int)firstDayOfWeek) % 7;

            var tt = Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                    .Select(day => new DateTime(year, month, day)) // Map each day to a date
                    .ToList(); // Load dates into a list

            int rowIndex = 0, colIndex = 0;
            if (date.DayOfWeek != firstDayOfWeek)
            {
                colIndex = emptyCells;
            }

            if (month != 1)
            {
                int previousMonthDaysCount = DateTime.DaysInMonth(year, month - 1);
                for (int cnt = emptyCells - 1; cnt >= 0; cnt--, previousMonthDaysCount--)
                {
                    monthStructure[0, cnt] = previousMonthDaysCount;
                }
            }

            foreach (DateTime rDate in tt)
            {
                int WeekNumber = GetWeekOfMonth(rDate, firstDayOfWeek);
                monthStructure[rowIndex, colIndex] = rDate.Day;
                colIndex++;

                if (colIndex % 7 == 0 && colIndex != 0)
                {
                    colIndex = 0;
                    rowIndex++;
                }
            }
            if (month != 12 && colIndex > 0)
            {
                int i = 1;
                for (int cnt = colIndex; cnt < 7; cnt++, i++)
                {
                    monthStructure[rowIndex, cnt] = i;
                }
            }

            return monthStructure;
        }


        public DateTime? GetNthWeekDayInMonth(DateTime firstDate, int nth, DayOfWeek weekday)
        {
            var daysInMonth = DateTime.DaysInMonth(firstDate.Year, firstDate.Month);
            var dates = Enumerable.Range(1, daysInMonth)
                .Select(n => new DateTime(firstDate.Year, firstDate.Month, n))
                .Where(date => date.DayOfWeek == weekday).ToArray();
            if (nth <= dates.Length)
            {
                return dates[nth - 1];
            }
            return null;

        }

        public DateTime? GetNthWeekDayInMonth(DateTime firstDate, string occurancePosition, DayOfWeek weekday)
        {
            int nth = 0;

            var daysInMonth = DateTime.DaysInMonth(firstDate.Year, firstDate.Month);
            var dates = Enumerable.Range(1, daysInMonth)
                .Select(n => new DateTime(firstDate.Year, firstDate.Month, n))
                .Where(date => date.DayOfWeek == weekday).ToArray();

            if (int.TryParse(occurancePosition, out nth))
            {
                if (nth <= dates.Length)
                {
                    return dates[nth - 1];
                }
            }
            else
            {
                if (occurancePosition.ToLower().Contains("last"))
                {
                    var count = dates.Length;
                    return dates[count - 1];
                }
            }

            return null;
        }


        public int GetWeekOfMonth(DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            DateTimeFormatInfo currentDateFormatInfo = DateTimeFormatInfo.CurrentInfo;
            Calendar calendar = currentDateFormatInfo.Calendar;


            DateTime first = new DateTime(datetime.Year, datetime.Month, 1);
            return GetWeekOfYear(datetime, calendar, firstDayOfWeek) - GetWeekOfYear(first, calendar, firstDayOfWeek) + 1;
        }
        private int GetWeekOfYear(DateTime time, Calendar calendar, DayOfWeek firstDayOfWeek)
        {
            return calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, firstDayOfWeek);
        }

        public int GetNumberOfWeekInMonth(int month, int year, DayOfWeek firstDayOfWeek)
        {
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            System.Globalization.Calendar calendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
            int lastWeek = calendar.GetWeekOfYear(lastDayOfMonth, System.Globalization.CalendarWeekRule.FirstDay, firstDayOfWeek);
            int firstWeek = calendar.GetWeekOfYear(firstDayOfMonth, System.Globalization.CalendarWeekRule.FirstDay, firstDayOfWeek);
            return lastWeek - firstWeek + 1;
        }

        public int GetWeekNumberOfYear(DateTime date, DayOfWeek firstDayOfWeek)
        {
            var cal = CultureInfo.CurrentCulture.Calendar;
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, firstDayOfWeek);
        }

        public int GetWeekNumberOfYear1(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetDayOfYear(date);
        }

        public DateTime StartDateOfWeek(int year, int weekNum, DayOfWeek firstDayOfWeek)
        {

            DateTime firstDayofYear = new DateTime(year, 1, 1);
            int daysOffset = firstDayOfWeek - firstDayofYear.DayOfWeek;
            DateTime firstWeekDay = firstDayofYear.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstDayofYear, CalendarWeekRule.FirstDay, firstDayOfWeek);

            if (firstWeek <= 1) { weekNum -= 1; }
            DateTime result = firstWeekDay.AddDays(weekNum * 7);
            return result;
        }

        public int GetLastDayOfWeek(int weekIndex, int[,] monthDisplayStructure)
        {
            int weekEndDay = 0;
            for (int dayIndex = 0; dayIndex < 7; dayIndex++)
            {
                if (weekEndDay < monthDisplayStructure[weekIndex, dayIndex])
                {
                    //Check used for creating calendar month structure as per the week rules
                    if (weekIndex == 0 && monthDisplayStructure[weekIndex, dayIndex] / 20 >= 1)
                        continue;
                    weekEndDay = monthDisplayStructure[weekIndex, dayIndex];

                }
            }
            return weekEndDay;

        }

        public int GetTotalNumberOfWeeksInMonth(int month, int year, DayOfWeek firstDayOfWeek)
        {

            int[,] monthDisplayStructure = GetMonthStructure(month, year, firstDayOfWeek);

            int weeksInMonth = GetNumberOfWeekInMonth(month, year, firstDayOfWeek);

            int actualWeeksMonth = 0;

            for (int weekIndex = 0; weekIndex < weeksInMonth; weekIndex++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (monthDisplayStructure[weekIndex, j] != 0)
                    {
                        actualWeeksMonth++;
                        break;
                    }
                }
            }


            if (actualWeeksMonth != weeksInMonth)
            {
                weeksInMonth = actualWeeksMonth;
            }

            try
            {
                if (weeksInMonth != 6)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (monthDisplayStructure[weeksInMonth, i] != 0)
                        {
                            weeksInMonth++;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return weeksInMonth;
        }
    }
}
