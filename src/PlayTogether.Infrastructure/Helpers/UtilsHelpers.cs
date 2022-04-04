using System;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class UtilsHelpers
    {
        public static double GetTime(DateTime dateStart, DateTime dateFinish)
        {
            TimeSpan ts = dateFinish - dateStart;
            var timeDone = ts.TotalSeconds;
            return timeDone;
        }

        public static double GetTimeDone(DateTime date)
        {
            TimeSpan ts = DateTime.UtcNow.AddHours(7) - date;
            var timeDone = ts.TotalSeconds;
            return timeDone;
        }
    }
}