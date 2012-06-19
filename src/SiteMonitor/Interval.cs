using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteMonitor
{
    public static class Interval
    {
        public static DateTime LastMinutes(this int minutes)
        {
            return DateTime.Now.AddMinutes(minutes * -1);
        }

        public static DateTime LastHours(this int hours)
        {
            return DateTime.Now.AddHours(hours * -1);
        }

        public static int ToMilliseconds(this int minutes)
        {
            return (minutes * 60000);
        }

        public static DateTime FromInterval(this string interval)
        {
            try
            {
                var date = Convert.ToInt32(interval.Split('_')[0]);
                var unit = interval.Split('_')[1];

                if (unit.IndexOf("minute") > -1)
                    return date.LastMinutes();
                else if (unit.IndexOf("hour") > -1)
                    return date.LastHours();
                else
                    return 6.LastHours();
            }
            catch
            {
                return 6.LastHours();
            }
        }
    }
}
