using System;
using System.Collections.Generic;
using System.Text;

namespace Futuristic.Services
{
    public static class Utilities
    {
        public static DateTime GetNowDateTime()
        {
            return DateTime.Now;
        }
        public static string WeekTimes(string timeString)
        {
            var weeklyString = string.Empty;
            List<string> weekTime = new List<string>();
            if (string.IsNullOrWhiteSpace(timeString))
            {
                timeString = "7:00-23:00";
            }
            var timeArr = timeString.Split(',');
            foreach (var item in timeArr)
            {
                weekTime.Add(GetTimeStr(item.Trim()));
            }
            List<string> weekDays = new List<string>();
            weekDays.Add("Sunday");
            weekDays.Add("Monday");
            weekDays.Add("Tuesday");
            weekDays.Add("Wednesday");
            weekDays.Add("Thursday");
            weekDays.Add("Friday");
            weekDays.Add("Saturday");
            if (weekTime.Count == 1)
            {
                foreach (var item in weekDays)
                {
                    weeklyString +=  weekTime[0] + " " + item;
                    weeklyString += Environment.NewLine;
                }
            }
           else if (weekTime.Count == 2)
            {
                foreach (var item in weekDays)
                {
                    if (item == "Saturday" || item == "Sunday")
                        weeklyString +=  weekTime[1] + " " + item;
                    else
                        weeklyString += weekTime[0] + " " + item;
                    weeklyString += Environment.NewLine;
                }
            }
            else if (weekTime.Count == 3)
            {
                foreach (var item in weekDays)
                {
                    if (item == "Sunday")
                        weeklyString += weekTime[2] + " " + item;
                    else if (item == "Saturday")
                        weeklyString +=  weekTime[1] + " " + item;
                    else
                        weeklyString += weekTime[0] + " " + item;
                    weeklyString += Environment.NewLine;
                }
            }
            else
            {
                
                foreach (var item in weekDays)
                {
                    if (item == "Sunday")
                        weeklyString +=  weekTime[0] +" " + item;
                    else if (item == "Monday")
                        weeklyString +=  weekTime[1] + " " + item;
                    else if (item == "Tuesday")
                        weeklyString +=  weekTime[2] + " " + item;
                    else if (item == "Wednesday")
                        weeklyString +=  weekTime[3] + " " + item;
                    else if (item == "Thursday")
                        weeklyString += weekTime[4] + " " + item;
                    else if (item == "Friday")
                        weeklyString += weekTime[5] + " " + item;
                    else if (item == "Saturday")
                        weeklyString +=  weekTime[6] + " " + item;
                    weeklyString += Environment.NewLine;
                }
               
            }
            return weeklyString;
        }
        private static string GetTimeStr(string timeStr)
        {
            string retTime = string.Empty;
            var timeStrArr = timeStr.Split('-');
            if (timeStrArr.Length > 1)
            {
                retTime += converHoursToAm(int.Parse(timeStrArr[0].Split(':')[0]));
                retTime = retTime + "-" + converHoursToAm(int.Parse(timeStrArr[1].Split(':')[0]));
            }
            return retTime;
        }
        public static string OpenCloseTime(in int openTime, in int closeTime, in int currentHous, out string timeLabel)
        {
            var hour = currentHous;
            var time = "";
            if (hour > openTime && hour < closeTime) // open
            {
                timeLabel = "Open";
                if ((hour + 1) == closeTime)
                    time = "Closing soon " + converHoursToAm(closeTime);
                else
                    time = "Closes " + converHoursToAm(closeTime);

            }
            else
            {
                timeLabel = "Close";
                if ((currentHous + 1) == openTime)
                    time = "Opening soon " + converHoursToAm(openTime);
                else if (currentHous > 12 && currentHous <= 23)
                    time = "Opens tomorrow " + converHoursToAm(openTime);
                else
                    time = "Opens " + converHoursToAm(openTime);
            }


            return time;
        }

        private static string converHoursToAm(int time)
        {
            if (time > 12)
                return ((time - 12) + ":00 PM");
            else if (time == 12)
                return ((time) + ":00 PM");
            else
                return ((time) + ":00 AM");
        }
    }
}
