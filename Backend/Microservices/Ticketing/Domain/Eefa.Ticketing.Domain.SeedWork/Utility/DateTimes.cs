using System;

namespace Eefa.Ticketing.Domain.SeedWork.Utility
{

    public class DateTimes
    {
        public string ShamsiTime(System.DateTime DT)
        {
            System.Globalization.PersianCalendar time = new System.Globalization.PersianCalendar();
            string hour = string.Empty;
            string minute = string.Empty;
            DateTimes dtime = new DateTimes();

            if (time.GetHour(DT).ToString().Length == 1)
                hour = "0" + time.GetHour(DT).ToString();
            else hour = time.GetHour(DT).ToString();
            if (time.GetMinute(DT).ToString().Length == 1)
                minute = "0" + time.GetMinute(DT).ToString();
            else minute = time.GetMinute(DT).ToString();
            return hour + minute;

        }
        public string ShamsiDate(System.DateTime DT)
        {

            System.Globalization.PersianCalendar time = new System.Globalization.PersianCalendar();
            string Day = string.Empty;
            string Month = string.Empty;

            if (time.GetDayOfMonth(DT).ToString().Length == 1) Day = "0" + time.GetDayOfMonth(DT).ToString();
            else Day = time.GetDayOfMonth(DT).ToString();
            if (time.GetMonth(DT).ToString().Length == 1) Month = "0" + time.GetMonth(DT).ToString();
            else Month = time.GetMonth(DT).ToString();
            return time.GetYear(DT).ToString() + "/" + Month + "/" + Day;

        }
        public string ShamsiDateRTL(System.DateTime DT)
        {

            System.Globalization.PersianCalendar time = new System.Globalization.PersianCalendar();
            string Day = string.Empty;
            string Month = string.Empty;
            string hour = string.Empty;
            string minute = string.Empty;
            DateTimes dtime = new DateTimes();

            if (time.GetHour(DT).ToString().Length == 1)
                hour = "0" + time.GetHour(DT).ToString();
            else hour = time.GetHour(DT).ToString();
            if (time.GetMinute(DT).ToString().Length == 1)
                minute = "0" + time.GetMinute(DT).ToString();
            else minute = time.GetMinute(DT).ToString();

            if (time.GetDayOfMonth(DT).ToString().Length == 1) Day = "0" + time.GetDayOfMonth(DT).ToString();
            else Day = time.GetDayOfMonth(DT).ToString();
            if (time.GetMonth(DT).ToString().Length == 1) Month = "0" + time.GetMonth(DT).ToString();
            else Month = time.GetMonth(DT).ToString();
            return Day + "/" + Month + "/" + time.GetYear(DT).ToString();

        }
        public string ShamsiDateTime(System.DateTime DT)
        {
            try
            {
                System.Globalization.PersianCalendar time = new System.Globalization.PersianCalendar();
                string Day = string.Empty;
                string Month = string.Empty;
                string hour = string.Empty;
                string minute = string.Empty;
                DateTimes dtime = new DateTimes();

                if (time.GetHour(DT).ToString().Length == 1)
                    hour = "0" + time.GetHour(DT).ToString();
                else hour = time.GetHour(DT).ToString();
                if (time.GetMinute(DT).ToString().Length == 1)
                    minute = "0" + time.GetMinute(DT).ToString();
                else minute = time.GetMinute(DT).ToString();

                if (time.GetDayOfMonth(DT).ToString().Length == 1) Day = "0" + time.GetDayOfMonth(DT).ToString();
                else Day = time.GetDayOfMonth(DT).ToString();
                if (time.GetMonth(DT).ToString().Length == 1) Month = "0" + time.GetMonth(DT).ToString();
                else Month = time.GetMonth(DT).ToString();
                return time.GetYear(DT).ToString() + "/" + Month + "/" + Day + "-" + hour + ":" + minute;

            }
            catch (Exception)
            {

                return "-";
            }


        }       
        public System.DateTime MiladyDate(string shamsiDate, string Time)
        {
            int Pyear = Convert.ToInt32(shamsiDate.Substring(0, 4));

            int Pmonth = Convert.ToInt32(shamsiDate.Substring(4, 2));

            int Pday = Convert.ToInt32(shamsiDate.Substring(6, 2));

            int Hour = Convert.ToInt32(Time.Substring(0, 2));

            int min = Convert.ToInt32(Time.Substring(3, 2));

            System.Globalization.PersianCalendar Mdate = new System.Globalization.PersianCalendar();

            var miladiDate = Mdate.ToDateTime(Pyear, Pmonth, Pday, Hour, min, 0, 0, System.Globalization.GregorianCalendar.ADEra);
            return miladiDate;
        }
        public System.DateTime MiladyDateTime(string shamsiDateTime)
        {
            int Pyear = Convert.ToInt32(shamsiDateTime.Substring(0, 4));

            int Pmonth = Convert.ToInt32(shamsiDateTime.Substring(4, 2));

            int Pday = Convert.ToInt32(shamsiDateTime.Substring(6, 2));

            int Hour = Convert.ToInt32(shamsiDateTime.Substring(8, 2));

            int min = Convert.ToInt32(shamsiDateTime.Substring(10, 2));

            System.Globalization.PersianCalendar Mdate = new System.Globalization.PersianCalendar();

            var miladiDate = Mdate.ToDateTime(Pyear, Pmonth, Pday, Hour, min, 0, 0, System.Globalization.GregorianCalendar.ADEra);
            return miladiDate;
        }
        public System.DateTime MiladyDateTimeWithsinn(string shamsiDateTime)
        {
            int Pyear = Convert.ToInt32(shamsiDateTime.Substring(0, 4));

            int Pmonth = Convert.ToInt32(shamsiDateTime.Substring(5, 2));

            int Pday = Convert.ToInt32(shamsiDateTime.Substring(8, 2));

            int Hour = Convert.ToInt32(shamsiDateTime.Substring(11, 2));

            int min = Convert.ToInt32(shamsiDateTime.Substring(14, 2));

            System.Globalization.PersianCalendar Mdate = new System.Globalization.PersianCalendar();

            var miladiDate = Mdate.ToDateTime(Pyear, Pmonth, Pday, Hour, min, 0, 0, System.Globalization.GregorianCalendar.ADEra);
            return miladiDate;
        }
        public System.DateTime MiladyDateWithSign(string shamsiDate, string Time)
        {
            int Pyear = Convert.ToInt32(shamsiDate.Substring(0, 4));

            int Pmonth = Convert.ToInt32(shamsiDate.Substring(5, 2));

            int Pday = Convert.ToInt32(shamsiDate.Substring(8, 2));

            int Hour = Convert.ToInt32(Time.Substring(0, 2));

            int min = Convert.ToInt32(Time.Substring(3, 2));

            System.Globalization.PersianCalendar Mdate = new System.Globalization.PersianCalendar();

            var miladiDate = Mdate.ToDateTime(Pyear, Pmonth, Pday, Hour, min, 0, 0, System.Globalization.GregorianCalendar.ADEra);
            return miladiDate;
        }
        public string Time()
        {
            System.Globalization.PersianCalendar time = new System.Globalization.PersianCalendar();
            string hour = string.Empty;
            string minute = string.Empty;
            if (time.GetHour(System.DateTime.Now).ToString().Length == 1)
                hour = "0" + time.GetHour(System.DateTime.Now).ToString();
            else hour = time.GetHour(System.DateTime.Now).ToString();
            if (time.GetMinute(System.DateTime.Now).ToString().Length == 1)
                minute = "0" + time.GetMinute(System.DateTime.Now).ToString();
            else minute = time.GetMinute(System.DateTime.Now).ToString();
            return hour + minute;

        }
        public Int32 ShamsiToTimeStamp(string shamsiDate, string Time)
        {
            int Pyear = Convert.ToInt32(shamsiDate.Substring(0, 4));

            int Pmonth = Convert.ToInt32(shamsiDate.Substring(5, 2));

            int Pday = Convert.ToInt32(shamsiDate.Substring(8, 2));

            int Hour = Convert.ToInt32(Time.Substring(0, 2));

            int min = Convert.ToInt32(Time.Substring(3, 2));

            System.Globalization.PersianCalendar Mdate = new System.Globalization.PersianCalendar();

            var miladiDate = Mdate.ToDateTime(Pyear, Pmonth, Pday, Hour, min, 0, 0, System.Globalization.GregorianCalendar.ADEra);
            //if (Pmonth < 6)
            //{
            //    miladiDate = miladiDate.AddHours(-3);
            //    miladiDate = miladiDate.AddMinutes(-30);
            //}
            //else
            //{
            //    miladiDate = miladiDate.AddHours(-4);
            //    miladiDate = miladiDate.AddMinutes(-30);
            //}
            Int32 unixTimestamp = (Int32)(miladiDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (Pmonth <= 6)
            {
                unixTimestamp -= 16200;
            }
            else
            {
                unixTimestamp -= 12600;
            }

            return unixTimestamp;
        }
        public Int32 MiladyToTimeStamp(DateTime datetime)
        {
            Int32 unixTimestamp = (Int32)(datetime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string shamsiDate = ShamsiDateTime(datetime);
            int Pyear = Convert.ToInt32(shamsiDate.Substring(0, 4));
            int Pmonth = Convert.ToInt32(shamsiDate.Substring(5, 2));
            if (Pmonth <= 6)
            {
                unixTimestamp -= 16200;
            }
            else
            {
                unixTimestamp -= 12600;
            }
            return unixTimestamp;
        }
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

    }
}
