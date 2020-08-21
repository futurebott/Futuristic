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
        public static string OpenCloseTime(in int openTime,in int closeTime, in int currentHous, out string timeLabel)
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
                if((currentHous + 1) == openTime)
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
            else if(time == 12)
                return ((time) + ":00 PM");
            else
                return ((time) + ":00 AM");
        }
    }
}
